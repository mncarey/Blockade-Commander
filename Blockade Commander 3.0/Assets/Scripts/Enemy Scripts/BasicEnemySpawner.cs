using UnityEngine;
using UnityEngine.InputSystem;

public class BasicEnemySpawner : MonoBehaviour
{

    public GameObject BasicEnemy;

    void Update()
    {
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Instantiate(BasicEnemy, gameObject.transform.position, Quaternion.identity);
    }
}
