using UnityEngine;

public class UpgradeTauntRangeButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;

    [SerializeField] private UpgradeManager upgradeManagerRef;
    public UpgradesText upgradesTextRef;
    public GameObject noMoneyPopup;
    public GameObject noLvlsPopup;

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

        if (currentLevel >= upgradeManagerRef.maxUpgradeLvl)
        {
            Debug.Log("not high enough level to upgrade");
            noLvlsPopup.gameObject.SetActive(true);
            return;

        }

        if (resourceUIRef.gold < upgradeManagerRef.tauntPrice)
        {
            Debug.Log("not enough gold to upgrade taunt range :( ");
            noMoneyPopup.gameObject.SetActive(true);

            return;
        }

        //subtract gold
        resourceUIRef.gold -= upgradeManagerRef.tauntPrice;
        //update health
        resourceUIRef.tauntRangeUpgrade++;
        //update upgrade level
        upgradeManagerRef.upgradeLvlTaunt++;
        //update upgrade price
        upgradeManagerRef.tauntPrice = upgradeManagerRef.tauntPrice + 30;
        

        //display correct text
        resourceUIRef.UpdateResourceUI();
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlTaunt);

        //display correct price
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.tauntPrice);

        Debug.Log($"Upgrade taunt range +1 for {upgradeManagerRef.tauntPrice} gold (now level {upgradeManagerRef.upgradeLvlTaunt})");

        //upgrade walls already in the scene
        TauntTower[] taunts = FindObjectsByType<TauntTower>(FindObjectsSortMode.None);
        foreach (TauntTower taunt in taunts)
        {
            taunt.ApplyRangeUpgrade(resourceUIRef.tauntRangeUpgrade);
        }
    }
}
