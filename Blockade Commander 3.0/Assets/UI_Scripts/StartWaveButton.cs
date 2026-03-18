using JetBrains.Annotations;
using UnityEngine;

public class StartWaveButton : MonoBehaviour
{
    public GameObject fortButtonRef;
    public GameObject fortMenuRef;
    public GameObject exitFortMenuRef;
    public GameObject fortRemoveRef;
    public GameObject progressBarRef;
    public Wave_Spawner_BasicEnemy spawnEnemyRef;
    public PlacingScript placingScriptRef;
    public IncreaseDifficulty increaseDiff;
    
    public GameObject player;

    public int maxEnemies;

    public bool isClicked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // placingScriptRef = FindObjectOfType<PlacingScript>();

        increaseDiff = FindObjectOfType<IncreaseDifficulty>();
    }

    // Update is called once per frame
    void Update()
    {
        if (placingScriptRef.currentPlaced > 0)
        {
            gameObject.SetActive(true);
        }
        /*
        if(player == null)
        {
            player.SetActive(true);
        }
        */
    }

    public void IWasClicked()
    {
        fortButtonRef.SetActive(false);
        if(fortMenuRef != null)
        {
            fortMenuRef.SetActive(false);
        }
        if (exitFortMenuRef != null)
        {
            exitFortMenuRef.SetActive(false);
        }
        if (fortRemoveRef != null)
        {
            fortRemoveRef.SetActive(false);
        }
        if(progressBarRef != null)
        {
            progressBarRef.SetActive(true);
        }

        //spawn the enemies!!!
        Debug.Log("spawning enemies");
        spawnEnemyRef.SpawnEnemy();
        maxEnemies = spawnEnemyRef.enemiesAlive;
        // Increase difficulty
        increaseDiff.IncreaseDiff();


        isClicked = true;
    }
}
