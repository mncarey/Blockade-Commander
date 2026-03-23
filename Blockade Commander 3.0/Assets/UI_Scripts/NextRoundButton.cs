using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    public GameObject enemiesDefeatedPopup;
    public GameObject enemeisWinPopup;
    public Wave_Spawner_BasicEnemy waveSpawnerRef;
    public PlacingScript placingScriptRef;
    public StartWaveButton startWaveButtonRef;

    public GameObject fortButtonRef;
    public GameObject fortMenuRef;
    public GameObject exitFortMenuRef;
    public GameObject fortRemoveRef;
    public GameObject waveProgressbar;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IWasClicked()
    {
        Debug.Log("ButtonClicked");
        enemiesDefeatedPopup.SetActive(false);
        enemeisWinPopup.SetActive(false);

        waveSpawnerRef.ClearFortifications();
        waveSpawnerRef.ClearEnemies();

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

        //reset fortifications to place
        placingScriptRef.currentPlaced = 0;

        //reset this variable to make sure start wave appears on next round
        startWaveButtonRef.isClicked = false;

        //deactivate this game object
        waveProgressbar.SetActive(false);
    }
}
