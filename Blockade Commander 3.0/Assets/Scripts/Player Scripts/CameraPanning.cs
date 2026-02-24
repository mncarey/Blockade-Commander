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
        if (moveLeft)
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (moveRight)
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;

        // Clamp position so it stays within bounds
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    public void StartMoveLeft()
    {
        moveLeft = true;
    }

    public void StopMoveLeft()
    {
        moveLeft = false;
    }

    public void StartMoveRight()
    {
        moveRight = true;
    }

    public void StopMoveRight()
    {
        moveRight = false;
    }
}
