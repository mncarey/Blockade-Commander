using UnityEngine;
using UnityEngine.InputSystem;

public class BasicEnemySpawner : MonoBehaviour
{

    public GameObject BasicEnemy;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            spawnEnemy();
        }
    }

    private void spawnEnemy()
    {
        Instantiate(BasicEnemy, gameObject.transform.position, Quaternion.identity);
    }
}
