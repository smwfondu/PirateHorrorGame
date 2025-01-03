using UnityEngine;

public class PauseMenuAnimator : MonoBehaviour
{
    public Animator eyelidAnimator; // Reference to the Animator for eyelid fading
    public string closeEyesAnimationName = "CloseEyes"; // Name of the closing eyes animation
    public float menuFadeDuration = 0.5f; // Time it takes for the menu to fade in/out

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause menu when pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Trigger the eyelid closing animation
        eyelidAnimator.SetTrigger("CloseEyes");

        // Wait for the animation to complete before freezing the game
        StartCoroutine(FreezeGameAfterAnimation());
    }

    public void Resume()
    {
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Trigger the eyelid opening animation
        eyelidAnimator.SetTrigger("OpenEyes");

        Time.timeScale = 1f;
    }

    private System.Collections.IEnumerator FreezeGameAfterAnimation()
    {
        // Get the AnimatorStateInfo for the current animation
        AnimatorStateInfo animationState = eyelidAnimator.GetCurrentAnimatorStateInfo(0);

        // Ensure the correct animation is playing and wait for its duration
        while (!animationState.IsName(closeEyesAnimationName))
        {
            animationState = eyelidAnimator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        yield return new WaitForSeconds(animationState.length);

        Time.timeScale = 0f; // Freeze the game
    }
}
