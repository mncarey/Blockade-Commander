using UnityEngine;

public class UpgradeWallHealthButton : MonoBehaviour
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
            resourceUIRef.wallHealthUpgrade++;
            //subtract gold
            resourceUIRef.gold -= resourceUIRef.goldCost1;
            resourceUIRef.UpdateResourceUI();
            Debug.Log("Upgraded wall health + 1 for " + resourceUIRef.goldCost1 + " gold");

            //upgrade walls already in the scene
            Wall[] walls = FindObjectsByType<Wall>(FindObjectsSortMode.None);
            foreach (Wall wall in walls)
            {
                wall.ApplyHealthUpgrade(resourceUIRef.wallHealthUpgrade);
            }
        }
        else
        {
            Debug.Log("Not enough gold to upgrade wall health:(");
        }


    }
}
