using UnityEngine;
using UnityEngine.InputSystem;
/*
 * Author: [Ruffner, Kaylie]
 * Date of Creation: [2-22-2026]
 * Summary: [This script handles the panning mechanic with the game's camera]
 */

public class CameraPanning : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float minX = -5f;   // Left boundary
    public float maxX = 5f;    // Right boundary

    private bool moveLeft;
    private bool moveRight;

    void Update()
    {
        // moves the camera in the left direction 
        if (moveLeft)
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        // moves the camera in the right direction
        if (moveRight)
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        // clamp position so it stays within bounds and gets the position minX maxX and stops moving
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    // starts moving left and makes it true
    public void StartMoveLeft()
    {
        moveLeft = true;
    }

     // stops moving left and makes it false
    public void StopMoveLeft()
    {
        moveLeft = false;
    }

    // starts moving right and makes it true
    public void StartMoveRight()
    {
        moveRight = true;
    }

    // stops moving right and makes it false
    public void StopMoveRight()
    {
        moveRight = false;
    }
}
