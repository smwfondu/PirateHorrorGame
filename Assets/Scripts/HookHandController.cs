using UnityEngine;

public class HookHandController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleSwinging();
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
}
