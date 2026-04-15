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

    public GameObject FastWaveRef;
    public GameObject FastSmallRef;
    public GameObject FastLargeRef;
    public GameObject TankWaveRef;
    public GameObject TankSmallRef;
    public GameObject TankLargeRef;
    public GameObject HybridWaveRef;
    public GameObject HybridSmallRef;
    public GameObject HybridLargeRef;
    private GameObject[] enemyPrefabs;
    private GameObject[] waves;

    //creates integer array that will point to the wave array numbers for the specific type needed
    private readonly int[] smallWaveIndices = { 1, 4, 7 };
    private readonly int[] mediumWaveIndices = { 0, 3, 6 };
    private readonly int[] largeWaveIndices = { 2, 5, 8 };

    //random points
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private Vector3 randomSpawn;

    public int enemiesAlive = 0;
    public int enemiesTotalThisWave;

    public int currentWaveNumber = 1;

    private bool canSpawn = false;

    private void Start()
    {
        //assign all the wave prefabs to this array
        waves = new GameObject[9];
        waves[0] = FastWaveRef;
        waves[1] = FastSmallRef;
        waves[2] = FastLargeRef;
        waves[3] = TankWaveRef;
        waves[4] = TankSmallRef;
        waves[5] = TankLargeRef;
        waves[6] = HybridWaveRef;
        waves[7] = HybridSmallRef;
        waves[8] = HybridLargeRef;

        
        SpawnEnemy();
        
    }

    void Update()
    {
       
    }
    //Set a function for the specific wave numbers
    //Ensure that
    public void SpawnEnemy()
    {
        enemiesAlive = 0;

        //get the randomized wave type
        GameObject selectedPrefab = GetWavePrefab();
        
        GameObject wave = Instantiate(selectedPrefab, spawnPoints[1].position, Quaternion.identity);
        //finds enemies within the wave ref and assigns them to the enemies array
        BasicEnemy[] enemies = wave.GetComponentsInChildren<BasicEnemy>();
        //sets the current enemeis alive and total enemies to however many there are
        enemiesAlive = enemies.Length;
        enemiesTotalThisWave = enemies.Length;

        foreach(BasicEnemy enemy in enemies)
        {
            enemy.waveSpawnerRef = this;
        }
        //increase the wave number after it has spawned
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

    private GameObject GetWavePrefab()
    {
        //Integer array of the pool of selected prefabs
        int[] pool;
        //if within a certain range, set the integer pool to the specified wave indices, then return a random index of that wave between a random value within that indices
        if(currentWaveNumber >= 1 && currentWaveNumber <= 10)
        {
            pool = smallWaveIndices;
        }
        else if(currentWaveNumber >= 11 && currentWaveNumber <= 20)
        {
            pool = mediumWaveIndices;
        }
        else if(currentWaveNumber >= 21 && currentWaveNumber <= 50)
        {
            pool = largeWaveIndices;
        }
        else
        {
            Debug.LogWarning($"Wave {currentWaveNumber} is out of defined range. Defaulting to small.");
            pool = smallWaveIndices;
        }

        int randomIndex = pool[Random.Range(0, pool.Length)];
        
        return waves[randomIndex];
    }
}
