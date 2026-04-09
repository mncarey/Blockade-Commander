using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using System.Linq;
using System.Collections.Generic;

public class Cannon : MonoBehaviour
{
    //change these values
    public int health = 4;
    public int maxLives = 10;
    public int range = 15;
    public int dmg = 2;
    public float tickRate = 2f;
    private Coroutine damageRoutine;
    public LayerMask enemyLayer;

    public bool isDed = false;

    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] PlacingScript placingRef;
    [SerializeField] PlayerFortress playerFortRef;
    [SerializeField] private StatPopupUI statPopupRef;

    private Transform currentTarget;

    public GameObject enemyWinPopup;

    public List<GameObject> targets = new List<GameObject>();

    public void OpenStats()
    {
        if(statPopupRef != null)
        {
            statPopupRef.ShowStats(health, range, dmg);
        }
        else
        {
            Debug.LogWarning("StatPopupUI not assigned on Cannon");
        }

    }

    public void Initialize(StatPopupUI popup)
    {
        statPopupRef = popup;
    }

    private void Awake()
    {
        //setting variables in runtime
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();

        if (placingRef == null)
            placingRef = FindFirstObjectByType<PlacingScript>();

        if (playerFortRef == null)
            playerFortRef = FindFirstObjectByType<PlayerFortress>();

    }


    void Start()
    {
        ResourceUI resourceRef = FindFirstObjectByType<ResourceUI>();

        if (resourceRef != null)
        {
            //handling upgrades for health, attack, and range

            maxLives += resourceRef.cannonHealthUpgrade;
            health = maxLives;

            range += resourceRef.cannonRangeUpgrade;

            dmg += resourceRef.cannonDmgUpgrade;
        }

        healthBar.UpdateHealthBar(health, maxLives);
    }

    //this function handles applying the health upgrade to cannons already in the scene
    public void ApplyHealthUpgrade(int amount)
    {
        maxLives += amount;
        health += amount;
        healthBar.UpdateHealthBar(health, maxLives);
    }

    //this function handles applying the attack upgrade to cannons already in the scene
    public void ApplyDmgUpgrade(int amount)
    {
        dmg += amount;
    }

    //this function handles applying the range upgrade to cannons already in the scene
    public void ApplyRangeUpgrade(int amount)
    {
        range += amount;
    }

    private void FixedUpdate()
    {
        //Check if current target is still valid
        if (currentTarget != null)
        {
            //find the distance
            float sqrDist = (currentTarget.position - transform.position).sqrMagnitude;

            //Target is out of range or destroyed
            if (sqrDist > range * range || currentTarget == null)
            {
                currentTarget = null;
            }
        }

        //Only find a new target if we don’t have one
        if (currentTarget == null)
        {
            FindClosestTarget();
        }

        //Start/stop attacking
        if (currentTarget != null && damageRoutine == null)
        {
            damageRoutine = StartCoroutine(DamageOverTime());
        }
        else if (currentTarget == null && damageRoutine != null)
        {
            StopCoroutine(damageRoutine);
            damageRoutine = null;
        }

    }

    private void FindClosestTarget()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);

        float closestDist = Mathf.Infinity;
        Transform closest = null;

        foreach (var col in enemies)
        {
            float dist = (col.transform.position - transform.position).sqrMagnitude;

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = col.transform;
            }
        }

        currentTarget = closest;
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
                if (placingRef.currentPlaced == 0 && playerFortRef.isDed)
                {
                    placingRef.enemiesWinPopupRef.gameObject.SetActive(true);
                }
                Destroy(gameObject);
            }
        }
    }



    private IEnumerator DamageOverTime()
    {
        while (currentTarget != null)
        {
            float sqrDist = (currentTarget.position - transform.position).sqrMagnitude;

            // Stop if out of range
            if (sqrDist > range * range)
            {
                currentTarget = null;
                break;
            }
            BasicEnemy enemy = currentTarget.GetComponent<BasicEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(dmg);
            }
            else
            {
                currentTarget = null; // safety if object is destroyed
            }

            yield return new WaitForSeconds(tickRate);
        }

        damageRoutine = null;
    }

    //Deals damage based on if hit by a sloop
    public void SloopDamage()
    {
        health = health - 5;
        healthBar.UpdateHealthBar(health, maxLives);
    }

    
}
