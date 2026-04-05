using UnityEngine;

public class NextWaveButton : MonoBehaviour
{
    public GameObject EnemyZoneRed;
    public GameObject EnemyZoneBlue;

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
    public ResourceUI resourceRef;
    public int maxEnemies;

    private AscensionManager ascensionManagerRef;


     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fortUIManagerRef = FindObjectOfType<FortUIManager>();
        resourceRef = FindObjectOfType<ResourceUI>();
        waveSpawnerRef = FindObjectOfType<Wave_Spawner_BasicEnemy>();
        ascensionManagerRef = FindObjectOfType<AscensionManager>();
    }

    public void IWasClicked()
    {
        fortUIManagerRef.ResetForNextWave();       
        enemiesDefeatedPopup.SetActive(false);
        enemeisWinPopup.SetActive(false);
        waveSpawnerRef.ClearFortifications();
        waveSpawnerRef.ClearEnemies();
   
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

       placingScriptRef.TogglePlacementLock();
        placingScriptRef.placementEnable = true;

        resourceRef.UpdateFortRef(0);
        //ASSIGN IN INSPECTOR!!!//
        EnemyZoneBlue.SetActive(false);
        EnemyZoneRed.SetActive(true);

        //Spawn Enemies
        //call spawn enemies
        waveSpawnerRef.SpawnEnemy();
        maxEnemies = waveSpawnerRef.enemiesAlive;

        //Check if Ascension is possible
        ascensionManagerRef.ToggleUI();
    }
}
