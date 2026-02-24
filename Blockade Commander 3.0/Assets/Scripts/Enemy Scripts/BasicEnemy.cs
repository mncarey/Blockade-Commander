using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using static UnityEditor.PlayerSettings;

public class BasicEnemy : MonoBehaviour
{
    private Transform target1;
    private Transform target2;
    private Transform currentTarget;

    private TauntTower tauntRef;
    private Wall wallRef;

    public float reachDistance = 5;

    //public Transform target2;
    public float speed = 5;
    public int lives = 5;
    public int maxLives = 5;
    public int dmg = 0;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
    //After that set priority
    //Taunt tower if within x units
    //everything else if taunt tower is not in range

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

        //Debug.Log("The closest fortification is " + targets[0].name);
        

        //---- Set Current Target to Closest ----//
        if(targets.Count > 0 && targets[0]  != null)
        {
            currentTarget = targets[0].transform;
            //Debug.Log(gameObject.name + " is targeting " + currentTarget.name);
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {
        target1 = GameObject.FindWithTag("EnemyTarget1")?.transform;

        
        if(currentTarget != null)
        {
            

            transform.position = Vector3.MoveTowards(transform.position, 
            currentTarget.position, speed * Time.deltaTime);//move towards the target
        }
           
    }

    public void takeDamage()
    {
        lives--;
        healthBar.UpdateHealthBar(lives, maxLives);
        if (lives <= 0)
        {
            Destroy(gameObject);

           Debug.Log("Enemy goldValue is: " + goldValue);
           ResourceUI.instance.UpdateGold(goldValue);
           ResourceUI.instance.UpdateKills(killValue);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "killZone")
        {
            Debug.Log("ENTER killZone");
            //taunt tower taking damage
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
            }

            if(tauntRef.isDed == true)//if the taunt tower is destroyed, stop the enemy from taking damage
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
        damageTauntRoutine = null;//damage taunt routine resets for the next tower to be damaged
    }
}
