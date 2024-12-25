using UnityEngine;

public class HookHandController : MonoBehaviour
{
    private Animator animator;
    private PlayerInteract pi;
    private int currentToolState = 1;

    [Header("Tool Attachments")]
    [SerializeField] private GameObject telescope;   // Hook with telescope attached
    [SerializeField] private GameObject lantern;     // Hook with lantern attached

    void Start()
    {
        animator = GetComponent<Animator>();
        pi = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteract>();

        // Ensure only the regular hook is enabled initially
        EnableToolAttachment(1);
    }

    void Update()
    {
        // Handle player state transitions
        HandleStateTransitions();

        if (pi.GetPlayerState().x == 1)
        {
            HandleSwinging();
        }
        
        HandleInspecting();
    }

    void HandleSwinging()
    {
        // If left mouse button is pressed or held, trigger swinging
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            animator.SetBool("isSwinging", true);
            // Cancel inspecting if swinging starts
            animator.SetBool("isInspecting", false);
        }
        else
        {
            animator.SetBool("isSwinging", false);
        }
    }

    void HandleInspecting()
    {
        // Only allow inspecting if not currently swinging
        if (Input.GetKeyDown(KeyCode.F) && !animator.GetBool("isSwinging"))
        {
            animator.SetBool("isInspecting", true);
        }
        else if (!Input.GetKey(KeyCode.F))
        {
            animator.SetBool("isInspecting", false);
        }
    }

    void HandleStateTransitions()
    {
        // Get the current state from PlayerInteract
        int newState = (int)pi.GetPlayerState().x;

        // Check if the state has changed
        if (newState != currentToolState)
        {
            currentToolState = newState;
            StartCoroutine(PlayTransitionAnimation(newState));
        }
    }

    private System.Collections.IEnumerator PlayTransitionAnimation(int newState)
    {
        // Play the "HookOut" animation
        animator.SetTrigger("PlayHookOut");

        // Wait for the "out" animation to finish
        yield return new WaitForSeconds(0.5f);

        // Enable the correct tool attachment
        EnableToolAttachment(newState);

        // Play the "HookIn" animation
        animator.SetTrigger("PlayHookIn");
    }

    private void EnableToolAttachment(int state)
    {
        telescope.SetActive(false);
        lantern.SetActive(false);

        // Enable the appropriate tool attachment based on the state
        switch (state)
        {
            case 1: // Regular hook
                break;
            case 2: // Telescope
                telescope.SetActive(true);
                break;
            case 3: // Lantern
                lantern.SetActive(true);
                break;
        }
    }
}
