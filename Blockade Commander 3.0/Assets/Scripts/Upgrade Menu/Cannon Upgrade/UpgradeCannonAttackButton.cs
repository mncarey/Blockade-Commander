using UnityEngine;

public class UpgradeCannonAttackButton : MonoBehaviour
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
            resourceUIRef.cannonDmgUpgrade++;
            //subtract gold
            resourceUIRef.gold -= resourceUIRef.goldCost1;
            resourceUIRef.UpdateResourceUI();
            Debug.Log("Upgraded cannon Attack + 1 for " + resourceUIRef.goldCost1 + " gold");

            //upgrade cannons already in the scene
            Cannon[] cannons = FindObjectsByType<Cannon>(FindObjectsSortMode.None);
            foreach (Cannon cannon in cannons)
            {
                cannon.ApplyDmgUpgrade(resourceUIRef.cannonDmgUpgrade);
            }
        }
        else
        {
            Debug.Log("Not enough gold to upgrade cannon attack:(");
        }


    }
}
