using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
public class PlacingScript : MonoBehaviour
{

    public ResourceUI resourceRef;
    public StartWaveButton startWaveButton;
    public GameObject enemiesWinPopupRef;
    public GameObject objectToPlace;
    public Camera mainCamera;

    //---- Layer Selections ----//
    public LayerMask groundLayer;
    public LayerMask placeableObjectsLayer;

    //---- Double click feature ----//
    public float doubleClickTime = 0.35f;
    private float lastClickTime = -999f;
    public float minDoubleClickTime = 0.08f;
    private Transform lastClickedRoot = null;

    //---- Fortification Placement Restriction ----//
    public int currentPlaced = 0;
    public int maxPlaced = 4;
    public int fortsAilve = 0;
    public bool canPlace => currentPlaced < maxPlaced && !showStats && placementEnable && !blockPlacement;
    public bool placementEnable = true;
    public bool removalToggle = false;
    public bool startPlaceState = false;
    public bool showStats = false;
    public bool blockPlacement = false;

    //---- Outline ----//
    private Outline currentOutline;
    
    //---- Input Action References ----//
    private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction pointAction;

    //---- Stats Popups ----//
    public GameObject tauntStatsPopup;
    public GameObject wallStatsPopup;
    public GameObject cannonStatsPopup;

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
    private void Update()
    {
        //Checks the position of the mouse
        UpdatePreviewPosition();
        //Checks the outline function if applicable
        UpdateOutlineHover();

        //start wave
        startWaveButton.gameObject.SetActive(currentPlaced > 0);

        if (startWaveButton.isClicked)
        {
            startWaveButton.gameObject.SetActive(false);
        }

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

        else
        {
            if (removalToggle == false)
            {
                // Check for Rotation/Interaction via the object layer
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placeableObjectsLayer))
                {
                    Transform clickedRoot = hit.collider.transform.root;

                    float timeSinceLastClick = Time.time - lastClickTime;
                    bool withinTime = timeSinceLastClick >= minDoubleClickTime && timeSinceLastClick <= doubleClickTime;
                    bool sameTarget = (lastClickedRoot == clickedRoot);

                    //Double Click to Rotate and Open Stats
                    if (withinTime && sameTarget)
                    {
                        //idk if we want this feature implemented
                        //clickedRoot.Rotate(0f, 90f, 0f);

                        if(clickedRoot.TryGetComponent(out TauntTower taunt))
                        {
                            blockPlacement = true;
                            tauntStatsPopup.SetActive(true);
                            Time.timeScale = 0f;
                        }

                        if (clickedRoot.TryGetComponent(out Cannon cannon))
                        {
                            blockPlacement = true;
                            cannonStatsPopup.SetActive(true);
                            tauntStatsPopup.SetActive(false);
                            Time.timeScale = 0f;
                        }

                        if (clickedRoot.TryGetComponent(out Wall wall))
                        {
                            blockPlacement = true;
                            wallStatsPopup.SetActive(true);
                            Time.timeScale = 0f;
                        }



                        //reseting variables
                        lastClickTime = -999f;
                        lastClickedRoot = null;
                    }
                    else
                    {
                        showStats = false;//hide stats on every unsuccessful double click
                        lastClickTime = Time.time;
                        lastClickedRoot = clickedRoot;
                    }

                    return;
                }

                // Check for Placement on the ground layer
                if (placementEnable && canPlace && Physics.Raycast(ray, out RaycastHit groundHit, float.MaxValue, groundLayer))
                {
                    GameObject newFort = Instantiate(objectToPlace, groundHit.point, Quaternion.identity);

                    //assigning popups based on fortification
                    // Assign popup references depending on type
                    if (newFort.TryGetComponent(out Cannon cannon))
                    {
                        cannon.Initialize(cannonStatsPopup.GetComponentInChildren<StatPopupUI>());
                        cannon.OpenStats();
                    }
                    else if (newFort.TryGetComponent(out Wall wall))
                    {
                       wall.Initialize(wallStatsPopup.GetComponentInChildren<StatPopupUI>());
                       wall.OpenStats();
                    }
                    else if (newFort.TryGetComponent(out TauntTower taunt))
                    {
                       taunt.Initialize(tauntStatsPopup.GetComponentInChildren<StatPopupUI>());
                       taunt.OpenStats();
                    }

                    currentPlaced++;
                    UpdateFortNumber();

                    //dont open stats when placing
                    showStats = false;

                    //reset click time
                    lastClickTime = -999f;
                    lastClickedRoot = null;
                }

                //lastClickTime = Time.time;
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
                        // Disable all colliders on the object immediately to stop further raycasts
                        foreach (var col in objectToRemove.GetComponentsInChildren<Collider>())
                        {
                            col.enabled = false;
                        }

                        Destroy(objectToRemove);
                        currentPlaced--;
                        //set the number in resource UI to the new value

                        UpdateFortNumber();
                        //Debug.Log($"Removed {objectToRemove.name}. Remaining: {currentPlaced}");
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

    public void SetCurrentFort(GameObject fort) => objectToPlace = fort;
}