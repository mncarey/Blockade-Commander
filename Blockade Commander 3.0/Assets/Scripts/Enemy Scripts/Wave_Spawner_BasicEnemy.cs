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
    public GameObject TankWaveRef;
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
        
        SpawnEnemy();
        
    }

    void Update()
    {
       
    }

    public void SpawnEnemy()
    {
        enemiesAlive = 0;

        
        GameObject wave = Instantiate(FastWaveRef, spawnPoints[1].position, Quaternion.identity);
        //finds enemies within the wave/fastwave ref and assigns them to the enemies array
        BasicEnemy[] enemies = wave.GetComponentsInChildren<BasicEnemy>();
        //sets the current enemeis alive and total enemies to however many there are
        enemiesAlive = enemies.Length;
        enemiesTotalThisWave = enemies.Length;

        foreach(BasicEnemy enemy in enemies)
        {
            enemy.waveSpawnerRef = this;
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
