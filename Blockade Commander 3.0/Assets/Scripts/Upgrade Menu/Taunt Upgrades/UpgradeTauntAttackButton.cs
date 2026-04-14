using UnityEngine;

public class UpgradeTauntAttackButton : MonoBehaviour
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
            resourceUIRef.tauntDmgUpgrade++;
            //subtract gold
            resourceUIRef.gold -= resourceUIRef.goldCost1;
            resourceUIRef.UpdateResourceUI();
            Debug.Log("Upgraded Taunt Attack + 1 for " + resourceUIRef.goldCost1 + " gold");

            //upgrade towers already in the scene
            TauntTower[] towers = FindObjectsByType<TauntTower>(FindObjectsSortMode.None);
            foreach (TauntTower tower in towers)
            {
                tower.ApplyDmgUpgrade(resourceUIRef.tauntDmgUpgrade);
            }
        }
        else
        {
            Debug.Log("Not enough gold to upgrade taunt attack:(");
        }


    }
}
