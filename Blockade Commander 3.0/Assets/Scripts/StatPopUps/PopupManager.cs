using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private PlacingScript placingScriptRef;

    public GameObject tauntTowerPopup;
    public GameObject wallPopup;
    public GameObject cannonPopup;

    public GameObject currentPopup;

    // Update is called once per frame
    void Update()
    {
        if (placingScriptRef.showStats == true)
        {
            ShowCorrectPopup();
            placingScriptRef.showStats = false;
        }

        
    }

    private void ShowCorrectPopup()
    {

        GameObject clicked = placingScriptRef.clickedObject;

        if (clicked == null)
        {
            Debug.LogWarning("No clicked object.");
            return;
        }

        StatPopupUI popupUI = null;

        if (clicked.GetComponent<TauntTower>() != null)
        {
            currentPopup = tauntTowerPopup;
            popupUI = tauntTowerPopup.GetComponent<StatPopupUI>();

            //show correct stats based on taunt tower's script
            var tower = clicked.GetComponent<TauntTower>();
            popupUI.ShowStats(tower.health, tower.range, 0);
        }
        else if (clicked.GetComponent<Cannon>() != null)
        {
            currentPopup = cannonPopup;
            popupUI = cannonPopup.GetComponent<StatPopupUI>();

            //show correct stats based on taunt tower's script
            var cannon = clicked.GetComponent<Cannon>();
            popupUI.ShowStats(cannon.health, cannon.range, cannon.dmg);
        }
        else if (clicked.GetComponent<Wall>() != null)
        {
            currentPopup = wallPopup;
            popupUI = wallPopup.GetComponent<StatPopupUI>();

            //show correct stats based on taunt tower's script
            var wall = clicked.GetComponent<Wall>();
            popupUI.ShowStats(wall.health, wall.range, wall.dmg);
        }
        else
        {
            Debug.LogWarning("No matching script found on clicked object.");
        }
    }

    public void CloseCurrentPopup()
    {
        currentPopup.SetActive(false);
        currentPopup = null;
    }
}
