using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public bool isClicked;
    public GameObject statsPopupRef;
    public PlacingScript placingRef;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placingRef = FindObjectOfType<PlacingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IWasClicked()
    {
        statsPopupRef.SetActive(false);
        //unpause game
        Time.timeScale = 1f;

        //unblock placement
        placingRef.placementEnable = false;
    }
}
