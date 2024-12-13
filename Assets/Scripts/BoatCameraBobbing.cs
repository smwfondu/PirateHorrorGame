using UnityEngine;

public class BoatCameraBobbing : MonoBehaviour
{
    // Frequency and amplitude of the bobbing effect
    public float bobFrequency = 1.0f; // How fast the bobbing happens
    public float bobAmplitude = 0.5f; // How far the camera moves up and down
    public float swayAmplitude = 0.2f; // How far the camera sways side-to-side

    private Vector3 initialPosition;

    void Start()
    {
        // Save the initial position of the camera holder
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Calculate bobbing using a sine wave for vertical movement
        float bobbingOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;

        // Calculate swaying using a cosine wave for horizontal movement
        float swayingOffset = Mathf.Cos(Time.time * bobFrequency * 0.5f) * swayAmplitude;

        // Apply the bobbing and swaying to the local position of the camera holder
        transform.localPosition = initialPosition + new Vector3(swayingOffset, bobbingOffset, 0);
    }
}
