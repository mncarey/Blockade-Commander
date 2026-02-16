using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction clickToPlace;
    private Vector2 placeObject;

    private void Update()
    {
        //placeObject = clickToPlace.ReadValue<Vector2>();
    }

    private void OnEnable()
    {
        clickToPlace.Enable();
    }

    private void OnDisable()
    {
        clickToPlace.Disable();
    }

}
