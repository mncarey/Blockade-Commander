using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using static UnityEditor.PlayerSettings;
using UnityEngine.Rendering;
using UnityEngine.UIElements.Experimental;

public class BasicEnemy : MonoBehaviour
{
    
    private Transform currentTarget;

    private TauntTower tauntRef;
    private Wall wallRef;
    public Wave_Spawner_BasicEnemy waveSpawnerRef;
    

    //---- Enemy Basic Var ----//
    public float speed = 5;
    public float currentSpeed;
    public float attackSpeed = 0f;
    public int lives = 5;
    public int maxLives = 5;
    public int dmg = 0;
    public float reachDistance = 5;
    private int goldValue = 10;
    private int killValue = 1;
    
    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;

    private Coroutine damageEnemyRoutine;
    private Coroutine damageWallRoutine;
    private Coroutine damageTauntRoutine;

    private bool isTakingDamage;

    
    
    //List to hold the fortifications within the scene
    public List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }
       
    //then if this enemy is within "range" of the fortification - set speed to 0 - execute damage towards the fortification

    //when the fortification health reaches 0 set next target
    //set speed back to default

    //This should keep going until enemies are defeated
    void Start()
    {
        Debug.Log("Time scale: " + Time.timeScale);
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

    // Update is called once per frame
    void FixedUpdate()
    {

        if (currentTarget == null)
        {
            //If this is within taunt field - set that as current target

            //If not, continue as normal
            FindNewTarget();
            return;

        }

        
        if (currentTarget != null)
        {
            Debug.Log(currentTarget.position);

            Vector3 direction = (currentTarget.position - rb.position).normalized;
            Debug.Log(direction);
            Vector3 moveVelocity = direction * currentSpeed;
            Debug.Log(moveVelocity);
            // Preserve gravity (Y velocity)
            rb.linearVelocity = new Vector3(
                moveVelocity.x,
                rb.linearVelocity.y,
                moveVelocity.z
            );
            Debug.Log(rb.linearVelocity);
            // transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, currentSpeed * Time.deltaTime);//move towards the target
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
            //Debug.Log(gameObject.name + " priority targeting Taunt Tower");
        }
        else
        {
            //target closest target
            currentTarget = targetTag.OrderBy(obj => Vector3.Distance(transform.position, obj.transform.position)).First().transform;
            Debug.Log(gameObject.name + " targeting closest fortification");
        }
        

    }

    public void takeDamage()
    {
        lives--;
        healthBar.UpdateHealthBar(lives, maxLives);
        if (lives <= 0)
        {
            Destroy(gameObject);
           
           //Debug.Log("Enemy goldValue is: " + goldValue);
           ResourceUI.instance.UpdateGold(goldValue);
           ResourceUI.instance.UpdateKills(killValue);
        }
    }

    private void OnDestroy()
    {
        if(waveSpawnerRef != null)
        {
            waveSpawnerRef.enemiesAlive--;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        //if this enters the taunt range of a taunt tower
        if (other.gameObject.CompareTag("Taunt Range"))
        {
            if(other.transform.parent != null)
            {
                //set this game object as current target
                currentTarget = other.transform;
                Debug.Log("Within Taunt Range");
            }
            

        }
        if (other.gameObject.tag == "killZone")
        {

            //taunt tower taking damage
            currentSpeed = attackSpeed;
            tauntRef = other.gameObject.GetComponentInParent<TauntTower>();
            if (tauntRef != null && damageTauntRoutine == null /* && tower is within range of enemy*/)
            {
                damageTauntRoutine = StartCoroutine(DamageTauntRoutine());
            }

            //the enemy taking damage
            if(damageEnemyRoutine == null)
            {
                Debug.Log("damage enemy");
                isTakingDamage = true;
                damageEnemyRoutine = StartCoroutine(DamageOverTime());
                currentSpeed = attackSpeed;
                
            }

            //if the taunt tower is destroyed, stop the enemy from taking damage
            if (tauntRef.isDed == true)
            {
                Debug.Log("EXIT kilLZone");
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

    private void OnTriggerExit(Collider other)
    {
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

    private IEnumerator DamageOverTime()
    {
        while (isTakingDamage && tauntRef != null)
        {
            Debug.Log("Damage enemy double");
                takeDamage();
                yield return new WaitForSeconds(1f);
        }
        currentSpeed = speed;
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
