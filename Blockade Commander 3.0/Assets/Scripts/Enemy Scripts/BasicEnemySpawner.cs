using UnityEngine;
using UnityEngine.InputSystem;

public class BasicEnemySpawner : MonoBehaviour
{
    public GameObject BasicEnemy;
    public int lives = 5;

    void Update()
    {
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Instantiate(BasicEnemy, gameObject.transform.position, Quaternion.identity);
    }

  
}
