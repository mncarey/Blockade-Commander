using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingScript : MonoBehaviour
{
    public GameObject objectToPlace;//asigned in inspector? dont sign in inspector?
    //want to create a bool that allows the player to even place an object or not
    public Camera mainCamera;

    public LayerMask groundLayer;
    public LayerMask placeableObjectsLayer;

    private Vector3 placeToSpawn;

    //implementing double click/tap to rotate
    private float doubleClickTime = 0.3f;
    private float lastClickTime;

    //implementing outline feature
    Outline outline;

    //public GameObject TauntTower;

    public void SetCurrentFort(GameObject fort)
    {
        objectToPlace = fort;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, groundLayer)){
            transform.position = raycastHit.point;
            placeToSpawn = transform.position;
            
        }

        if (Physics.Raycast(ray, out RaycastHit placeableHit, float.MaxValue, placeableObjectsLayer))
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                //check to find a double click/tap first
                if (Time.time - lastClickTime <= doubleClickTime)
                {
                    placeableHit.transform.Rotate(0f, 90f, 0f); //rotate object 90 degrees on y
                }

                lastClickTime = Time.time;//click time resets
                return;//dont also place a new object
            }

            if(placeableHit.transform.TryGetComponent(out Outline outline))//trying to check whether or not it has the Outline component
            {
                if(this.outline != outline)
                {
                    this.outline?.OutlineBoolFunc(false);//disable outline on previous object, if it exists
                }

                //override old outline with current outline on object
                this.outline = outline;
                outline.OutlineBoolFunc(true);
            }
            else
            {
                this.outline?.OutlineBoolFunc(false);
                this.outline = default;
            }
        }
        else
        {
            //if there is no object to be outlined, disable outline
            outline?.OutlineBoolFunc(false);
            outline = default;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Instantiate(objectToPlace, gameObject.transform.position, Quaternion.identity);
        }
          

    }   

}
