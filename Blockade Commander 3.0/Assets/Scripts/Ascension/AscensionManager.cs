using UnityEngine;

public class AscensionManager : MonoBehaviour
{
    //Declarations
    private bool ascensionUIOn = false;

    [SerializeField] GameObject AscensionUIRef;
    [SerializeField] GameObject AscendTextRef;
    [SerializeField] GameObject LockedIcon;
    [SerializeField] GameObject MortarSelection;
   
    private ResourceUI KillsRef;
    private int killNum;
    private int killThreshold;






    private void Start()
    {
        KillsRef = FindObjectOfType<ResourceUI>();
        AscensionUIRef.SetActive(false);
        killThreshold = 5;
    }

    private void FixedUpdate()
    {
       killNum = KillsRef.kills;
        
    }





    //Toggle UI asset
    public void ToggleUI()
    {
        if(killNum >= killThreshold)
        {
            ascensionUIOn = !ascensionUIOn;
            //if true, turn on the UI else turn it off
            if (ascensionUIOn) AscensionUIRef.SetActive(true);
            else AscensionUIRef.SetActive(false);

            
            
        }
        
    }

    //Unlock fortification
    //Input kill number, if greater than minimum threshold but less than next threshold, unlock fort 1, repeat for next fort
    protected void Ascend()
    {
        if(killNum >= killThreshold)
        {
            //turn off lockedFort
            LockedIcon.SetActive(false);
            //turn on Mortar Icon
            MortarSelection.SetActive(true);
            KillsRef.AscendResetResource();
            //unlock upgrade cap to lvl 10
            //increase threshold for next ascension and reset gold
            killThreshold = killThreshold * 2;
            // Increase upgrade capacity
            //After 3rd ascension increase number of fortifications able to be placed

        }
    }

    public void ClickedIcon()
    {
        ToggleUI();
        AscendInfo();

    }
    //when ascend clicks
    public void AscendClicked()
    {
        AscendTextRef.SetActive(false);
        Ascend();
    }

    //Display the ascension view menu
    //It is mostly text, saying explaining that you will unlock the mortar, but reset your gold and enemies
    private void AscendInfo()
    {
        AscendTextRef.SetActive(true);
    }
}
