using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private DayNightCycle dayNightCycle;
    [SerializeField] private TextMeshProUGUI interactStatusText;
    [SerializeField] private GameObject taskList;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private FishingStarter fishingStarter;

    private GameObject parrot;
    private FishInteractable currentFish;

    [Header("Player States")]
    public bool grabbedList = false;
    public bool holdingFish = false;
    public bool inFishingCollider = false;

    private void Start()
    {
        taskList.SetActive(false);
        parrot = GameObject.FindGameObjectWithTag("Parrot");
    }

    private void Update()
    {
        // Drop the fish if holding one
        if (holdingFish && Input.GetKeyDown(KeyCode.E))
        {
            DropCurrentFish();
            return; // Exit early to avoid conflicting actions
        }

        // Only handle fishing if inside a fishing collider
        if (inFishingCollider)
        {
            HandleFishingInteraction();
        }
        else
        {
            HandleInteraction();
        } 
    }

    // Handles player interactions with objects
    private void HandleInteraction()
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
            else if (inFishingCollider)
            {
                HandleFishingInteraction();
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
        interactStatusText.text = "Press E to sleep";
        if (Input.GetKey(KeyCode.E))
        {
            if (dayNightCycle.GetNightStatus())
            {
                SceneManager.LoadScene(0);
                interactStatusText.text = "Going to sleep!";
            }
            else
            {
                interactStatusText.text = "Can't sleep now, too bright outside!";
            }
        }
    }

    // Handles interaction with the task list
    private void HandleTaskListInteraction(RaycastHit hit)
    {
        interactStatusText.text = "Press E to grab";
        if (Input.GetKeyDown(KeyCode.E))
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
        if (!holdingFish)
        {
            interactStatusText.text = "Press E to grab fish";
            if (Input.GetKeyDown(KeyCode.E))
            {
                FishInteractable fish = hit.collider.GetComponent<FishInteractable>();
                if (fish != null)
                {
                    fish.PickUpFish(); // Pick up the fish
                    holdingFish = true;
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
            holdingFish = false;
            currentFish = null; // Reset the reference to the current fish
        }
    }

    // Handles interaction with fishing spots
    private void HandleFishingInteraction()
    {
        if (!fishingStarter.inMinigame)
        {
            interactStatusText.text = "Press E to fish";
            if (Input.GetKeyDown(KeyCode.E))
            {
                fishingStarter.StartMinigame();
            }
        }
        else
        {
            interactStatusText.text = "";
        }
    }
}
