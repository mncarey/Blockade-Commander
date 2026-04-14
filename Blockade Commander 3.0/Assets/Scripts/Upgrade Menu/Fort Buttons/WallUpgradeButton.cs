using UnityEngine;

public class WallUpgradeButton : MonoBehaviour
{
    public GameObject wallUpgradeMenuRef;
    [SerializeField] private UpgradesText upgradesTextRef;
    [SerializeField] private ResourceUI resourceUIRef;

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
        upgradesTextRef.ShowUpgrades(resourceUIRef.goldCost1);
    }
}
