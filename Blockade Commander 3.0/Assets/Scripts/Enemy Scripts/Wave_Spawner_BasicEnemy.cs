using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum WaveType { fastSmall, fastMedium, fastLarge, tankSmall, tankMedium, tankLarge, hybridSmall, hybridMedium, hybridLarge }
public class Wave_Spawner_BasicEnemy : MonoBehaviour
{

    public Transform[] spawnPoints; //<- for setting specific spawn points
    public StartWaveButton startWaveRef;
    public GameObject enemyDefeatPopup;
    public GameObject enemyWinPopup;


    public GameObject[] meleePrefabs;
    public GameObject[] rangedPrefabs;
    public GameObject[] tankPrefabs;
    public GameObject[] fastPrefabs;


    public int enemiesAlive = 0;
    public int enemiesTotalThisWave;

    public int currentWaveNumber = 1;


    private void Start()
    {
        
        
        SpawnEnemy();
        
    }
    //Set a function for the specific wave numbers
    //Ensure that
    public void SpawnEnemy()
    {
        enemiesAlive = 0;

        //gets the wavetype generated
        WaveType waveType = GetWaveType();

        //gets the wavetype, goes through the switch until it is the correct one
        switch (waveType)
        {
            case WaveType.fastSmall:
                SpawnFromPool(fastPrefabs, 4); Debug.Log("Current Wave Set: Fast/Small");  break;
            case WaveType.fastMedium:
                SpawnFromPool(fastPrefabs, 6); Debug.Log("Current Wave Set: Fast/Medium"); break;
            case WaveType.fastLarge:
                SpawnFromPool(fastPrefabs, 8); Debug.Log("Current Wave Set: Fast/Large"); break;

            case WaveType.tankSmall:
                SpawnFromPool(tankPrefabs, 4); Debug.Log("Current Wave Set: Tank/Small"); break;
            case WaveType.tankMedium:
                SpawnFromPool(tankPrefabs, 6); Debug.Log("Current Wave Set: Tank/Medium"); break;
            case WaveType.tankLarge:
                SpawnFromPool(tankPrefabs, 8); Debug.Log("Current Wave Set: Tank/Large"); break;
                
            case WaveType.hybridSmall:
                SpawnHybrid(4); Debug.Log("Current Wave Set: Hybrid/Small"); break;  
            case WaveType.hybridMedium:
                SpawnHybrid(6); Debug.Log("Current Wave Set: Hybrid/Medium"); break;
            case WaveType.hybridLarge:
                SpawnHybrid(8); Debug.Log("Current Wave Set: Hybrid/Large"); break;
                
        }
        currentWaveNumber++;

    }

    public void EnemyDied()
    {
        
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            
            enemyDefeatPopup.gameObject.SetActive(true);
        }
        
    }
    /*
    public void PlayerDied()
    {
        
    }
    */
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

    //Spawns hybrid enemies if called, input is the enemies for that wave, decided upon by GetWaveType
    private void SpawnFromPool(GameObject[] pool, int toSpawn)
    {
        //Create a list that stores the spawned enemies
        List<BasicEnemy> spawnedEnemies = new List<BasicEnemy>();
        //spawn using the minimum number between these two inputs
        int spawnCount = Mathf.Min(toSpawn, spawnPoints.Length);
        //spawn while the number we want spawned isn't reached yet
        for(int i = 0; i < spawnCount; i++) 

        {

            GameObject prefab = pool[Random.Range(0, pool.Length)];
            //spawn the enemy
            GameObject spawned = Instantiate(prefab, spawnPoints[i].position, Quaternion.identity);
            //assigns enemyComponent
            BasicEnemy enemyComponent = spawned.GetComponent<BasicEnemy>();

            if (enemyComponent != null)
            {
                enemyComponent.waveSpawnerRef = this;
                spawnedEnemies.Add(enemyComponent);
            }
            
        }

        enemiesAlive = spawnedEnemies.Count;
        enemiesTotalThisWave = spawnedEnemies.Count;
        
    }

    private void SpawnHybrid(int toSpawn)
    {
        //Create a list that stores the spawned enemies
        List<BasicEnemy> spawnedEnemies = new List<BasicEnemy>();
        int spawnCount = Mathf.Min(toSpawn, spawnPoints.Length);

        for (int i = 0; i < spawnCount; i++)
        {
            //enemyToSpawn declared
            GameObject enemyToSpawn;
            // first half of spawnpoints are always melee
            if (i < toSpawn / 2)
            {
                //randomly pick from melee prefabs
                enemyToSpawn = meleePrefabs[Random.Range(0, meleePrefabs.Length)];
            }
            //second half of spawnPoints are always ranged
            else
            {
                //randomly pick from ranged prefabs
                enemyToSpawn = rangedPrefabs[Random.Range(0, rangedPrefabs.Length)];
            }
            //spawn the enemy based on the selected prefab
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnPoints[i].position, Quaternion.identity);
            BasicEnemy enemyComponent = spawnedEnemy.GetComponent<BasicEnemy>();

            //assigns references then adds the enemyComponent to the list of enemies so it can keep track of them
            if (enemyComponent != null)
            {
                enemyComponent.waveSpawnerRef = this;
                spawnedEnemies.Add(enemyComponent);
            }
        }

        enemiesAlive = spawnedEnemies.Count;
        enemiesTotalThisWave = spawnedEnemies.Count;

    }

    private WaveType GetWaveType()
    {
        //if within a certain range, set the integer pool to the specified wave indices, then return a random index of that wave between a random value within that indices
        if(currentWaveNumber >= 1 && currentWaveNumber <= 10)
        {
            //small waves only
            WaveType[] smallPool = { WaveType.fastSmall, WaveType.tankSmall, WaveType.hybridSmall };
            return smallPool[Random.Range(0, smallPool.Length)];
        }
        else if(currentWaveNumber >= 11 && currentWaveNumber <= 20)
        {
            WaveType[] mediumPool = { WaveType.fastMedium, WaveType.tankMedium, WaveType.hybridMedium };
            return mediumPool[Random.Range(0, mediumPool.Length)];
        }
        else if(currentWaveNumber >= 21 && currentWaveNumber <= 50)
        {
            WaveType[] largePool = { WaveType.fastLarge, WaveType.tankLarge, WaveType.hybridLarge };
            return largePool[Random.Range(0, largePool.Length)];
        }
        else
        {
            
            
        }
 
        return WaveType.fastSmall;
    }

   
}
