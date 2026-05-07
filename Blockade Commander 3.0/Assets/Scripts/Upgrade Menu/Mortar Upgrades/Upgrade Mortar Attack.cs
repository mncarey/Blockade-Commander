using UnityEngine;

public class UpgradeMortarAttack : MonoBehaviour
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

    public void IWasClicked()
    {
        int currentLevel = upgradeManagerRef.upgradeLvlMortar;

        if (currentLevel >= upgradeManagerRef.maxUpgradeLvl)
        {
            Debug.Log("not high enough level to upgrade");
            noLvlsPopup.gameObject.SetActive(true);
            return;
        }

        if (resourceUIRef.gold < upgradeManagerRef.mortarPrice)
        {
            Debug.Log("not enough gold to upgrade mortar dmg :( ");
            noMoneyPopup.gameObject.SetActive(true);

            return;
        }

        //play coin sound
        audioRef.Play();
        //subtract gold
        resourceUIRef.gold -= upgradeManagerRef.mortarPrice;
        //update health
        resourceUIRef.mortarDmgUpgrade++;
        //update upgrade level
        upgradeManagerRef.upgradeLvlMortar++;
        //update upgrade price
        upgradeManagerRef.mortarPrice = upgradeManagerRef.mortarPrice + 30;

        //display correct text
        resourceUIRef.UpdateResourceUI();
        upgradesTextRef.ShowUpgradeLvl(upgradeManagerRef.upgradeLvlMortar);

        //display correct price
        upgradesTextRef.ShowUpgrades(upgradeManagerRef.mortarPrice);

        Debug.Log($"Upgrade mortar dmg +1 for {upgradeManagerRef.mortarPrice} gold (now level {upgradeManagerRef.upgradeLvlMortar})");

        //upgrade walls already in the scene
        Mortar[] mortars = FindObjectsByType<Mortar>(FindObjectsSortMode.None);
        foreach (Mortar mortar in mortars)
        {
            mortar.ApplyDmgUpgrade(resourceUIRef.mortarDmgUpgrade);
        }
    }
}
