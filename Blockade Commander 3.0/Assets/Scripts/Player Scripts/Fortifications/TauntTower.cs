using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class TauntTower : MonoBehaviour
{
    //Base Stats//
    [SerializeField] private int baseMaxLives = 10;
    [SerializeField] private int baseRange = 5;
    [SerializeField] private int baseDmg = 0;
    public float tauntDetectionRange = 25f;

    //Runtime Stats//
    public int health = 10;
    public int maxLives = 10;
    public int range = 5;
    public int dmg = 0;
    public bool isDed = false;

    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] PlacingScript placingRef;
    [SerializeField] PlayerFortress playerFortRef;
    [SerializeField] private StatPopupUI statPopupRef;

    public GameObject enemyWinPopup;

    private Coroutine damageRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    
        if (placingRef == null)
            placingRef = FindFirstObjectByType<PlacingScript>();

        if (playerFortRef == null)
            playerFortRef = FindFirstObjectByType<PlayerFortress>();
    }
    
    public void OpenStats()
    {
        if (statPopupRef != null)
        {
            statPopupRef.ShowStats(health, range, dmg);
        }
        else
        {
            Debug.LogWarning("StatPopupUI not assigned on Cannon!");
        }

    }

    public void Initialize(StatPopupUI popup)
    {
        statPopupRef = popup;
    }
    

    void Start()
    {
        ResourceUI resourceRef = FindFirstObjectByType<ResourceUI>();

        if(resourceRef != null)
        {
            //handling upgrades for health, attack, and range

            maxLives += resourceRef.tauntHealthUpgrade;
            health = maxLives;

            range += resourceRef.tauntRangeUpgrade;

            dmg += resourceRef.tauntRangeUpgrade;
        }

        healthBar.UpdateHealthBar(health, maxLives);
    }

    //this function handles applying the health upgrade to towers already in the scene
    public void ApplyHealthUpgrade(int amount)
    {
        maxLives += amount;
        health += amount;
        healthBar.UpdateHealthBar(health, maxLives);
    }

    //this function handles applying the attack upgrade to towers already in the scene
    public void ApplyDmgUpgrade(int amount)
    {
        dmg += amount;
    }

    //this function handles applying the range upgrade to towers already in the scene
    public void ApplyRangeUpgrade(int amount)
    {
        range += amount;
    }

    public void takeDamage()
    {
        if (isDed) return;
        health--;
        healthBar.UpdateHealthBar(health, maxLives);
        if (health <= 0)
        {
            isDed = true;
            
            if (gameObject != null)
            {
                placingRef.currentPlaced--;

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BasicEnemy")
        {
            if(damageRoutine == null)
            {
               damageRoutine = StartCoroutine(DamageOverTime());
            }
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BasicEnemy")
        {
            if(damageRoutine != null)
            {
                StopCoroutine(damageRoutine);
            }
            
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            takeDamage();
            //tauntRef.takeDamage();
            yield return new WaitForSeconds(1f);
        }
    }

    //Deals damage based on if hit by a sloop
    public void SloopDamage()
    {
        health = health - 5;
        healthBar.UpdateHealthBar(health, maxLives);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.0f);

        // Taunt detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tauntDetectionRange);
    }

}
