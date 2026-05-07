using UnityEngine;

public class UpgradeCannonHealthButton : MonoBehaviour
{
    private ResourceUI resourceUIRef;

    [SerializeField] private UpgradeManager upgradeManagerRef;
    public UpgradesText upgradesTextRef;
    public AudioSource audioRef;
    public GameObject noMoneyPopup;
    public GameObject noLvlsPopup;

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
        int currentLevel = upgradeManagerRef.upgradeLvlCannon;

        if (currentLevel >= upgradeManagerRef.maxUpgradeLvl)
        {
            Debug.Log("not high enough level to upgrade");
            noLvlsPopup.gameObject.SetActive(true);
            return;

        }

        if (resourceUIRef.gold < upgradeManagerRef.cannonPrice)
        {
            Debug.Log("not enough gold to upgrade cannon health :( ");
            noMoneyPopup.gameObject.SetActive(true);

            return;
        }

        //play coin sound
        audioRef.Play();
        //subtract gold
        resourceUIRef.gold -= upgradeManagerRef.cannonPrice;
        //update health
        resourceUIRef.cannonHealthUpgrade++;
        //update upgrade level
        upgradeManagerRef.upgradeLvlCannon++;
        //update upgrade price
        upgradeManagerRef.cannonPrice = upgradeManagerRef.cannonPrice + 30;


        //display correct text
        resourceUIRef.UpdateResourceUI();
        //display upgrade level
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlCannon);

        //display correct price
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.cannonPrice);

        Debug.Log($"Upgrade cannon health +1 for {upgradeManagerRef.cannonPrice} gold (now level {upgradeManagerRef.upgradeLvlCannon})");
       
        //upgrade cannons already in the scene
        Cannon[] cannons = FindObjectsByType<Cannon>(FindObjectsSortMode.None);
        foreach (Cannon cannon in cannons)
        {
           cannon.ApplyHealthUpgrade(resourceUIRef.cannonHealthUpgrade);
        }
    }
}
