using UnityEngine;

public class CannonPolish : MonoBehaviour
{
    public float polishTime = 3f;
    public AudioClip scrapSound;
    public GameObject psminiPrefab;
    [SerializeField] private CannonTaskManager cannonTaskManager;

    private AudioSource audioSource;
    private Renderer cannonRenderer;
    private ParticleSystem ps;
    private Color originalColor;
    private Color darkColor;

    private float polishProgress = 0f;
    private bool isPolishing = false;
    private bool isPolished = false;

    private float particleTimer = 0f;
    private float firstEffectTime = 0.5f;
    private float subsequentEffectTime = 0.7f;
    private bool isFirstParticle = true;

    private void Start()
    {
        cannonRenderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        originalColor = cannonRenderer.material.color;
        darkColor = originalColor * 0.3f;
        cannonRenderer.material.color = darkColor;
    }

    void Update()
    {
        if (isPolished) return;
        if (!cannonTaskManager.GetTaskStarted()) return;

        if (IsPlayerLookingAtCannon() && Input.GetMouseButton(0))
        {
            if (!isPolishing) isPolishing = true;

            polishProgress += Time.deltaTime / polishTime;
            cannonRenderer.material.color = Color.Lerp(darkColor, originalColor, polishProgress);

            if (polishProgress >= 1f)
            {
                isPolished = true;
                ps.Play();
                cannonTaskManager.RegisterPolishedCannon();
            }
        }
        else if (isPolishing)
        {
            isPolishing = false;
        }
    }

    void FixedUpdate()
    {
        if (!cannonTaskManager.GetTaskStarted()) return;

        if (IsPlayerLookingAtCannon() && Input.GetMouseButton(0) && !isPolished)
        {
            particleTimer += Time.deltaTime;

            if (isFirstParticle && particleTimer >= firstEffectTime)
            {
                SpawnParticleEffect();
                particleTimer = 0f;
                isFirstParticle = false;
            }
            else if (!isFirstParticle && particleTimer >= subsequentEffectTime)
            {
                SpawnParticleEffect();
                particleTimer = 0f;
            }
        }
    }

    private bool IsPlayerLookingAtCannon()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            return hit.collider.gameObject == gameObject && hit.collider.CompareTag("Cannon");
        }
        return false;
    }

    private void SpawnParticleEffect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            if (hit.collider.gameObject == gameObject && hit.collider.CompareTag("Cannon"))
            {
                Instantiate(psminiPrefab, hit.point, Quaternion.identity);
            }
        }
    }
}
