using UnityEngine;

public class AscensionManager : MonoBehaviour
{
    //Declarations
    private bool ascensionUIOn = false;

    [SerializeField] GameObject AscensionUIRef;
    [SerializeField] GameObject AscendTextRef;
    [SerializeField] GameObject LockedIcon1;
    [SerializeField] GameObject LockedIcon2;
    [SerializeField] GameObject MortarSelection;
    [SerializeField] GameObject LighthouseSelection;
   
    private ResourceUI KillsRef;
    private int killNum;
    private int killThreshold;

    public int ascensionTracker = 0;

    UpgradeManager upgradeRef;
    public TextFader textFader;

    private void Start()
    {
        KillsRef = FindObjectOfType<ResourceUI>();
        AscensionUIRef.SetActive(false);
        killThreshold = 5;
        textFader = FindObjectOfType<TextFader>();
        upgradeRef = FindObjectOfType<UpgradeManager>();
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
        if(ascensionTracker == 0)
        {
            
            if (killNum >= killThreshold)
            {
                upgradeRef.maxUpgradeLvl = 3;
                //turn off lockedFort
                LockedIcon1.SetActive(false);
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

        if (ascensionTracker == 1)
        {
            upgradeRef.maxUpgradeLvl = 5;
            textFader.FadeInThenOut(holdTime: 2f);
            KillsRef.AscendResetResource();
            
            killThreshold = killThreshold * 2;

        }
        if (ascensionTracker == 2)
        {
            upgradeRef.maxUpgradeLvl = 7;
            //turn off lockedFort
            LockedIcon2.SetActive(false);
            //turn on Lighthouse Icon
            LighthouseSelection.SetActive(true);
            KillsRef.AscendResetResource();
            //unlock upgrade cap to lvl 10
            //increase threshold for next ascension and reset gold
            killThreshold = killThreshold * 2;
            
        }
        ascensionTracker++;

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
