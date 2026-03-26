using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class TauntTower : MonoBehaviour
{
    public int health = 10;
    public int maxLives = 10;
    public int range = 5;
    public int dmg = 0;

    public bool isDed = false;

    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] PlacingScript placingRef;
    [SerializeField] PlayerFortress playerFortRef;

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
    /*
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
    */

    void Start()
    {
        healthBar.UpdateHealthBar(health, maxLives);
    }
    private void FixedUpdate()
    {
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

}
