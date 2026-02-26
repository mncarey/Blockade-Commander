using UnityEngine;

public class NextRoundButton : MonoBehaviour
{
    public GameObject enemiesDefeatedPopup;
    public Wave_Spawner_BasicEnemy waveSpawnerRef;
    public GameObject fortButtonRef;
    public GameObject fortMenuRef;
    public GameObject exitFortMenuRef;
    public GameObject fortRemoveRef;

    public PlacingScript placingScriptRef;
    public PlayerFortress playerRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = FindObjectOfType<PlayerFortress>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IWasClicked()
    {
        playerRef.health = 10;
        playerRef.isDed = false;
        playerRef.Heal();

        placingScriptRef.currentPlaced = 0;
        waveSpawnerRef.ClearEnemies();
        

        enemiesDefeatedPopup.SetActive(false);

        waveSpawnerRef.ClearFortifications();
        
        

        fortButtonRef.SetActive(true);
        if (fortMenuRef != null)
        {
            fortMenuRef.SetActive(true);
        }
        if (exitFortMenuRef != null)
        {
            exitFortMenuRef.SetActive(true);
        }
        if (fortRemoveRef != null)
        {
            fortRemoveRef.SetActive(true);
        }

        /*
            Vector3 position = new Vector3(139f, 37f, 11f);
            playerRef.gameObject.SetActive(true);
            //spawn player
            Debug.Log("DECLAN");
            Instantiate(playerRef.gameObject, position, Quaternion.identity);
        */

    }
}
