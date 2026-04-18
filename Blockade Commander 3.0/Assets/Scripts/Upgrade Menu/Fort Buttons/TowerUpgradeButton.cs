using UnityEngine;

public class TowerUpgradeButton : MonoBehaviour
{
    public GameObject towerUpgradeMenu;
    [SerializeField] private UpgradesText upgradesTextRef;
    [SerializeField] private ResourceUI resourceUIRef;
    [SerializeField] private UpgradeManager upgradeManagerRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IWasClicked()
    {
        towerUpgradeMenu.SetActive(true);

        //display the upgrade and cost
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.tauntPrice);
        

        //display upgrade level
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlTaunt);
    }


}
