using System.Collections;
using UnityEngine;

public class ParrotController : MonoBehaviour
{
    public Animator animator;    // Reference to the Animator component
    private string triggerName = "flytowards";   // Name of the trigger to activate
    public float delay = 30f;    // Delay in seconds before triggering the animation

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        // Start the delayed trigger coroutine
        StartCoroutine(TriggerAfterDelay());
    }

    private IEnumerator TriggerAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Set the trigger on the Animator
        animator.SetTrigger(triggerName);
    }
}
