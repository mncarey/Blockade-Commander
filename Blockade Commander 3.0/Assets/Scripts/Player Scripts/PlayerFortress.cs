using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerFortress : MonoBehaviour
{
    public GameObject startWaveRef;
    public GameObject nextWaveRef;
    public GameObject enemiesWinPopupRef;

    public int health = 10;
    public int maxLives = 10;
    public bool isDed = false;

    public ResourceUI resourceRef;
    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;
    [SerializeField] PlacingScript placingRef;

    private Coroutine damageRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        resourceRef = FindObjectOfType<ResourceUI>();
    }

    void Start()
    {
        healthBar.UpdateHealthBar(health, maxLives);
    }
    private void Update()
    {
        
    }

    public void updateHealthBar()
    {
        healthBar.UpdateHealthBar(health, maxLives);
    }
    public void takeDamage()
    {
        if (isDed) return;

        health--;
        updateHealthBar();
        if (health <= 0)
        {
            health = 0;
            isDed = true;

            if (gameObject != null)
            {
                
                //show lose screen
                startWaveRef.SetActive(true);
                nextWaveRef.SetActive(true);
                
                resourceRef.ResetResources();
                
            }
            Debug.Log("player fort died");
           

            if(placingRef.currentPlaced == 0)
            {
                enemiesWinPopupRef.SetActive(true);
            }
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BasicEnemy")
        {
            damageRoutine = StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "BasicEnemy")
        {
            StopCoroutine(damageRoutine);
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
}
