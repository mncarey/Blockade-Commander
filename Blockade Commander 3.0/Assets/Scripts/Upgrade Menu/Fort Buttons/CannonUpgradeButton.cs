using UnityEngine;

public class CannonUpgradeButton : MonoBehaviour
{
    public GameObject cannonUpgradeMenuRef;
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
        cannonUpgradeMenuRef.SetActive(true);
        upgradesTextRef.ShowUpgrades(resourceUIRef.goldCost1);
    }
}
