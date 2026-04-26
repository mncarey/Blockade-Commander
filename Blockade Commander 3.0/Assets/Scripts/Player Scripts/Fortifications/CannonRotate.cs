using UnityEngine;

public class CannonRotate : MonoBehaviour
{
    private Cannon cannonRef;

    //Once this exists in the game scene, since we can't use start
    public void Initialize(Cannon cannon)
    {
        cannonRef = cannon;
    }

    //Move it to face the current enemy
    void Update()
    {
        if (cannonRef == null || cannonRef.currentTarget == null) return;

        Vector3 direction = cannonRef.currentTarget.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 150f * Time.deltaTime);
    }
}