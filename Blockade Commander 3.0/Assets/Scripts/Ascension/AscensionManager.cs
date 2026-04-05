using UnityEngine;

public class AscensionManager : MonoBehaviour
{
    //Declarations
    private bool ascensionUIOn = false;

    [SerializeField] GameObject AscensionUIRef;
    private ResourceUI KillsRef;
    private int killNum;






    private void Start()
    {
        KillsRef = FindObjectOfType<ResourceUI>();
        AscensionUIRef.SetActive(false);
    }

    private void FixedUpdate()
    {
       killNum = KillsRef.kills;
        
    }





    //Toggle UI asset
    public void ToggleUI()
    {
        if(killNum >= 2)
        {
            ascensionUIOn = !ascensionUIOn;
            //if true, turn on the UI else turn it off
            if (ascensionUIOn) AscensionUIRef.SetActive(true);
            else AscensionUIRef.SetActive(false);
        }
        
    }

    //Unlock fortification
    //Input kill number, if greater than minimum threshold but less than next threshold, unlock fort 1, repeat for next fort
    protected void Ascend(int kills)
    {
        if(kills >= 2)
        {
            
            //Unlock Mortar
            Debug.Log("Ascend");
        }
    }

    public void ClickedIcon()
    {
        ToggleUI();


    }
    //when ascend clicks
    public void AscendClicked()
    {

    }

}
