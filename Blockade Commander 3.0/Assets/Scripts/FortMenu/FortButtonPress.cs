using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FortButtonPress : MonoBehaviour
{

    public GameObject FortMenuRef;
    public GameObject FortButtonRef;
    public GameObject FortExitRef;

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
        Debug.Log("Clicked");
        FortMenuRef.SetActive(true);
        FortExitRef.SetActive(true);
        FortButtonRef.SetActive(false);

    }

    public void ExitFortMenu()
    {
        FortExitRef.SetActive(false);
        FortMenuRef.SetActive(false);
        FortButtonRef.SetActive(true);
    }
}
