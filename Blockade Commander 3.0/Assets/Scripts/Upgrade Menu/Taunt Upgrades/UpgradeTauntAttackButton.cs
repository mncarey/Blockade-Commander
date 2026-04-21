using UnityEngine;

public class UpgradeTauntAttackButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;

    [SerializeField] private UpgradeManager upgradeManagerRef;
    public UpgradesText upgradesTextRef;
    public GameObject noMoneyPopup;

    public int cost;
    public int nextCost;

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
            Debug.Log("not enough gold to upgrade taunt dmg :( ");
            noMoneyPopup.gameObject.SetActive(true);

            return;
        }

        //subtract gold
        resourceUIRef.gold -= upgradeManagerRef.tauntPrice;
        //update dmg
        resourceUIRef.tauntDmgUpgrade++;
        //update upgrade level
        upgradeManagerRef.upgradeLvlTaunt++;
        //update upgrade price
        upgradeManagerRef.tauntPrice = upgradeManagerRef.tauntPrice + 30;
        

        //display correct text
        resourceUIRef.UpdateResourceUI();
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlTaunt);

        //display correct price
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.tauntPrice);
        //*good stuff**//

        Debug.Log($"Upgrade taunt dmg +1 for {upgradeManagerRef.tauntPrice} gold (now level {upgradeManagerRef.upgradeLvlTaunt})");

        //upgrade walls already in the scene
        TauntTower[] taunts = FindObjectsByType<TauntTower>(FindObjectsSortMode.None);
        foreach (TauntTower taunt in taunts)
        {
            taunt.ApplyDmgUpgrade(resourceUIRef.tauntDmgUpgrade);
        }
    }
    /*
       private int GetCostForLevel(int level)
       {
           return level switch
           {
               0 => resourceUIRef.goldCost1,
               1 => resourceUIRef.goldCost2,
               2 => resourceUIRef.goldCost3,
               3 => resourceUIRef.goldCost4,
               4 => resourceUIRef.goldCost5,
               _ => int.MaxValue
           };
       }
       */
}
