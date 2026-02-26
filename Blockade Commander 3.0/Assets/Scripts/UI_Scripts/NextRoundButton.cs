using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    public GameObject enemiesDefeatedPopup;
    public Wave_Spawner_BasicEnemy waveSpawnerRef;
    public GameObject fortButtonRef;
    public GameObject fortMenuRef;
    public GameObject exitFortMenuRef;
    public GameObject fortRemoveRef;

    public PlacingScript placingScriptRef;

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

        //reset fortifications to place
        placingScriptRef.currentPlaced = 0;
    }
}
