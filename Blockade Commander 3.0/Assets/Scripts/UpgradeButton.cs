using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    public GameObject upgradeMenuRef;
    public GameObject buttonObject;

    public PlacingScript placingScriptRef;
    public ResourceUI resourceRef;

    public int upgradeLvl = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public bool CanUpgrade()
    {
        if(resourceRef.gold >= resourceRef.goldCost1)
        {
            return true;
        }
        return false;
    }

    public void ShowUpgradeButton(bool show)
    {
        buttonObject.SetActive(show);
    }

    public void IWasClicked()
    {
        upgradeMenuRef.SetActive(true);
        //deactivate placement while the upgrade menu is open
        placingScriptRef.placementEnable = false;

        placingScriptRef.showStats = true;

        //dont nessesarily want the button to deactivate if the player still has gold to use
        if(resourceRef.gold < resourceRef.goldCost1)
        {
           buttonObject.SetActive(false);
        }

    }
}
