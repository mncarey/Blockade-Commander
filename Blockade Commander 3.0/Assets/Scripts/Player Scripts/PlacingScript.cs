using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static FortData;

public class PlacingScript : MonoBehaviour
{

    public ResourceUI resourceRef;
    public StartWaveButton startWaveButton;
    public GameObject enemiesWinPopupRef;
    public GameObject objectToPlace;
    public GameObject upgradeMenuRef;
    public Camera mainCamera;
    

    //---- Layer Selections ----//
    public LayerMask groundLayer;
    public LayerMask placeableObjectsLayer;

    //---- Double click feature ----//
    public float doubleClickTime = 0.3f;
    private float lastClickTime = -999f;
    private Transform lastClickedRoot = null;
    [HideInInspector] public GameObject clickedObject;

    //---- Fortification Placement Restriction ----//
    public int currentPlaced = 0;
    public int maxPlaced = 4;
    public int fortsAilve = 0;

    //Set up for fortification specific limit
    public int numCannon = 0;
    public int numMortar = 0;
    public int numWall = 0;
    public int numLighthouse = 0;
    //if any of these exceed 3, set placementEnable to false
    public int maxPerType = 3;
    //Enum attached to each fortification prefab, designates what type they are
    private FortType currentFortType;
    public bool canPlace => currentPlaced < maxPlaced;
    public bool placementEnable = true;
    public bool removalToggle = false;
    public bool startPlaceState = false;
    public bool showStats = false;

    //---- Outline ----//
    private Outline currentOutline;
    
    //---- Input Action References ----//
    private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction pointAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
        //Input action link
        clickAction = playerInput.actions["Click"];
        pointAction = playerInput.actions["Point"];

        resourceRef = FindObjectOfType<ResourceUI>();
    }

    private void OnEnable()
    {
        
        clickAction.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        
        clickAction.performed -= OnClickPerformed;
    }

    //Update checks for changes based on information received from the Input Action events
    private void FixedUpdate()
    {
        //Checks the position of the mouse
        UpdatePreviewPosition();
        //Checks the outline function if applicable
        UpdateOutlineHover();

        //start wave
        if (currentPlaced == maxPlaced)
        {
            startWaveButton.gameObject.SetActive(true);
        }
        else
        {
            startWaveButton.gameObject.SetActive(false);
        }


        if (startWaveButton.isClicked)
        {
            startWaveButton.gameObject.SetActive(false);
        }

        
    }

    public void SetCurrentFort(GameObject fort)
    {
        objectToPlace = fort;
        // Read the type directly from the prefab
        currentFortType = fort.GetComponent<FortData>().fortType;
    }
    private void UpdatePreviewPosition()
    {
        Vector2 mousePos = pointAction.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundLayer))
        {
            transform.position = hit.point;
        }
    }

    private void UpdateOutlineHover()
    {
        Vector2 mousePos = pointAction.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        //If the preview is on the placable object layer
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placeableObjectsLayer))
        {
            if (hit.transform.TryGetComponent(out Outline foundOutline))
            {
                if (currentOutline != foundOutline)
                {
                    
                    currentOutline?.OutlineBoolFunc(false);
                    currentOutline = foundOutline;
                    currentOutline.OutlineBoolFunc(true);
                }
            }
        }
        else
        {
            currentOutline?.OutlineBoolFunc(false);
            currentOutline = null;
        }
    }

    // Listener which runs when you tap or click
    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Vector2 mousePos = pointAction.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (!context.performed) return;

        if(startPlaceState == false)
        {
            
        }
        else
        {
            if (removalToggle == false)
            {
                // Check for Rotation/Interaction via the object layer
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placeableObjectsLayer))
                {
                    Transform clickedRoot = hit.collider.transform.root;

                    bool withinTime = (Time.time - lastClickTime) <= doubleClickTime;
                    bool sameTarget = (lastClickedRoot == clickedRoot);

                    //Double Click to Rotate and Open Stats
                    if (withinTime && sameTarget)
                    {
                        clickedObject = clickedRoot.gameObject;
                        clickedRoot.Rotate(0f, 90f, 0f);
                        showStats = true;

                        //block placement when stats are open
                         placementEnable = false;
                        

                        //reseting variables
                        lastClickTime = -999f;
                        lastClickedRoot = null;
                    }
                    else
                    {
                        lastClickTime = Time.time;
                        lastClickedRoot = clickedRoot;
                    }

                    return;
                }

                // Check for Placement on the ground layer
                if (placementEnable && canPlace && Physics.Raycast(ray, out RaycastHit groundHit, float.MaxValue, groundLayer))
                {
                    //if you have hit the max type, tell the player
                    if (!CanPlaceType(currentFortType))
                    {
                        Debug.Log("Max placement for " + currentFortType + " reached!");
                    }
                    //else place it
                    else
                    {
                        Instantiate(objectToPlace, groundHit.point, Quaternion.identity);
                        currentPlaced++;
                        ModifyFortCount(currentFortType, +1);
                        UpdateFortNumber();
                    }
                }


            }
            else
            {
                if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placeableObjectsLayer))
{
                    // Find the highest object in the hierarchy that belongs to this prefab
                    GameObject objectToRemove = hit.collider.transform.root.gameObject;


                    //Check to make sure that it is what we want to remove using the tag "Fortification"
                    if (objectToRemove.CompareTag("Fortification"))
                    {
                        foreach (var col in objectToRemove.GetComponentsInChildren<Collider>())
                            col.enabled = false;

                        FortType removedType = objectToRemove.GetComponent<FortData>().fortType;

                        Destroy(objectToRemove);
                        currentPlaced--;
                        //subtract from that types value so you can place more later
                        ModifyFortCount(removedType, -1);
                        UpdateFortNumber();
                    }
                    return;
                }
            }
            
        }
        
    }

    public void TogglePlacementLock()
    {
        placementEnable = !placementEnable;
    }

    public void UpdateFortNumber()
    {
        
        resourceRef.UpdateFortRef(currentPlaced);
    }

    //Checks that each type is under the maximum alotted number.
    private bool CanPlaceType(FortType type)
    {
        return type switch
        {
            FortType.Cannon => numCannon < maxPerType,
            FortType.Mortar => numMortar < maxPerType,
            FortType.Wall => numWall < maxPerType,
            FortType.Lighthouse => numLighthouse < maxPerType,
            _ => true
        };
    }

    //take the FortType enum, goes through and will add to the individual number for that
    //specific type.
    private void ModifyFortCount(FortType type, int delta)
    {
        switch (type)
        {
            case FortType.Cannon: numCannon += delta; break;
            case FortType.Mortar: numMortar += delta; break;
            case FortType.Wall: numWall += delta; break;
            case FortType.Lighthouse: numLighthouse += delta; break;
        }
    }

    public void ResetFortTypePlacement()
    {
        numCannon = 0;
        numMortar = 0;
        numWall = 0;
        numLighthouse = 0;

    }

}