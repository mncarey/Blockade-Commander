using UnityEngine;
using UnityEngine.InputSystem;
/*
 * Author: [Ruffner, Kaylie]
 * Date of Creation: [2-22-2026]
 * Summary: [This script handles the panning mechanic with the game's camera]
 */

public class CameraPanning : MonoBehaviour
{
    private Vector3 touchStart;
    public Camera cam;
    private bool isDragging;

    void Update()
    {
        // PRIORITY: Touch first (covers mobile + touchscreen PCs)
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.wasPressedThisFrame)
            {
                isDragging = true;
                touchStart = GetWorldPosition(touch.position.ReadValue());
            }

            if (touch.press.isPressed && isDragging)
            {
                Vector3 direction = touchStart - GetWorldPosition(touch.position.ReadValue());
                cam.transform.position += direction;
            }

            if (touch.press.wasReleasedThisFrame)
            {
                isDragging = false;
            }

            return; // Prevent mouse from also running
        }

        // Mouse fallback
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                isDragging = true;
                touchStart = GetWorldPosition(Mouse.current.position.ReadValue());
            }

            if (Mouse.current.leftButton.isPressed && isDragging)
            {
                Vector3 direction = touchStart - GetWorldPosition(Mouse.current.position.ReadValue());
                cam.transform.position += direction;
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                isDragging = false;
            }
        }
    }

    private Vector3 GetWorldPosition(Vector2 screenPos)
    {
        // Prevent invalid screen positions
        if (float.IsInfinity(screenPos.x) || float.IsInfinity(screenPos.y) ||
            float.IsNaN(screenPos.x) || float.IsNaN(screenPos.y))
        {
            return cam.transform.position;
        }

        Ray ray = cam.ScreenPointToRay(screenPos);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);

        return cam.transform.position;
    }
}
