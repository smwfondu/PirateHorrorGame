using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    public float floatSpeed = 1.0f; // Speed of the floating effect
    public float floatMagnitude = 10.0f; // Amplitude of the floating effect
    public float phaseOffset = 0.0f;
    public float rotationSpeed = 10.0f; // Speed of the rotation
    public float maxRotationAngle = 5.0f; // Maximum angle for rotation

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition; // Store the initial position of the UI element
    }

    void Update()
    {
        // Floating effect
        float offsetY = Mathf.Sin(Time.unscaledTime * floatSpeed + phaseOffset) * floatMagnitude;
        float offsetX = Mathf.Cos(Time.unscaledTime * floatSpeed + phaseOffset * 0.5f) * (floatMagnitude / 2);
        transform.localPosition = initialPosition + new Vector3(offsetX, offsetY, 0);

        // Rotational effect
        float rotationAngle = Mathf.Sin(Time.unscaledTime * rotationSpeed) * maxRotationAngle;
        transform.localRotation = Quaternion.Euler(0, 0, rotationAngle);
    }
}
