using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wave_Spawner_BasicEnemy : MonoBehaviour
{

    public GameObject BasicEnemy;
    public GameObject Brigantine;
    public GameObject Galleon;
    public GameObject Sloop;
    private GameObject[] enemyTypes;
    public Transform[] spawnPoints; //<- for setting specific spawn points

    //random points
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private Vector3 randomSpawn;

    private void Start()
    {
        enemyTypes = new GameObject[] {Brigantine, Galleon, Sloop};
        Debug.Log("spawning wave");
        SpawnEnemy();
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
    }

    private void SpawnEnemy()
    {
        if (enemyTypes.Length == 0) return;

       
        for(int i = 0; i< spawnPoints.Length; i++)
        {
            int randomIndex = Random.Range(0, enemyTypes.Length);

            GameObject selectedShip = enemyTypes[randomIndex];
            Instantiate(selectedShip, spawnPoints[i].position, Quaternion.identity);
        
        }    
        
    }
}
