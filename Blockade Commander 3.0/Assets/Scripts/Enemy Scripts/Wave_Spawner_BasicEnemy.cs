using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wave_Spawner_BasicEnemy : MonoBehaviour
{

    public GameObject BasicEnemy;
    public Transform[] spawnPoints; //<- for setting specific spawn points

    //random points
    public Vector2 spawnAreaMin;
    public Vector2 spawnAreaMax;
    private Vector3 randomSpawn;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)// <-- temporary trigger for spawning waves
        {
            Debug.Log("spawning wave");
            SpawnEnemy(4);
        }
    }

    private void SpawnEnemy(int amount)
    {
        for(int i = 0; i<= spawnPoints.Length-1; i++)
        {
            Instantiate(BasicEnemy, spawnPoints[i].position, Quaternion.identity);
        }    
        
    }
}
