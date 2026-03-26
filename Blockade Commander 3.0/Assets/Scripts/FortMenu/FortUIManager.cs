using UnityEngine;

public class FortUIManager : MonoBehaviour
{
    public GameObject fortMenu;
    public GameObject fortButton;
    public GameObject fortExit;
    public GameObject fortRemove;
    public GameObject fortSelection;

    public void ShowFortMenu()
    {
        fortMenu.SetActive(true);
        fortExit.SetActive(true);
        fortRemove.SetActive(true);
        fortButton.SetActive(false);
        fortSelection.SetActive(true);
    }

    public void HideFortMenu()
    {
        fortMenu.SetActive(false);
        fortExit.SetActive(false);
        fortRemove.SetActive(false);
        fortButton.SetActive(true);
        fortSelection.SetActive(false);
    }

    public void ResetForNextWave()
    {
        fortMenu.SetActive(true);
        fortExit.SetActive(true);
        fortRemove.SetActive(true);
        fortButton.SetActive(false);
        fortSelection.SetActive(true);

        
    }
}
