using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using static UnityEditor.PlayerSettings;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;
using UnityEngine.InputSystem;

public class BasicEnemy : MonoBehaviour
{
    
    private Transform currentTarget;

    //---- References ----//
    private TauntTower tauntRef;
    private Wall wallRef;
    //private Cannon cannonRef;
    //private Mortar mortarRef;
    public Wave_Spawner_BasicEnemy waveSpawnerRef;
    

    //---- Enemy Basic Var ----//
    public float speed = 5;
    public float currentSpeed;
    public float attackSpeed = 0f;
    public int lives = 5;
    public int maxLives = 5;
    public int dmg = 0;
    public float reachDistance = 5;
    public int goldValue = 10;
    public int killValue = 1;
    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;

    //---- Coroutine Variables ----//
    private Coroutine damageEnemyRoutine;
    private Coroutine damageWallRoutine;
    private Coroutine damageTauntRoutine;

    private bool isTakingDamage;

    
    
    //List to hold the fortifications within the scene
    public List<GameObject> targets = new List<GameObject>();

    
    //Initialize
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
       

    
    void Start()
    {
        //Set up the health bar to use the local lives and max lives of this enemy
        healthBar.UpdateHealthBar(lives, maxLives);

        currentSpeed = speed;
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

    // Update is called once .02 seconds, 50 per second
    void FixedUpdate()
    {
        //Debug.Log("currentTarget " + currentTarget);
        if (currentTarget == null)
        {
            //If this is within taunt field - set that as current target

            //If not, continue as normal
            rb.linearVelocity = Vector3.zero;
            FindNewTarget();
            
            

        }

        //Move toward the current target
        else
        {
            Vector3 direction = (currentTarget.position - rb.position).normalized;           
            Vector3 moveVelocity = direction * currentSpeed;           
            rb.linearVelocity = new Vector3(moveVelocity.x,rb.linearVelocity.y,moveVelocity.z);            
        }
        
           
    }

    private void FindNewTarget()
    {
        currentSpeed = speed;
        //remove all null references
        targets.RemoveAll(t => t == null);

        //Find all new references if there are fortifications within the scene
        GameObject[] targetTag = GameObject.FindGameObjectsWithTag("Fortification");
        
        //If there are no targets left
        if (targetTag.Length == 0)
        {
            currentTarget = null;
            speed = 0;
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
            //Debug.Log(gameObject.name + " targeting closest fortification");
        }
        

    }

    public void takeDamage()
    {
        lives--;
        healthBar.UpdateHealthBar(lives, maxLives);
        if (lives <= 0)
        {
            //Update resource UI
            ResourceUI.instance.UpdateGold(goldValue);
            ResourceUI.instance.UpdateKills(killValue);

            //Destroy this enemy
            Destroy(gameObject);           
        }
    }

    private void OnDestroy()
    {
        if(waveSpawnerRef != null)
        {
            waveSpawnerRef.enemiesAlive--;
        }
    }

    //Trigger interactions when this enemy is within a trigger
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered: " + other?.gameObject?.tag); This tests what trigger this enemy is within 



        //if this enters the taunt range of a taunt tower
        if (other.gameObject.CompareTag("Taunt Range"))
        {
            if(other.transform.parent != null)
            {
                //set this game object as current target
                currentTarget = other.transform;
                
            }
            

        }
        //If the enemy enters the kill zone of a fortification
        if (other.gameObject.tag == "killZone")
        {

            //taunt tower taking damage
            rb.linearVelocity = Vector3.zero;
            tauntRef = other.gameObject.GetComponentInParent<TauntTower>();
            if (tauntRef != null && damageTauntRoutine == null)
            {
                damageTauntRoutine = StartCoroutine(DamageTauntRoutine());
            }

            //the enemy taking damage
            if(damageEnemyRoutine == null)
            {
                
                isTakingDamage = true;
                damageEnemyRoutine = StartCoroutine(DamageOverTime());

                //Stop moving
                currentSpeed = attackSpeed;
                
            }

            //if the taunt tower is destroyed, stop the enemy from taking damage
            if (tauntRef.isDed == true)
            {
                
                isTakingDamage = false;
                StopCoroutine(damageEnemyRoutine);
                damageEnemyRoutine = null;
              
            }
        }

        //wall taking damage
        if(other.gameObject.tag == "wallZone")
        {
            currentSpeed = attackSpeed;
            wallRef = other.gameObject.GetComponentInParent<Wall>();

            if (wallRef != null && damageWallRoutine == null)
            {
                
                wallRef.takeDamage();
                damageWallRoutine = StartCoroutine(DamageWallRoutine());
            }
        }
    }

    //When the enemy leaves the trigger area, mostly a failsafe for null enemy, stopping damage coroutines
    private void OnTriggerExit(Collider other)
    {
        //If the killzone despawns while the enemy is within it
        if(other.gameObject.CompareTag("killZone"))
        {
            //if the taunt tower despawns before the enemy can leave the killZone, the coroutine can't stop
                isTakingDamage = false;
                StopCoroutine(damageEnemyRoutine);
                damageEnemyRoutine = null;
            
            

            if(damageTauntRoutine != null)
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

    //---- COROUTINES ----//
    private IEnumerator DamageOverTime()
    {
        while (isTakingDamage && tauntRef != null)
        {
            Debug.Log("Damage enemy double");
                takeDamage();
                yield return new WaitForSeconds(1f);
        }
        
        damageEnemyRoutine = null;
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
        currentSpeed = speed;
        damageTauntRoutine = null;//damage taunt routine resets for the next tower to be damaged
    }
}
