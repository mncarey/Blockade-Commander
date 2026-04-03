using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private PlacingScript placingScriptRef;

    public GameObject tauntTowerPopup;
    public GameObject wallPopup;
    public GameObject cannonPopup;

    // Update is called once per frame
    void Update()
    {
        if (placingScriptRef.showStats == true)
        {
            placingScriptRef.showStats = false;
        }

        ShowCorrectPopup();
    }

    private void ShowCorrectPopup()
    {

        GameObject clicked = placingScriptRef.clickedObject;

        if (clicked == null)
        {
            Debug.LogWarning("No clicked object.");
            return;
        }

        if (clicked.GetComponent<TauntTower>() != null)
        {
            tauntTowerPopup.SetActive(true);
        }
        else if (clicked.GetComponent<Cannon>() != null)
        {
            cannonPopup.SetActive(true);
        }
        else if (clicked.GetComponent<Wall>() != null)
        {
            wallPopup.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No matching script found on clicked object.");
        }
    }
}
