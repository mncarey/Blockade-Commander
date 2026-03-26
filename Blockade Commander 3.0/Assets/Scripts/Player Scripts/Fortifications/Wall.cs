using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] PlacingScript placingRef;
    [SerializeField] PlayerFortress playerFortRef;
    [SerializeField] private StatPopupUI statPopupRef;

    public int health = 10;
    public int maxLives = 10;
    public int range = 0;
    public int dmg = 0;

    private Coroutine damageRoutine;

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

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();

        if (placingRef == null)
            placingRef = FindFirstObjectByType<PlacingScript>();

        if (playerFortRef == null)
            playerFortRef = FindFirstObjectByType<PlayerFortress>();
    }

    private void Start()
    {
        healthBar.UpdateHealthBar(health, maxLives);
    }
    private void Update()
    {

    }

    public void takeDamage()
    {
        health--;
        healthBar.UpdateHealthBar(health, maxLives);
        if (health <= 0)
        {
            placingRef.currentPlaced--;
            if (placingRef.currentPlaced == 0 && playerFortRef.isDed)
            {
                placingRef.enemiesWinPopupRef.gameObject.SetActive(true);
            }

            Destroy(gameObject);
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("BasicEnemy"))
        {
            //found enemy
            Debug.Log("Enemy collided");
           
            damageRoutine = StartCoroutine(DamageOverTime());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (gameObject.CompareTag("BasicEnemy"))
        {
            if (damageRoutine != null)
            {
                StopCoroutine(damageRoutine);   
                damageRoutine = null;
            }
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            takeDamage();
            yield return new WaitForSeconds(1f);
        }
    }
    */
}
