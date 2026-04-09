using UnityEngine;

public class ExitButton2 : MonoBehaviour
{
    public GameObject objectToDeactivate;
    public PlacingScript placingScriptRef;

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
        //re-enable placement if the object to close was the update menu
        if (objectToDeactivate.CompareTag("UpgradeMenu"))
        {
            if (placingScriptRef != null)
            {
                placingScriptRef.placementEnable = true;
            }
        }

        //deactivate popup
        objectToDeactivate.SetActive(false);

        if (placingScriptRef != null)
        {
            placingScriptRef.showStats = false;
        }
        
    }
}
