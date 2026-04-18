using UnityEngine;

public class UpgradeWallRangeButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;

    [SerializeField] private UpgradeManager upgradeManagerRef;
    public UpgradesText upgradesTextRef;

    public int cost; 

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
        int currentLevel = upgradeManagerRef.upgradeLvlWall;

        if (resourceUIRef.gold < upgradeManagerRef.wallPrice)
        {
            Debug.Log("not enough gold to upgrade wall range :( ");
            return;
        }

        //subtract gold
        resourceUIRef.gold -= upgradeManagerRef.wallPrice;
        //update health
        resourceUIRef.wallRangeUpgrade++;
        //update upgrade level
        upgradeManagerRef.upgradeLvlWall++;
        //update upgrade price
        upgradeManagerRef.wallPrice = upgradeManagerRef.wallPrice + 30;


        //display correct text
        resourceUIRef.UpdateResourceUI();
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlWall);

        //display correct price
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.wallPrice);

        Debug.Log($"Upgrade wall range +1 for {upgradeManagerRef.wallPrice} gold (now level {upgradeManagerRef.upgradeLvlWall})");

        //upgrade walls already in the scene
        Wall[] walls = FindObjectsByType<Wall>(FindObjectsSortMode.None);
        foreach (Wall wall in walls)
        {
            wall.ApplyRangeUpgrade(resourceUIRef.wallRangeUpgrade);
        }
    }
}
