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
    public void IWasClicked()
    {
        transform.root.gameObject.SetActive(false);

        //unpause game
        Time.timeScale = 1f;

        //unblock placement
        placingRef.placementEnable = true;
    }
}
