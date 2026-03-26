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
    
    private Transform currentTarget;

    private TauntTower tauntRef;
    private Wall wallRef;
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

    private float unitPriority;
    
    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] WaveProgressBar waveProgressBarRef;

    //---- Coroutines ----//
    private Coroutine damageEnemyRoutine;
    private Coroutine damageWallRoutine;
    private Coroutine damageTauntRoutine;
  
    //List to hold the fortifications within the scene
    public List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        waveProgressBarRef = FindObjectOfType<WaveProgressBar>(true);
        increaseDiff = FindObjectOfType<IncreaseDifficulty>();
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
        float separationRadius = 4f;
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

            //if the friends priority is higher than this, move this
            if(friendScript != null && friendScript.unitPriority < unitPriority)
            {
                //find the push direction
                Vector3 pushDir = transform.position - friend.transform.position;

                pushDir.y = 0f;

                float dist = pushDir.magnitude;

                if (dist <= separationRadius)
                {
                    Vector3 slideDir = Vector3.Cross(Vector3.up, pushDir);
                    // The closer they are, the harder they push // normalized so speed is constant
                    totalPush += (pushDir.normalized + slideDir.normalized * 2f) / (pushDir.magnitude + 0.01f);
                }
            }
           
            


        }
        return totalPush;

    }

    public void TakeDamage(int dmg)
    {
        lives--;
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


    private void OnTriggerEnter(Collider other)
    {



        //if this enters the taunt range of a taunt tower
        if (other.gameObject.CompareTag("Taunt Range"))
        {
            if (other.transform.parent != null)
            {
                //set this game object as current target
                currentTarget = other.transform;
                //Debug.Log("setting target to taunt");

            }


        }
        
        if (other.gameObject.tag == "killZone")
        {
            
            //taunt tower taking damage
            rb.linearVelocity = Vector3.zero;
            
            tauntRef = other.gameObject.GetComponentInParent<TauntTower>();
            
            if (tauntRef != null && damageTauntRoutine == null)
            {
                if (this.name == "Sloop Enemy")
                {
                    
                    tauntRef.SloopDamage();
                    lives = 0;
                }
                else
                {
                    damageTauntRoutine = StartCoroutine(DamageTauntRoutine());
                }
                    
            }

            //the enemy taking damage
            if(damageEnemyRoutine == null)
            {
                
                
               
                
                
            }            
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("killZone"))
        {
            //if the taunt tower despawns before the enemy can leave the killZone, the coroutine can't stop
            if (damageEnemyRoutine != null)
            {
                StopCoroutine(damageEnemyRoutine);
                damageEnemyRoutine = null;
            }



            if (damageTauntRoutine != null)
            {
                StopCoroutine(damageTauntRoutine);
                damageTauntRoutine = null;
            }

            tauntRef = null;//reset variable
        }
        if (other.gameObject.CompareTag("wallZone"))
        {
            if(damageWallRoutine != null)
            {
                StopCoroutine(damageWallRoutine);
                damageWallRoutine = null;
            }

            wallRef = null;//reset variable 
        }
    }


    private void MoveTowardsTarget()
    {
        // Stop any attack routines when moving again
        StopAllAttackCoroutines();
        float nudgeStrength = 5f;
        Vector3 direction = (currentTarget.position - rb.position).normalized;

        Vector3 nudgeDir = EnemyNudge();
        Vector3 finalDir = (direction + (nudgeDir * nudgeStrength)).normalized;
        Vector3 moveVelocity = finalDir * speed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    //Handles damaging target
    private void HandleTargetInRange()
    {
        rb.linearVelocity = Vector3.zero;

        // Try TauntTower
        tauntRef = currentTarget.GetComponentInParent<TauntTower>();
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.0f); // Match your separationRadius
    }
}
