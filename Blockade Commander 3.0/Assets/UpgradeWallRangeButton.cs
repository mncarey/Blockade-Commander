using UnityEngine;

public class UpgradeWallRangeButton : MonoBehaviour
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
            resourceUIRef.wallRangeUpgrade++;
            //subtract gold
            resourceUIRef.gold -= resourceUIRef.goldCost1;
            Debug.Log("Upgraded wall range + 1 for " + resourceUIRef.goldCost1 + " gold");

            //upgrade walls already in the scene
            Wall[] walls = FindObjectsByType<Wall>(FindObjectsSortMode.None);
            foreach (Wall wall in walls)
            {
                wall.ApplyRangeUpgrade(resourceUIRef.wallRangeUpgrade);
            }
        }
        else
        {
            Debug.Log("Not enough gold to upgrade wall range:(");
        }


    }
}
