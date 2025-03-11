using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshProUGUI interactStatusText;
    [SerializeField] private Material mobileMaterial;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private Image binocularOverlay;
    [SerializeField] private TaskPaperManager playerTaskPaper;

    private GameObject parrot;
    private FishInteractable currentFish;
    private RumBottleInteractable currentRumBottle;

    [Header("Player States")]
    private Vector2 currentPlayerState = new(1, 1);

    [Header("Telescope Settings")]
    private float targetFOV;
    private float zoomSpeed = 5f;
    private float targetScale = 3f;
    private float minScale = 1.0f;
    private bool isZoomingOut = false;

    private void Start()
    {
        InitializeComponents();
    }

    private void Update()
    {
        HandleStateChange();
        HandleInteractions();
        if (isZoomingOut) SmoothZoomOut();
    }

    // Initialize scene components
    private void InitializeComponents()
    {
        interactStatusText.material = mobileMaterial;
        parrot = GameObject.FindGameObjectWithTag("Parrot");

        Camera cameraComponent = playerCamera.GetComponent<Camera>();
        if (cameraComponent != null) targetFOV = cameraComponent.fieldOfView;
        if (binocularOverlay != null)
            binocularOverlay.rectTransform.localScale = new Vector3(targetScale, targetScale, 1f);
    }

    // Handle changing the player's interaction state
    private void HandleStateChange()
    {
        int previousState = (int)currentPlayerState.x;
        int newState = previousState;

        if (Input.GetKeyDown(KeyCode.Alpha1)) newState = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) newState = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) newState = 3;

        // Only update if state actually changes
        if (previousState != newState)
        {
            currentPlayerState.x = newState;

            // If transitioning away from telescope state, trigger smooth zoom out
            if (previousState == 2 && newState != 2)
                isZoomingOut = true;

            if (previousState == 1 && newState != 1 && currentPlayerState.y == 2)
                DropCurrentFish();

            if (previousState == 1 && newState != 1 && currentPlayerState.y == 3)
                DropCurrentRum();

            // Trigger animations based on the new state
            TriggerStateAnimations(newState);
        }
    }

    // Function to handle animation triggers when state changes
    private void TriggerStateAnimations(int state)
    {
        Animator animator = GetComponent<Animator>();
        if (animator == null) return;

        switch (state)
        {
            case 1: animator.SetTrigger("EquipHook"); break;
            case 2: animator.SetTrigger("EquipTelescope"); break;
            case 3: animator.SetTrigger("EquipLantern"); break;
        }
    }

    // Handle different interactions based on the player's current state
    private void HandleInteractions()
    {
        if (currentPlayerState.y == 2 && Input.GetMouseButtonDown(0))
        {
            DropCurrentFish();
            return;
        } 
        else if (currentPlayerState.y == 3 && Input.GetMouseButtonDown(0))
        {
            Debug.Log("Drop Rum Called!");
            DropCurrentRum();
            return;
        }

        switch ((int)currentPlayerState.x)
        {
            case 1: HandleHookInteraction(); break;
            case 2: HandleTelescopeInteraction(); isZoomingOut = false; break;
            case 3: HandleLanternInteraction(); break;
        }
    }

    // Handles raycast-based interactions
    private void HandleHookInteraction()
    {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 15f))
        {
            interactStatusText.text = "";
            return;
        }

        float distance = Vector3.Distance(transform.position, hit.point);
        if (distance > 3f)
        {
            interactStatusText.text = "";
            return;
        }

        switch (hit.collider.tag)
        {
            case "TaskList": HandleTaskListInteraction(hit); break;
            case "Fish": HandleFishInteraction(hit); break;
            case "RumBottle": HandleRumInteraction(hit); break;
            default: interactStatusText.text = ""; break;
        }
    }

    // Handles interaction with the task list
    private void HandleTaskListInteraction(RaycastHit hit)
    {
        interactStatusText.text = "Left click to grab";
        if (!Input.GetMouseButtonDown(0)) return;

        Destroy(hit.collider.gameObject);
        playerTaskPaper.SetGrabbedList(true);

        if (parrot != null) parrot.GetComponent<Animator>()?.SetTrigger("flyaway");
    }

    // Handles interaction with a fish
    private void HandleRumInteraction(RaycastHit hit)
    {
        if (currentPlayerState.y == 3 || currentPlayerState.y == 2)
        {
            interactStatusText.text = "";
            return;
        }

        interactStatusText.text = "Left click to Grab Rum";
        if (!Input.GetMouseButtonDown(0)) return;

        currentRumBottle = hit.collider.GetComponent<RumBottleInteractable>();
        if (currentRumBottle == null) return;

        currentRumBottle.PickUpBarrel();
        currentPlayerState.y = 3;
    }

    // Handles interaction with a fish
    private void HandleFishInteraction(RaycastHit hit)
    {
        if (currentPlayerState.y == 3 || currentPlayerState.y == 2)
        {
            interactStatusText.text = "";
            return;
        }

        interactStatusText.text = "Left click to Stab fish";
        if (!Input.GetMouseButtonDown(0)) return;

        currentFish = hit.collider.GetComponent<FishInteractable>();
        if (currentFish == null) return;

        currentFish.PickUpFish();
        currentPlayerState.y = 2;
    }

    // Drop the currently held fish
    public void DropCurrentFish()
    {
        if (currentFish == null) return;

        currentFish.DropFish();
        currentPlayerState.y = 1;
        currentFish = null;
    }

    // Drop the currently held fish
    public void DropCurrentRum()
    {
        if (currentRumBottle == null) return;

        currentRumBottle.DropBarrel();
        currentPlayerState.y = 1;
        currentRumBottle = null;
    }

    // Handle telescope interaction
    private void HandleTelescopeInteraction()
    {
        interactStatusText.text = "Using telescope...";
        HandleZoom();
    }

    // Handle zooming in and out smoothly
    private void HandleZoom()
    {
        Camera cameraComponent = playerCamera.GetComponent<Camera>();
        if (cameraComponent == null) return;

        targetFOV = Input.GetMouseButton(0) ? 30f : 80f;
        targetScale = Input.GetMouseButton(0) ? minScale : 4f;

        cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        if (binocularOverlay != null)
        {
            float currentScale = Mathf.Lerp(binocularOverlay.rectTransform.localScale.x, targetScale, Time.deltaTime * zoomSpeed);
            binocularOverlay.rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);
        }
    }

    // Smoothly transition out of the zoomed-in telescope view
    private void SmoothZoomOut()
    {
        Camera cameraComponent = playerCamera.GetComponent<Camera>();
        if (cameraComponent == null) return;

        cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, 80f, Time.deltaTime * zoomSpeed);
        if (binocularOverlay == null) return;

        float currentScale = Mathf.Lerp(binocularOverlay.rectTransform.localScale.x, 4f, Time.deltaTime * (zoomSpeed / 10));
        binocularOverlay.rectTransform.localScale = new Vector3(currentScale, currentScale, 1f);

        if (Mathf.Abs(cameraComponent.fieldOfView - 80f) < 0.1f && Mathf.Abs(currentScale - 4f) < 0.1f)
            isZoomingOut = false;
    }

    // Handle lantern interaction
    private void HandleLanternInteraction()
    {
        interactStatusText.text = "Using lantern...";
    }

    // Getters & Setters for player state
    public Vector2 GetPlayerState() => currentPlayerState;
    public bool GetIsZoomingOut() => isZoomingOut;
    public void SetPlayerState(Vector2 newPlayerState) => currentPlayerState = newPlayerState;
}
