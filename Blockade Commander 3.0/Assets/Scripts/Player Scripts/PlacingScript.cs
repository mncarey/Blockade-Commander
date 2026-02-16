using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingScript : MonoBehaviour
{
    public GameObject objectToPlace;

    public Camera mainCamera;

    public LayerMask layerMask;

    private Vector3 placeToSpawn;

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask)){
            transform.position = raycastHit.point;

            placeToSpawn = transform.position;

        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Instantiate(objectToPlace, gameObject.transform.position, Quaternion.identity);
        }
    }

   

}
