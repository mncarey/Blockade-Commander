using UnityEngine;
using UnityEngine.InputSystem;

public class PlacingScript : MonoBehaviour
{
    public GameObject objectToPlace;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)//left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//creates a ray shooting out from camera to mouse position
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(objectToPlace, hit.point, Quaternion.identity);//check to see if a ray hits a collider.
            }
        }
    }
}
