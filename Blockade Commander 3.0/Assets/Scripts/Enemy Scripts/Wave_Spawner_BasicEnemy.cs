using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wave_Spawner_BasicEnemy : MonoBehaviour
{

    public GameObject BasicEnemy;
    public Transform[] spawnPoints; //<- for setting specific spawn points
    public StartWaveButton startWaveRef;
    public GameObject enemyDefeatPopup;
    public GameObject enemyWinPopup;

    public BasicEnemy enemyRef;


    public GameObject sloopRef;
    public GameObject brigRef;
    public GameObject gallRef;
    public GameObject rangeGallRef;
    private GameObject[] enemyPrefabs;

    //random points
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private Vector3 randomSpawn;

    public int enemiesAlive = 0;
    public int enemiesTotalThisWave;

    private bool canSpawn = false;

    private void Start()
    {
        enemyPrefabs = new GameObject[4];
        enemyPrefabs[0] = sloopRef;
        enemyPrefabs[1] = brigRef;
        enemyPrefabs[2] = gallRef;
        enemyPrefabs[3] = rangeGallRef;
        SpawnEnemy();
        
    }

    void Update()
    {
       
    }

    public void SpawnEnemy()
    {
        enemiesAlive = 0;
        

        for(int i = 0; i< enemiesTotalThisWave; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject randomEnemy = enemyPrefabs[randomIndex];
            GameObject enemy = Instantiate(randomEnemy, spawnPoints[i].position, Quaternion.identity);
            enemiesAlive++;

            enemy.GetComponent<BasicEnemy>().waveSpawnerRef = this;
            
        }     
    }

    public void EnemyDied()
    {
        
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            
            enemyDefeatPopup.gameObject.SetActive(true);
        }
        
    }

    public void PlayerDied()
    {
        
    }

    public void ClearFortifications()
    {
        GameObject[] forts = GameObject.FindGameObjectsWithTag("Fortification");

        foreach (GameObject fort in forts)
        {
            Destroy(fort);
        }

        
    }

    public void ClearEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        enemiesAlive = 0;
        Debug.Log("All enemies destroyed.");
    }
}
