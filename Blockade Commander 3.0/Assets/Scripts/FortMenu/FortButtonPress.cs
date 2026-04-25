using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FortButtonPress : MonoBehaviour
{
    public TutorialSequence tutorialRef;
    public GameObject FortMenuRef;
    public GameObject FortButtonRef;
    public GameObject FortExitRef;
    public GameObject FortRemoveRef;

    //---- UI Image ----//
    public GameObject RemoveIcon;
    public GameObject PlaceIcon;

    public PlacingScript placingScript;
    public FortUIManager fortUIManagerRef;
    private bool firstTimeClick = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //Finds the script in the scene to call
        if(placingScript == null)
        {
            placingScript = FindAnyObjectByType<PlacingScript>();
        }
        fortUIManagerRef = FindObjectOfType<FortUIManager>();
        tutorialRef = FindObjectOfType<TutorialSequence>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    public void IWasClicked()
    {
        if (firstTimeClick)
        {
            //unlock the tutorial text
             
            tutorialRef.UnlockCondition("tappedMenu");
            
            fortUIManagerRef.ShowFortMenu();
            firstTimeClick = false;
        }
        else
        {

            fortUIManagerRef.ShowFortMenu();
        }
            

    }

    public void ExitFortMenu()
    {
        FortExitRef.SetActive(false);
        FortMenuRef.SetActive(false);
        FortRemoveRef.SetActive(false);
        FortButtonRef.SetActive(true);
        placingScript.startPlaceState = false;
    }

    //When clicked, this will toggle the ability to place fortifications off and turn on the ability to remove them
    public void RemovalToggle()
    {
        if(placingScript.removalToggle == false)
        {
            placingScript.removalToggle = true;
            RemoveIcon.SetActive(false);
            PlaceIcon.SetActive(true);
        }
        else
        {
            placingScript.removalToggle = false;
            RemoveIcon.SetActive(true);
            PlaceIcon.SetActive(false);
        }
    }
}
