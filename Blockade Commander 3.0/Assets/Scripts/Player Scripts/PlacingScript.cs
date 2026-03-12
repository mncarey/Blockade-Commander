using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class PlacingScript : MonoBehaviour
{

    public StartWaveButton startWaveButton;
    public GameObject objectToPlace;
    public Camera mainCamera;

    //---- Layer Selections ----//
    public LayerMask groundLayer;
    public LayerMask placeableObjectsLayer;

    
    public float doubleClickTime = 0.3f;
    private float lastClickTime;

    //---- Fortification Placement Restriction ----//
    public int currentPlaced = 0;
    public int maxPlaced = 4;
    public bool canPlace => currentPlaced < maxPlaced;
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
        startWaveButton.gameObject.SetActive(!canPlace);

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

        if(startPlaceState == false)
        {
            Debug.Log("can't place");
        }
        else
        {
            if (removalToggle == false)
            {
                // Check for Rotation/Interaction via the object layer
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placeableObjectsLayer))
                {
                    //Double Click to Rotate
                    if (Time.time - lastClickTime <= doubleClickTime)
                    {
                        hit.transform.Rotate(0f, 90f, 0f);
                        showStats = true;
                        Debug.Log("your mom");
                    }

                    lastClickTime = Time.time;

                    return;
                }

                // Check for Placement on the ground layer
                if (canPlace && Physics.Raycast(ray, out RaycastHit groundHit, float.MaxValue, groundLayer))
                {
                    Instantiate(objectToPlace, groundHit.point, Quaternion.identity);
                    currentPlaced++;

                }

                lastClickTime = Time.time;
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

                        Debug.Log($"Removed {objectToRemove.name}. Remaining: {currentPlaced}");
                    }
                    return;
                }
            }
            
        }
        
    }

    public void SetCurrentFort(GameObject fort) => objectToPlace = fort;
}