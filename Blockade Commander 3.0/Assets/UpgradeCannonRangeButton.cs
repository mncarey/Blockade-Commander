using UnityEngine;

public class UpgradeCannonRangeButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;

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
        if (resourceUIRef.gold >= resourceUIRef.goldCost1)
        {
            resourceUIRef.cannonRangeUpgrade++;
            //subtract gold
            resourceUIRef.gold -= resourceUIRef.goldCost1;
            resourceUIRef.UpdateResourceUI();
            Debug.Log("Upgraded cannon range + 1 for " + resourceUIRef.goldCost1 + " gold");

            //upgrade cannons already in the scene
            Cannon[] cannons = FindObjectsByType<Cannon>(FindObjectsSortMode.None);
            foreach (Cannon cannon in cannons)
            {
                cannon.ApplyRangeUpgrade(resourceUIRef.cannonRangeUpgrade);
            }
        }
        else
        {
            Debug.Log("Not enough gold to upgrade cannon range:(");
        }


    }
}
