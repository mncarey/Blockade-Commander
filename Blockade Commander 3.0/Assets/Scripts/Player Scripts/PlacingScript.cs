using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static FortData;

public class PlacingScript : MonoBehaviour
{
    public TutorialSequence tutorialRef;
    public ResourceUI resourceRef;
    public StartWaveButton startWaveButton;
    public GameObject enemiesWinPopupRef;
    public GameObject objectToPlace;
    public GameObject upgradeMenuRef;
    public Camera mainCamera;

    private bool validFortSelected = false;
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

    void Start()
    {
        tutorialRef = FindObjectOfType<TutorialSequence>();
    }

    private void OnEnable()
    {
        clickAction.performed += OnClickPerformed;
    }

    private void OnDisable()
    {
        clickAction.performed -= OnClickPerformed;
    }

    private void FixedUpdate()
    {
        UpdatePreviewPosition();
        UpdateOutlineHover();

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
        if (fort == null)
        {
            validFortSelected = false;
            return;
        }

        FortData fortData = fort.GetComponent<FortData>();
        if (fortData == null)
        {
            validFortSelected = false;
            return;
        }
        objectToPlace = fort;
        currentFortType = fortData.fortType;
        validFortSelected = true;
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

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        tutorialRef.Advance();
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Vector2 mousePos = pointAction.ReadValue<Vector2>();
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (!context.performed) return;

        if (startPlaceState == false)
        {

        }
        else
        {
            if (removalToggle == false)
            {
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placeableObjectsLayer))
                {
                    Transform clickedRoot = hit.collider.transform.root;

                    bool withinTime = (Time.time - lastClickTime) <= doubleClickTime;
                    bool sameTarget = (lastClickedRoot == clickedRoot);

                    if (withinTime && sameTarget)
                    {
                        clickedObject = clickedRoot.gameObject;
                        showStats = true;
                        placementEnable = false;
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

                if (validFortSelected && placementEnable && canPlace && Physics.Raycast(ray, out RaycastHit groundHit, float.MaxValue, groundLayer))
                {
                    if (!CanPlaceType(currentFortType))
                    {
                        Debug.Log("Max placement for " + currentFortType + " reached!");
                    }
                    else
                    {
                        GameObject placedObject = Instantiate(objectToPlace, groundHit.point, objectToPlace.transform.rotation);
                        currentPlaced++;
                        ModifyFortCount(currentFortType, +1);
                        UpdateFortNumber();

                        // Hook up CannonRotate if this is a cannon
                        Cannon cannonScript = placedObject.GetComponent<Cannon>();
                        CannonRotate rotateScript = placedObject.GetComponentInChildren<CannonRotate>();

                        if (cannonScript != null && rotateScript != null)
                            rotateScript.Initialize(cannonScript);

                        if (currentPlaced == maxPlaced)
                        {
                            tutorialRef.UnlockCondition("placedFourUnits");
                        }
                    }
                }
            }
            else
            {
                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, placeableObjectsLayer))
                {
                    GameObject objectToRemove = hit.collider.transform.root.gameObject;

                    if (objectToRemove.CompareTag("Fortification"))
                    {
                        foreach (var col in objectToRemove.GetComponentsInChildren<Collider>())
                            col.enabled = false;

                        FortType removedType = objectToRemove.GetComponent<FortData>().fortType;

                        Destroy(objectToRemove);
                        currentPlaced--;
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

    public int GetFortCount(FortType type)
    {
        return type switch
        {
            FortType.Cannon => numCannon,
            FortType.Mortar => numMortar,
            FortType.Wall => numWall,
            FortType.Lighthouse => numLighthouse,
            _ => 0
        };
    }

    //increase maximum placed
    public void IncreasePlacementCap()
    {
        maxPlaced++;
    }
}