using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class fortSlot : MonoBehaviour
{
    public TMP_Text fortDescrptionName;
    public TMP_Text fortDescriptionText;
    public string fortDescription;
    //====== FORT DATA =======//
    
    public Sprite fortSprite;

    public FortMenuController slotContainerRef;
    public GameObject fortReference;
    

    //====== FORT SLOT ======//

    


    public GameObject selectedShader;
    public bool thisFortSelected;

    private Toggle toggle;
    
    private bool wasOn = false;

    public string content = "This is the Content of this Fortification";


    [SerializeField] private FortImageDisplay fortImageDisplay;
    [SerializeField] private Sprite newSprite;

    private string fortName; 
  void Awake()
    {
        
        
        
        toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            
            //setting event listener, whenever isOn changes, run OnIsOnChanged
            toggle.onValueChanged.AddListener(OnIsOnChanged);
            
            
        }
        fortName = gameObject.name;       
        fortImageDisplay = FindObjectOfType<FortImageDisplay>();
        
        
        
    }

    void Update()
    {
        

        //Watching for isOn to go off when user clicks another toggle
        //if wasOn is false, it does not attempt this again.
        if (wasOn && !toggle.isOn)
        {
            //Debug.Log("Deselected " + this.name);
            selectedShader.SetActive(false);
            wasOn = false;
        }
    }
    
    void Start()
    {
        Transform child = transform.Find("FortImage");
        //Image childImage = child.GetComponent<Image>();
        

    }
    
    
    
    //Event Listener, tied to toggle on value change.
    //3 instances where isOn changes
    //  1.When user clicks on this, isOn is changed from off to on.
    //  2.When user clicks on this object again.
    //  3.When the user clicks on another toggle, isOn is turned off, but it does not trigger this event.
    public void OnIsOnChanged(bool isOn)
    {
        if (isOn && !wasOn)
        {
            
            //Debug.Log("Selected " + this.name);
            //Debug.Log(toggle.group.name);
           //turn off all toggles
            toggle.group.SetAllTogglesOff();
            
            selectedShader.SetActive(true);
            wasOn = true;
            //turns this toggle back on
            toggle.isOn = true;
            fortImageDisplay.ChangeImage(newSprite, fortName, content);
            //set the display image to the image associated with this object
           
        }

        
    }

    
    
    
        
    
}
