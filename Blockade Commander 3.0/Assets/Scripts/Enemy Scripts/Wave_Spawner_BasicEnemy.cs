using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wave_Spawner_BasicEnemy : MonoBehaviour
{

    public GameObject BasicEnemy;
    public Transform[] spawnPoints; //<- for setting specific spawn points
    public StartWaveButton startWaveRef;

    //random points
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private Vector3 randomSpawn;

    public int enemiesAlive = 0;

    private bool canSpawn = false;

    private void Start()
    {
        /*
        Debug.Log("spawning wave");
        SpawnEnemy();
        */
    }

    void Update()
    {
        /*
        if (Keyboard.current == null) return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)// <-- temporary trigger for spawning waves
        {
            Debug.Log("spawning wave");
            SpawnEnemy();
        }
        */
        if (enemiesAlive <= 0)
        {
            //Debug.Log("Wave Complete!");
        }
    }

    public void SpawnEnemy()
    {
        for(int i = 0; i< spawnPoints.Length; i++)
        {
            GameObject enemy = Instantiate(BasicEnemy, spawnPoints[i].position, Quaternion.identity);
            enemiesAlive++;

            enemy.GetComponent<BasicEnemy>().waveSpawnerRef = this;
        }     
    }

    public void EnemyDied()
    {
        /*
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            Debug.Log("Wave Complete!");
        }
        */
    }

    

}
