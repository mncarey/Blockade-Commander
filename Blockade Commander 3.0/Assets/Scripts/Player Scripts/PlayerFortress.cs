using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class PlayerFortress : MonoBehaviour
{
    public int health = 10;
    public int maxLives = 10;
    public bool isDed = false;

    public GameObject enemiesWinPopupRef;

    Rigidbody rb;

    [SerializeField] FloatingHealthBar healthBar;

    private Coroutine damageRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    void Start()
    {
        healthBar.UpdateHealthBar(health, maxLives);
        //enemiesWinPopupRef = GameObject.FindGameObjectWithTag("EnemiesWin");

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
            StopCoroutine(damageRoutine);
            Debug.Log("DELCAN HELP");
            enemiesWinPopupRef.SetActive(true);
            //Destroy(gameObject);

            if (gameObject != null)
            {
                
                
            }
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

    public void Heal()
    {
        healthBar.UpdateHealthBar(health, maxLives);
        damageRoutine = null;
    }

    private IEnumerator DamageOverTime()
    {
        while (health>0)
        {
            takeDamage();
            //tauntRef.takeDamage();
            yield return new WaitForSeconds(1f);
        }

        damageRoutine = null;
        //reset player health
    }
}
