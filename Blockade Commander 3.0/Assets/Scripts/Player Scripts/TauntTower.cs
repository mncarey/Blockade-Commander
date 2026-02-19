using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TauntTower : MonoBehaviour
{
    public int health = 10;
    public int maxLives = 10;

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
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BasicEnemy")
        {
            Debug.Log("Trying to kill guys");
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
