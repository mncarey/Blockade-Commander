using UnityEngine;

public class UpgradeTauntHealthButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;
    public GameObject tauntTowerStatsPopup;
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
            resourceUIRef.tauntHealthUpgrade++;
            //subtract gold
            resourceUIRef.gold -= resourceUIRef.goldCost1;
            resourceUIRef.UpdateResourceUI();
            Debug.Log("Upgraded Taunt Health + 1 for " + resourceUIRef.goldCost1 + " gold");

            //upgrade towers already in the scene
            TauntTower[] towers = FindObjectsByType<TauntTower>(FindObjectsSortMode.None);
            foreach (TauntTower tower in towers)
            {
                tower.ApplyHealthUpgrade(resourceUIRef.tauntHealthUpgrade);
            }
        }
        else
        {
            Debug.Log("Not enough gold to upgrade taunt health:(");
        }

      

        //show an upgrade completed screen?
    }
}
