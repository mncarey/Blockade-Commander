using UnityEngine;

public class UpgradeTauntHealthButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;
   // public GameObject tauntTowerStatsPopup;

    [SerializeField] private UpgradeManager upgradeManagerRef;
    public UpgradesText upgradesTextRef;
    public GameObject noMoneyPopup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        resourceUIRef = FindFirstObjectByType<ResourceUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IWasClicked()
    {
        int currentLevel = upgradeManagerRef.upgradeLvlTaunt;

        if (resourceUIRef.gold < upgradeManagerRef.tauntPrice)
        {
            Debug.Log("not enough gold to upgrade taunt health :( ");
            noMoneyPopup.gameObject.SetActive(true);
            return;
        }

        //subtract gold
        resourceUIRef.gold -= upgradeManagerRef.tauntPrice;
        //update health
        resourceUIRef.tauntHealthUpgrade++;
        //update upgrade level
        upgradeManagerRef.upgradeLvlTaunt++;
        //update upgrade price
        upgradeManagerRef.tauntPrice = upgradeManagerRef.tauntPrice + 30;
        

        //display correct text
        resourceUIRef.UpdateResourceUI();
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlTaunt);

        //display correct price
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.tauntPrice);

        Debug.Log($"Upgrade taunt health +1 for {upgradeManagerRef.tauntPrice} gold (now level {upgradeManagerRef.upgradeLvlTaunt})");

        //upgrade walls already in the scene
        TauntTower[] taunts = FindObjectsByType<TauntTower>(FindObjectsSortMode.None);
        foreach (TauntTower taunt in taunts)
        {
            taunt.ApplyHealthUpgrade(resourceUIRef.tauntHealthUpgrade);
        }
    }
    
}
