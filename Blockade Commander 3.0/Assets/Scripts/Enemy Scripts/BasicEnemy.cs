using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        healthBar.UpdateHealthBar(lives, maxLives);
        
        target1 = GameObject.FindWithTag("EnemyTarget1")?.transform;
        target2 = GameObject.FindWithTag("EnemyTarget2")?.transform;

        currentTarget = target1;
    }

    // Update is called once per frame
    void Update()
    {
        target1 = GameObject.FindWithTag("EnemyTarget1")?.transform;

        if(target1 != null)
        {
            currentTarget = target1;
        }
        else
        {
            currentTarget = target2;
        }
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
            if (tauntRef != null && damageTauntRoutine == null)
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
