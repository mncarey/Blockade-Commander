using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Collections;

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

        if (clicked.GetComponent<TauntTower>() != null)
        {
            tauntTowerPopup.SetActive(true);
            currentPopup = tauntTowerPopup;
        }
        else if (clicked.GetComponent<Cannon>() != null)
        {
            cannonPopup.SetActive(true);
            currentPopup = cannonPopup;
        }
        else if (clicked.GetComponent<Wall>() != null)
        {
            wallPopup.SetActive(true);
            currentPopup = wallPopup;
        }
        else if (clicked.GetComponent<Mortar>() != null)
        {
            placingScriptRef.placementEnable = true;
            Debug.Log("Clicked on Mortar");
        }
        else
        {
            Debug.LogWarning("No matching script found on clicked object.");
            placingScriptRef.placementEnable = true;
        }
    }

    public void CloseCurrentPopup()
    {
        currentPopup.SetActive(false);
        currentPopup = null;
        StartCoroutine(ReenaablePlacementNextFrame());

    }

    private IEnumerator ReenaablePlacementNextFrame()
    {
        yield return null;
        placingScriptRef.placementEnable = true;
    }
}
