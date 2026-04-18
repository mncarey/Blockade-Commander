using UnityEngine;

public class UpgradeCannonRangeButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;

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
        int currentLevel = upgradeManagerRef.upgradeLvlCannon;

        if (resourceUIRef.gold < upgradeManagerRef.tauntPrice)
        {
            Debug.Log("not enough gold to upgrade cannon range :( ");
            noMoneyPopup.gameObject.SetActive(true);
            return;
        }

        //subtract gold
        resourceUIRef.gold -= upgradeManagerRef.tauntPrice;
        //update health
        resourceUIRef.cannonRangeUpgrade++;
        //update upgrade level
        upgradeManagerRef.upgradeLvlCannon++;
        //update upgrade price
        upgradeManagerRef.cannonPrice = upgradeManagerRef.cannonPrice + 30;


        //display correct text
        resourceUIRef.UpdateResourceUI();
        //display upgrade level
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlCannon);

        //display correct price
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.cannonPrice);

        Debug.Log($"Upgrade cannon health +1 for {upgradeManagerRef.tauntPrice} gold (now level {upgradeManagerRef.upgradeLvlCannon})");

        //upgrade cannons already in the scene
        Cannon[] cannons = FindObjectsByType<Cannon>(FindObjectsSortMode.None);
        foreach (Cannon cannon in cannons)
        {
            cannon.ApplyRangeUpgrade(resourceUIRef.cannonRangeUpgrade);
        }
    }
}
