using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FortButtonPress : MonoBehaviour
{

    public GameObject FortMenuRef;
    public GameObject FortButtonRef;
    public GameObject FortExitRef;
    public GameObject FortRemoveRef;

    public PlacingScript placingScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //Finds the script in the scene to call
        if(placingScript == null)
        {
            placingScript = FindAnyObjectByType<PlacingScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  

    public void IWasClicked()
    {
        Debug.Log("Clicked");
        FortMenuRef.SetActive(true);
        FortExitRef.SetActive(true);
        FortRemoveRef.SetActive(true);
        FortButtonRef.SetActive(false);

    }

    public void ExitFortMenu()
    {
        FortExitRef.SetActive(false);
        FortMenuRef.SetActive(false);
        FortRemoveRef.SetActive(false);
        FortButtonRef.SetActive(true);
    }

    //When clicked, this will toggle the ability to place fortifications off and turn on the ability to remove them
    public void RemovalToggle()
    {
        if(placingScript.removalToggle == false)
        {
            placingScript.removalToggle = true;
        }
        else
        {
            placingScript.removalToggle = false;
        }
    }
}
