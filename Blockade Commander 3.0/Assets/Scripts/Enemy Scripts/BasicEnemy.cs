using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicEnemy : MonoBehaviour
{
    private Transform target1;
    private Transform target2;
    private Transform currentTarget;

    private TauntTower tauntRef;

    public float reachDistance = 5;

    //public Transform target2;
    public float speed = 5;
    public int lives = 5;
    public int dmg = 0;

    private Coroutine damageRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target1 = GameObject.FindWithTag("EnemyTarget1")?.transform;
        target2 = GameObject.FindWithTag("EnemyTarget2")?.transform;

        currentTarget = target1;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTarget == null) return;
        if(target1 == null)
        {
            currentTarget = target2;
        }
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
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
            Debug.Log("Trying to kill guys");
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
            tauntRef.takeDamage();
            yield return new WaitForSeconds(1f);
        }
    }
}
