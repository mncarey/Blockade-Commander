using UnityEngine;

public class MortarUpgradeButton : MonoBehaviour
{
    public GameObject mortarUpgradeMenuRef;
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
        mortarUpgradeMenuRef.SetActive(true);

        //display the upgrade and cost
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.mortarPrice); ;

        //display upgrade level
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlMortar);
    }
}
