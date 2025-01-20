using UnityEngine;

public class CameraLookScript : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of the camera rotation
    public Vector2 rotationLimitsX = new Vector2(-70f, 70f);
    public Vector2 rotationLimitsY = new Vector2(-70f, 70f);

    private Vector3 currentRotation;

    void Start()
    {
        currentRotation = transform.localEulerAngles;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get the mouse input for rotation
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float vertical = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Update the current rotation
        currentRotation.y += horizontal;
        currentRotation.x += vertical;

        // Clamp the rotation between -60 and 60 degrees for both axes
        currentRotation.x = Mathf.Clamp(currentRotation.x, rotationLimitsX.x, rotationLimitsX.y);
        currentRotation.y = Mathf.Clamp(currentRotation.y, rotationLimitsY.x, rotationLimitsY.y);

        // Apply the clamped rotation to the transform
        transform.localEulerAngles = currentRotation;
    }
}
