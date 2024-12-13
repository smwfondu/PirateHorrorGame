using UnityEngine;
using UnityEngine.UI; // For UI components

public class BinocularZoom : MonoBehaviour
{
    public GameObject playerCamera; // Assign your camera in the inspector
    public Image binocularOverlay; // Assign your binocular image in the inspector

    private float targetFOV;
    private float zoomSpeed = 5f;
    private float targetScale = 2.5f; // The starting scale of the overlay
    private float minScale = 1.0f;    // The zoomed-in scale of the overlay

    private void Start()
    {
        // Set the initial FOV to match the camera's current FOV
        Camera cameraComponent = playerCamera.GetComponent<Camera>();
        if (cameraComponent != null)
        {
            targetFOV = cameraComponent.fieldOfView;
        }

        // Initialize the scale of the binocular overlay
        if (binocularOverlay != null)
        {
            binocularOverlay.rectTransform.localScale = new Vector3(targetScale, targetScale, 1f);
        }
    }

    private void Update()
    {
        HandleZoom();
    }

    private void HandleZoom()
    {
        Camera cameraComponent = playerCamera.GetComponent<Camera>();
        if (cameraComponent != null)
        {
            // Set target FOV and overlay scale based on input
            if (Input.GetKey(KeyCode.Z))
            {
                targetFOV = 30f; // Zoomed-in FOV
                targetScale = minScale; // Zoomed-in scale
            }
            else
            {
                targetFOV = 70f; // Default FOV
                targetScale = 2.5f; // Default scale
            }

            // Smoothly transition to the target FOV
            cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);

            // Smoothly transition the binocular overlay scale
            if (binocularOverlay != null)
            {
                float currentScale = Mathf.Lerp(binocularOverlay.rectTransform.localScale.x, targetScale, Time.deltaTime * zoomSpeed);
                binocularOverlay.rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);
            }
        }
    }
}
