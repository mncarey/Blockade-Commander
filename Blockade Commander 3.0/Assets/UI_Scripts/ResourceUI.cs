using UnityEngine;
using TMPro;
using System.Collections;

public class ResourceUI : MonoBehaviour
{

    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text fortNumberText;
    [SerializeField] private UpgradesText upgradesTextRef;
    PlacingScript placingRef;

    public int gold = 0;
    public int kills = 0;
    public int fortNumber = 0;
    //Taunt tower upgrades//
    public int tauntHealthUpgrade = 0;
    public int tauntRangeUpgrade = 0;
    public int tauntDmgUpgrade = 0;

    //Wall upgrades//
    public int wallHealthUpgrade = 0;
    public int wallRangeUpgrade = 0;
    public int wallDmgUpgrade = 0;

    //Cannon upgrades//
    public int cannonHealthUpgrade = 0;
    public int cannonRangeUpgrade = 0;
    public int cannonDmgUpgrade = 0;

    //Motar upgrades//
    public int mortarHealthUpgrade = 0;
    public int mortarRangeUpgrade = 0;
    public int mortarDmgUpgrade = 0;

    public static ResourceUI instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placingRef = FindAnyObjectByType<PlacingScript>();
        goldText.text = "Gold: " + gold;
        killsText.text = "Kills: " + kills;
        fortNumberText.text = fortNumber + "/" + placingRef.maxPlaced;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Call these functions when you want to update these resources.

    //CALL THIS WHEN BUYING UPGRADES!!!!//
    public void UpdateResourceUI()
    {
        goldText.text = "Gold: " + gold;
        killsText.text = "Kills: " + kills;

    }
    public void UpdateGold(int newGold)
    {
        
        gold += newGold;
        goldText.text = "Gold: " + gold;
    }
    public void UpdateKills(int newKills)
    {
        kills += newKills;
        killsText.text = "Kills: " + kills;
    }

    public void ResetResources()
    {
        kills = 0;
        gold = 0;
        killsText.text = "Kills: " + kills;
        goldText.text = "Gold: " + gold;
    }

    public void AscendResetResource()
    {
        gold = 0;
        goldText.text = "Gold: " + gold;
        fortNumberText.text = fortNumber + "/" + placingRef.maxPlaced;

    }

    public void UpdateFortRef(int fortNum)
    {
        fortNumberText.text = fortNum + "/4";
    }
}
