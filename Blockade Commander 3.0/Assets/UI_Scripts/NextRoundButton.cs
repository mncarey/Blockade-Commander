using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    public GameObject enemiesDefeatedPopup;
    public GameObject enemeisWinPopup;
    public Wave_Spawner_BasicEnemy waveSpawnerRef;
    public PlacingScript placingScriptRef;
    public StartWaveButton startWaveButtonRef;
    [SerializeField] public GameObject fortCanvasRef;
    public GameObject waveProgressbar;
    public GameObject fortButtonRef;
     public GameObject fortMenuRef;
     public GameObject exitFortMenuRef;
     public GameObject fortRemoveRef;
     public PlayerFortress playerFortRef;
    public FortUIManager fortUIManagerRef;


     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fortUIManagerRef = FindObjectOfType<FortUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IWasClicked()
    {
        fortUIManagerRef.ResetForNextWave();
        Debug.Log("ButtonClicked");
        enemiesDefeatedPopup.SetActive(false);
        enemeisWinPopup.SetActive(false);

        waveSpawnerRef.ClearFortifications();
        waveSpawnerRef.ClearEnemies();


        //fortCanvasRef.SetActive(true);
        
        
        
        //reset fortifications to place
        placingScriptRef.currentPlaced = 0;

        //reset this variable to make sure start wave appears on next round
        startWaveButtonRef.isClicked = false;

        //deactivate this game object
        waveProgressbar.SetActive(false);

        //reset player health
        playerFortRef.health = playerFortRef.maxLives;

        //reset player health bar
        playerFortRef.updateHealthBar();
    }
}
