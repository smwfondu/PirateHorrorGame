using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private TextMeshProUGUI interactStatusText;
    [SerializeField] private GameObject taskList;
    [SerializeField] private Material mobileMaterial;

    private GameObject parrot;
    private FishInteractable currentFish;

    [Header("Player States")]
    private Vector2 currentPlayerState = new(1, 1);
    public bool grabbedList = false;

    [Header("Telescope")]
    public GameObject playerCamera; // Assign your camera in the inspector
    public Image binocularOverlay; // Assign your binocular image in the inspector

    private float targetFOV;
    private float zoomSpeed = 5f;
    public float targetScale = 3f; // The starting scale of the overlay
    private float minScale = 1.0f;    // The zoomed-in scale of the overlay

    private void Start()
    {
        interactStatusText.material = mobileMaterial;
        taskList.SetActive(false);
        parrot = GameObject.FindGameObjectWithTag("Parrot");

        //For telescope
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
        // Check for state change inputs
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentPlayerState.x = 1; // Set state to handle hook interaction
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentPlayerState.x = 2; // Set state to handle telescope interaction
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentPlayerState.x = 3; // Set state to handle lantern interaction
        }

        // Drop the fish if holding one
        if (currentPlayerState.y == 2 && Input.GetMouseButtonDown(0))
        {
            DropCurrentFish();
            return; // Exit early to avoid conflicting actions
        }

        // Handle interactions based on current state
        switch ((int)currentPlayerState.x)
        {
            case 1:
                HandleHookInteraction();
                break;
            case 2:
                HandleTelescopeInteraction();
                break;
            case 3:
                HandleLanternInteraction();
                break;
        }
    }


    // Handles player interactions with objects
    private void HandleHookInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 15f))
        {
            float distance = Vector3.Distance(transform.position, hit.point);

            if (distance <= 3f) // Close-range interactions
            {
                if (hit.collider.CompareTag("Bed"))
                {
                    HandleBedInteraction();
                }
                else if (hit.collider.CompareTag("TaskList"))
                {
                    HandleTaskListInteraction(hit);
                }
                else if (hit.collider.CompareTag("Fish"))
                {
                    HandleFishInteraction(hit);
                }
                else
                {
                    interactStatusText.text = "";
                }
            }
            else
            {
                interactStatusText.text = "";
            }
        }
        else
        {
            interactStatusText.text = "";
        }
    }

    // Handles interaction with the bed
    private void HandleBedInteraction()
    {
        if (dayNightCycle.GetNightStatus())
        {
            interactStatusText.text = "Left click to sleep!";
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            interactStatusText.text = "Can't sleep now, too bright outside!";
        }
    }

    // Handles interaction with the task list
    private void HandleTaskListInteraction(RaycastHit hit)
    {
        interactStatusText.text = "Left click to grab";
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(hit.collider.gameObject);
            taskList.SetActive(true);
            if (parrot != null)
            {
                Animator parrotAnimator = parrot.GetComponent<Animator>();
                if (parrotAnimator != null)
                {
                    parrotAnimator.SetTrigger("flyaway");
                }
            }
            grabbedList = true;
        }
    }

    // Handles interaction with a fish
    private void HandleFishInteraction(RaycastHit hit)
    {
        if (currentPlayerState.y != 2)
        {
            interactStatusText.text = "Left click to Stab fish";
            if (Input.GetMouseButtonDown(0))
            {
                FishInteractable fish = hit.collider.GetComponent<FishInteractable>();
                if (fish != null)
                {
                    fish.PickUpFish(); // Pick up the fish
                    currentPlayerState = new(currentPlayerState.x, 2);
                    currentFish = fish; // Assign the fish to the currentFish variable
                }
            }
        }
        else
        {
            interactStatusText.text = "";
        }
    }

    public void DropCurrentFish()
    {
        if (currentFish != null)
        {
            currentFish.DropFish(); // Call the fish's drop method
            currentPlayerState = new(currentPlayerState.x, 1);
            currentFish = null; // Reset the reference to the current fish
        }
    }

    // Handle telescope interaction
    private void HandleTelescopeInteraction()
    {
        interactStatusText.text = "Using telescope...";

        HandleZoom();
    }

    private void HandleZoom()
    {
        Camera cameraComponent = playerCamera.GetComponent<Camera>();
        if (cameraComponent != null)
        {
            // Set target FOV and overlay scale based on input
            if (Input.GetMouseButton(0))
            {
                targetFOV = 30f; // Zoomed-in FOV
                targetScale = minScale; // Zoomed-in scale
            }
            else
            {
                targetFOV = 70f; // Default FOV
                targetScale = 4f; // Default scale
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

    // Handle lantern interaction
    private void HandleLanternInteraction()
    {
        interactStatusText.text = "Using lantern...";
    }

    public Vector2 GetPlayerState()
    {
        return currentPlayerState;
    }

    public void SetPlayerState(Vector2 newPlayerState)
    {
        currentPlayerState = newPlayerState;
    }
}
