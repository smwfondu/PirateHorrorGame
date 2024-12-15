using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CannonPolish : MonoBehaviour
{
    public PlayerInteract pi;
    public float polishTime = 3f;
    public TextMeshProUGUI cannonTaskText;

    public AudioClip scrapSound;  // Reference to the scrap sound clip
    private AudioSource audioSource;  // AudioSource to play the sound

    private float polishProgress = 0f;
    private bool isPolishing = false;
    private static int cannonsPolished = 0;
    private int targetCannonsPolished = 5;
    private bool isPolished = false;

    private Renderer cannonRenderer;
    private Color originalColor;
    private Color darkColor;
    private ParticleSystem ps;

    // New particle effect prefab
    public GameObject psminiPrefab;  // Particle system prefab to instantiate
    private float particleTimer = 0f;
    private float firstEffectTime = 0.5f;  // Time for first effect
    private float subsequentEffectTime = 0.7f;  // Time for subsequent effects
    private bool isFirstParticle = true;  // Flag to track the first particle

    private void Start()
    {
        cannonRenderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

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
        if (IsPlayerLookingAtCannon() && Input.GetMouseButton(0) && pi.grabbedList)
        {
            if (!isPolishing)
            {
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
        }
    }

    // In FixedUpdate()
    void FixedUpdate()
    {
        // Check if the player is looking at this cannon and holding left mouse
        if (IsPlayerLookingAtCannon() && Input.GetMouseButton(0) && pi.grabbedList && !isPolished)
        {
            // Particle effect first after 0.5 seconds, then every 1 second while left-click is held down
            particleTimer += Time.deltaTime;

            if (isFirstParticle && particleTimer >= firstEffectTime)
            {
                // Play the scrap sound effect
                //PlayScrapSound();

                // Instantiate the first particle effect after 0.5 seconds
                SpawnParticleEffect();
                particleTimer = 0f;  // Reset the timer
                isFirstParticle = false;  // Disable the first particle flag
            }
            else if (!isFirstParticle && particleTimer >= subsequentEffectTime)
            {
                // Play the scrap sound effect
                //PlayScrapSound();

                // Instantiate subsequent particle effects every 1 second
                SpawnParticleEffect();
                particleTimer = 0f;  // Reset the timer
            }
        }
    }

    // Method to play the scrap sound
    private void PlayScrapSound()
    {
        if (scrapSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.PlayOneShot(scrapSound);  // Play the scrap sound
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

    // Method to spawn the particle effect at the hit point on the cannon
    private void SpawnParticleEffect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))  // Adjust distance as needed
        {
            if (hit.collider.gameObject == gameObject && hit.collider.CompareTag("Cannon"))
            {
                // Instantiate the particle system prefab at the hit point
                Instantiate(psminiPrefab, hit.point, Quaternion.identity);
            }
        }
    }

    // Optional: Reset all progress when a new day starts (example function)
    public static void ResetPolishingProgress()
    {
        cannonsPolished = 0;
    }
}
