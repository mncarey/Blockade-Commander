using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
//using static UnityEditor.PlayerSettings;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;
using UnityEngine.InputSystem;

public class BasicEnemy : MonoBehaviour
{
    //enemy tracker
    public static List<BasicEnemy> AllEnemies = new List<BasicEnemy>();

    private Transform currentTarget;

    private TauntTower tauntRef;
    private Cannon cannonRef;
    private Wall wallRef;
    private Mortar mortarRef;
    public Wave_Spawner_BasicEnemy waveSpawnerRef;
    private PlayerFortress fortRef;

    public EnemyInfo enemyInfo;
    public IncreaseDifficulty increaseDiff;

    //Enemy Layer//
    public LayerMask enemyLayer;


    //---- Enemy Basic Var ----//
    public float startSpeed = 0f;
    public float speed = 10;
    
    public float lives = 5f;
    public float maxLives = 5;
   
    public float reachDistance = 5f;
    public int goldValue = 10;
    private int killValue = 1;

    public bool canMove = false;

    private float unitPriority;

    private float retargetTimer = 0f;
    private float retargetInterval = 1f;
    private float rotationSpeed = 5f;

    private Vector3 smoothNudge;

    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] WaveProgressBar waveProgressBarRef;

    //---- Coroutines ----//
 
    private Coroutine damageWallRoutine;
    private Coroutine damageTauntRoutine;
    private Coroutine damageCannonRoutine;
    private Coroutine damageMortarRoutine;
  
    //List to hold the fortifications within the scene
    public List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        waveProgressBarRef = FindObjectOfType<WaveProgressBar>(true);
        increaseDiff = FindObjectOfType<IncreaseDifficulty>();


    }
    private void OnEnable()
    {
        if(!AllEnemies.Contains(this))
            AllEnemies.Add(this);
    }

    private void OnDisable()
    {
        AllEnemies.Remove(this);
    }
    void Start()
    {
        
        //Assign unit priority for movement interactions
        switch (true)
        {
            case bool when gameObject.name.Contains("Brigantine Enemy"):
                unitPriority = Random.Range(0f, 100f);
                
                break;

            case bool when gameObject.name.Contains("Galleon Ranged Enemy"):
                unitPriority = Random.Range(201f, 300f);
                
                break;

            case bool when gameObject.name.Contains("Sloop Enemy"):
                unitPriority = Random.Range(101f, 200f);
                
                break;

            default:
                break;
            
        }

        //maxLives = lives * increaseDiff.Instance.multiplier;
        //Debug.Log("Increased Health to: " + maxLives);
        lives = maxLives;
        healthBar.UpdateHealthBar(lives, maxLives);
        waveProgressBarRef.UpdateHealthBar();
        
        
        
        //---- Get Targets For Attack Info ----//
        //Finds objects in scene with this tag
        GameObject[] targetTag = GameObject.FindGameObjectsWithTag("Fortification");
        //add game objects tagged "fortification" to the target list
        targets.AddRange(targetTag);

        //---- Get transform information and figure out relative distance, then add to list ----//

        targets = targets.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).ToList();


        //---- Set Current Target to Closest ----//
        if(targets.Count > 0 && targets[0]  != null)
        {
            currentTarget = targets[0].transform;
            
        }    
    }

    // Update is called once per 0.02 seconds or 50 per second
    void FixedUpdate()
    {
        retargetTimer += Time.deltaTime;

        if (retargetTimer >= retargetInterval)
        {
            FindNewTarget();
            retargetTimer = 0f;
        }

        if (currentTarget == null)
        {
            StopAllAttackCoroutines();
            rb.linearVelocity = Vector3.zero;
            
            FindNewTarget();
            if (currentTarget == null) StopAllAttackCoroutines();
                
                return;

        }

        //gets the distance
        float sqrDistance = (transform.position - currentTarget.position).sqrMagnitude;

        if (sqrDistance <= reachDistance * reachDistance)
        {
            HandleTargetInRange();
        }

        else
        {
            MoveTowardsTarget();
        }
        

    }

    private void FindNewTarget()
    {
        
        //remove all null references
        targets.RemoveAll(t => t == null);

        //Find all new references if there are fortifications within the scene
        GameObject[] targetTag = GameObject.FindGameObjectsWithTag("Fortification");
        
        //If there are no targets left
        if (targetTag.Length == 0)
        {
            GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
            currentTarget = playerRef.transform;
            
            if (currentTarget == null) return;
            return;
        }

        //Collect a collection of targetTag, for each object within targetTag and checks all not null
        //Checks if there is a taunt tower component attached
        //Adds to a list
        var tauntTowers = targetTag.Where(obj => obj != null && obj.GetComponent<TauntTower>() != null).ToList();

        //If there are taunt towers
        if (tauntTowers.Count > 0)
        {
            //Set priority target to taunt tower
            currentTarget = tauntTowers.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).First().transform;
            
        }
        else
        {
            //target closest target
            currentTarget = targetTag.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).First().transform;           
        }        
    }
    
    //Handles enemies "nudging" eachother

    private Vector3 EnemyNudge()
    {
        float separationRadius = 6f;
        Vector3 totalPush = Vector3.zero;

        //Gets enemies within this range
        Collider[] friends = Physics.OverlapSphere(transform.position, separationRadius, enemyLayer);

        //foreach nearby enemy
        foreach (var friend in friends)
        {
            //reference the friends script
            BasicEnemy friendScript = friend.GetComponent<BasicEnemy>();
            // dont nudge yourself
            if (friend.gameObject == gameObject) continue;

            //if the friends priority is higher than this, react
            if(friendScript != null && friendScript.unitPriority < unitPriority - 5f)
            {
                //find the other enemy relative to this
                Vector3 pushDir = transform.position - friend.transform.position;

                //keep everything flat
                pushDir.y = 0f;

                //distance between this enemy and the other one
                float dist = pushDir.magnitude;

                //Only apply avoidance if not overlapping exactly or within the separation radius
                if (dist > 0f && dist <= separationRadius)
                {
                    Vector3 slideDir = Vector3.Cross(Vector3.up, pushDir);

                    //create falloff, strength gets weaker or stronger depending upon the distance
                    float pushStrength = Mathf.Clamp01((separationRadius - dist) / separationRadius);
                    
                    //set the magnitude of the target as 1 (normalized) , the direction you want to move
                    Vector3 toTarget = (currentTarget.transform.position - transform.position).normalized;

                    //cross is a perpendicular direction, the cross product, toTarget is forward direction, Vector3.up is the other part of the cross, resulting in a sideways vector
                    Vector3 sideDir = Vector3.Cross(Vector3.up, toTarget);
                    //decide which direction to go, left or right 
                    //Dot shows how aligned the directions are, if same, if opposite, if perpendicular, is it on the right or left essentially
                    //Sign returns if the value is positive negative or zero, so it returns left or right, -1 or 1, left or right
                    float side = Mathf.Sign(Vector3.Dot(pushDir, sideDir));
                    //no direct pushback
                    Vector3 avoidance = sideDir * side;
                    totalPush += avoidance * pushStrength;
                }

                
            }
           
            


        }

        //deadzone clamp, ignores jittery, tiny movements
        if (totalPush.magnitude < 0.1f)
        {
            return Vector3.zero;
        }
        
           //limit max strength so it doesn't rapidly change speed/direction
            return Vector3.ClampMagnitude(totalPush, 1f);
        

    }

    public void TakeDamage(int dmg)
    {
        lives = lives - dmg;
        healthBar.UpdateHealthBar(lives, maxLives);
        waveProgressBarRef.UpdateHealthBar();
        if (lives <= 0)
        {
            Destroy(gameObject);
           
           
           ResourceUI.instance.UpdateGold(goldValue);
           ResourceUI.instance.UpdateKills(killValue);

           waveSpawnerRef.EnemyDied();
        }
    }



    private void MoveTowardsTarget()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        
        // Stop any attack routines when moving again
        StopAllAttackCoroutines();
        float nudgeStrength = 5f;
        Vector3 direction = (currentTarget.position - rb.position).normalized;

        Vector3 nudgeDir = EnemyNudge();
        smoothNudge = Vector3.Lerp(smoothNudge, nudgeDir, 5f * Time.deltaTime);
       Vector3 finalDir = (direction + (smoothNudge * nudgeStrength)).normalized;

        //Sets the rotation to look at the current target.
        Vector3 lookDir = finalDir;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDir) * Quaternion.Euler(0, 270, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 moveVelocity = finalDir * speed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    public void SetMovement(bool enabled)
    {
        canMove = enabled;

        if (!canMove)
        {
            rb.linearVelocity = Vector3.zero; // immediately stop
        }
    }

    //Handles damaging target
    private void HandleTargetInRange()
    {

        //Reset references

        tauntRef = null;
        cannonRef = null;
        wallRef = null;
        mortarRef = null;

        rb.linearVelocity = Vector3.zero;

        // Try TauntTower
        tauntRef = currentTarget.GetComponentInParent<TauntTower>();
        cannonRef = currentTarget.GetComponentInParent<Cannon>();
        mortarRef = currentTarget.GetComponentInParent<Mortar>();
        //if there there is a taunt tower set as the current target
        if (tauntRef != null)
        { 
            //if it is not already being damaged
            if (damageTauntRoutine == null) /* Start damage */ damageTauntRoutine = StartCoroutine(DamageTauntRoutine()); 

            return;
        }

        // Try Wall
        wallRef = currentTarget.GetComponent<Wall>();
        if (wallRef != null)
        {
            if (damageWallRoutine == null)
                damageWallRoutine = StartCoroutine(DamageWallRoutine());

            return;
        }

        //try cannon
        if(cannonRef != null)
        {
            if(damageCannonRoutine == null) damageCannonRoutine = StartCoroutine(DamageCannonRoutine());
            return;
        }

        //try mortar
        if (mortarRef != null)
        {
            if (damageMortarRoutine == null) damageMortarRoutine = StartCoroutine(DamageMortarRoutine());
            return;
        }
        //if the current target is the player fortress
        if(currentTarget.tag == "Player")
        {
            fortRef = FindObjectOfType<PlayerFortress>();
            fortRef.takeDamage();
        }
    }

    

    //stops all damage routines if they are ongoing
    private void StopAllAttackCoroutines()
    {
        if (damageTauntRoutine != null)
        {
            StopCoroutine(damageTauntRoutine);
            damageTauntRoutine = null;
        }

        if (damageWallRoutine != null)
        {
            StopCoroutine(damageWallRoutine);
            damageWallRoutine = null;
        }

        if(damageCannonRoutine != null)
        {
            StopCoroutine(damageCannonRoutine);
            damageCannonRoutine = null;
        }

        if(damageMortarRoutine != null)
        {
            StopCoroutine(damageMortarRoutine);
            damageMortarRoutine = null;
        }

        
    }
    

    private IEnumerator DamageWallRoutine()
    {
        while (wallRef != null)
        {
            wallRef.takeDamage();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator DamageTauntRoutine()
    {
        while (tauntRef != null)
        {
            tauntRef.takeDamage();
            yield return new WaitForSeconds(1f);
        }
        
        damageTauntRoutine = null;//damage taunt routine resets for the next tower to be damaged
    
    }

    private IEnumerator DamageCannonRoutine()
    {
        while (cannonRef != null)
        {
            cannonRef.takeDamage();
            yield return new WaitForSeconds(1f);
        }

        damageCannonRoutine = null;//damage taunt routine resets for the next tower to be damaged

    }

    private IEnumerator DamageMortarRoutine()
    {
        while (mortarRef != null)
        {
            mortarRef.takeDamage();
            yield return new WaitForSeconds(1f);
        }

        damageMortarRoutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.0f); // Match your separationRadius
    }
}
