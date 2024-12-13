using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CannonPolish : MonoBehaviour
{
    public Image hookHand;
    public PlayerInteract pi;
    public float polishTime = 3f;
    public TextMeshProUGUI cannonTaskText;

    private float polishProgress = 0f;
    private bool isPolishing = false;
    private static int cannonsPolished = 0;
    private int targetCannonsPolished = 5;
    private bool isPolished = false;

    private Renderer cannonRenderer;
    private Color originalColor;
    private Color darkColor;
    private Color hookColor;
    private ParticleSystem ps;

    private void Start()
    {
        hookColor = hookHand.color;
        hookColor.a = 0f;  // Set initial alpha to 0 (invisible)
        hookHand.color = hookColor;

        cannonRenderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();

        originalColor = cannonRenderer.material.color;
        darkColor = originalColor * 0.3f;
        cannonRenderer.material.color = darkColor;
        cannonsPolished = 0;

        cannonTaskText.text = "Clean Cannons: " + cannonsPolished + "/" + targetCannonsPolished;
    }

    void Update()
    {
        if (isPolished) return;  // Skip if this cannon is already polished

        // Check if the player is looking at this cannon and holding "E"
        if (IsPlayerLookingAtCannon() && Input.GetKey(KeyCode.E) && pi.grabbedList)
        {
            if (!isPolishing)
            {
                // Start polishing, make hookHand visible
                hookColor.a = 1f;
                hookHand.color = hookColor;
                isPolishing = true;
            }

            polishProgress += Time.deltaTime / polishTime;  // Increase progress over time

            // Lerp cannon color from dark to original as it’s polished
            cannonRenderer.material.color = Color.Lerp(darkColor, originalColor, polishProgress);

            // Check if polishing is complete
            if (polishProgress >= 1f)
            {
                isPolishing = false;
                isPolished = true;
                cannonsPolished++;

                ps.Play();
                cannonTaskText.text = "Clean Cannons: " + cannonsPolished + "/" + targetCannonsPolished;

                // Hide the hookHand once polishing is complete
                hookColor.a = 0f;
                hookHand.color = hookColor;

                // Check if all cannons are polished
                if (cannonsPolished == targetCannonsPolished)
                {
                    cannonTaskText.text = "Clean Cannons finished!";
                }
            }
        }
        else if (isPolishing)
        {
            // Stop polishing and hide the hookHand
            isPolishing = false;
            hookColor.a = 0f;
            hookHand.color = hookColor;
        }
    }

    // Method to check if the player is looking at this cannon
    private bool IsPlayerLookingAtCannon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))  // Adjust distance as needed
        {
            return hit.collider.gameObject == gameObject && hit.collider.CompareTag("Cannon");
        }
        return false;
    }

    // Optional: Reset all progress when a new day starts (example function)
    public static void ResetPolishingProgress()
    {
        cannonsPolished = 0;
    }
}
