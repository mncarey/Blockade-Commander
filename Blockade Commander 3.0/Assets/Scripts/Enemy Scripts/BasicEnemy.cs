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
    

    //---- Enemy Basic Var ----//
    public float speed = 5;
    public float currentSpeed;
    public int lives = 5;
    public int maxLives = 5;
    public int dmg = 0;
    public float reachDistance = 5f;
    public int goldValue = 10;
    private int killValue = 1;
    
    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] WaveProgressBar waveProgressBar;

    //---- Coroutines ----//
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
        
        healthBar.UpdateHealthBar(lives, maxLives);
        
        
        
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
            currentSpeed = speed;
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

    public void takeDamage()
    {
        lives--;
        healthBar.UpdateHealthBar(lives, maxLives);
        waveProgressBar.UpdateHealthBar();
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
                Debug.Log("setting target to taunt");

            }


        }
        if (other.gameObject.tag == "Damage Range")
        {
            if (damageEnemyRoutine == null)
            {
                
                isTakingDamage = true;
                damageEnemyRoutine = StartCoroutine(DamageOverTime());
                

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
                
                isTakingDamage = true;
                damageEnemyRoutine = StartCoroutine(DamageOverTime());
                
                
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


    private void MoveTowardsTarget()
    {
        // Stop any attack routines when moving again
        StopAllAttackCoroutines();

        Vector3 direction = (currentTarget.position - rb.position).normalized;
        Vector3 moveVelocity = direction * speed;
        rb.linearVelocity = new Vector3(moveVelocity.x, rb.linearVelocity.y, moveVelocity.z);
    }

    //Handles damaging target
    private void HandleTargetInRange()
    {
        rb.linearVelocity = Vector3.zero;

        // Try TauntTower
        tauntRef = currentTarget.GetComponent<TauntTower>();
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
    private IEnumerator DamageOverTime()
    {
        while (isTakingDamage)
        {
            
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
