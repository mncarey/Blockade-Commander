using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public bool isClicked;
    public GameObject statsPopupRef;
    public PlacingScript placingRef;
    public PopupManager popupManagerRef;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placingRef = FindObjectOfType<PlacingScript>();
        popupManagerRef = FindObjectOfType<PopupManager>();
    }
    public void IWasClicked()
    {
        popupManagerRef.CloseCurrentPopup();

        //unpause game
        Time.timeScale = 1f;

        //unblock placement
        placingRef.placementEnable = true;

        //undo showstats = true;
        placingRef.showStats = false;

        //deselect last object
        placingRef.clickedObject = null;
    }
}
