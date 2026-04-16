using UnityEngine;

public class UpgradeWallHealthButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;

    [SerializeField] private UpgradeManager upgradeManagerRef;
    public UpgradesText upgradesTextRef;

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

        int cost = GetCostForLevel(currentLevel);

        if (resourceUIRef.gold < cost)
        {
            Debug.Log("not enough gold to upgrade wall health :( ");
            return;
        }

        //subtract gold
        resourceUIRef.gold -= cost;
        //update health
        resourceUIRef.wallHealthUpgrade++;
        //update upgrade level
        upgradeManagerRef.upgradeLvlCannon++;

        //display correct text
        resourceUIRef.UpdateResourceUI();
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlCannon);

        Debug.Log($"Upgrade wall health +1 for {cost} gold (now level {upgradeManagerRef.upgradeLvlCannon})");

        //upgrade cannons already in the scene
        Cannon[] cannons = FindObjectsByType<Cannon>(FindObjectsSortMode.None);
        foreach (Cannon cannon in cannons)
        {
            cannon.ApplyHealthUpgrade(resourceUIRef.wallHealthUpgrade);
        }
    }

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
}
