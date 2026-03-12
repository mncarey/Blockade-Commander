using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private PlacingScript placingScriptRef;
    [SerializeField] private GameObject popupObject;

   private void Awake()
    {
        popupObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(placingScriptRef.showStats == true)
        {
            popupObject.gameObject.SetActive(true);
            placingScriptRef.showStats = false;
        }
    }
}
