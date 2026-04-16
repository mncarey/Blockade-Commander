using UnityEngine;

public class WallUpgradeButton : MonoBehaviour
{
    public GameObject wallUpgradeMenuRef;
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
        wallUpgradeMenuRef.SetActive(true);

        //display the upgrade and cost
        upgradesTextRef.ShowUpgrades(resourceUIRef.goldCost1);

        //display upgrade level
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlWall);
    }
}
