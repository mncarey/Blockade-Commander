using System.Collections;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] FloatingHealthBar healthBar;

    public int lives = 10;
    public int maxLives = 10;

    private Coroutine damageRoutine;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start()
    {
        healthBar.UpdateHealthBar(lives, maxLives);
    }
    private void Update()
    {

    }

    public void takeDamage()
    {
        lives--;
        healthBar.UpdateHealthBar(lives, maxLives);
        if (lives <= 0)
        {
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
