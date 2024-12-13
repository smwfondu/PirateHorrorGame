using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public float launchSpeed = 25f;

    public void LaunchFish(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = direction.normalized * launchSpeed; // Set the velocity in the given direction
    }
}
