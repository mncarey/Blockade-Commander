using System.Collections;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public Transform target;
    public float speed = 5;
    public int lives = 5;
    public int dmg = 0;

    private Coroutine damageRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);   
    }
    private void takeDamage()
    {
        lives--;
        if (lives <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "killZone")
        {
            damageRoutine = StartCoroutine(DamageOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "killZone")
        {
            StopCoroutine(damageRoutine);
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
}
