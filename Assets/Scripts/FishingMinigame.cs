using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    public Slider fishingBar;
    public float barIncreaseSpeed = 0.5f; // How fast the bar moves left
    public float clickDecreaseeAmount = 0.1f; // How much each click pushes the bar right
    public GameObject fishPrefab; // The fish to spawn when caught

    public GameObject fishingCollider; // The fish to spawn when caught

    private GameObject[] fishSpawnPoint; // Where the fish spawns
    private bool isFishing = true;

    private void Start()
    {
        fishSpawnPoint = GameObject.FindGameObjectsWithTag("FishSpawnPoint");
        fishingCollider = GameObject.FindGameObjectWithTag("FishingCollider");
    }

    void Update()
    {
        if (isFishing)
        {
            // Decrease the bar over time
            fishingBar.value += barIncreaseSpeed * Time.deltaTime;

            // Check for mouse clicks
            if (Input.GetKeyDown(KeyCode.E))
            {
                fishingBar.value -= clickDecreaseeAmount;
            }

            // Check if the player wins or loses
            if (fishingBar.value >= fishingBar.maxValue)
            {
                EndMinigame(false);
            }
            else if (fishingBar.value <= fishingBar.minValue)
            {
                CatchFish();
            }
        }
    }

    void CatchFish()
    {
        isFishing = false;

        int pointIndex = Random.Range(0, fishSpawnPoint.Length);

        isFishing = false;

        // Instantiate the fish at the spawn point
        GameObject fish = Instantiate(fishPrefab, fishSpawnPoint[pointIndex].transform.position, fishSpawnPoint[pointIndex].transform.rotation);

        // Randomize fish size
        fish.transform.localScale = new Vector3(Random.Range(60, 80f), Random.Range(60, 80f), Random.Range(80, 100f));

        // Launch the fish in the direction of the spawn point
        FishBehavior fishBehavior = fish.GetComponent<FishBehavior>();
        fishBehavior.LaunchFish(fishSpawnPoint[pointIndex].transform.up);

        EndMinigame(true);
    }

    void EndMinigame(bool success)
    {
        // Optionally play animations, sounds, or handle success/failure
        fishingCollider.GetComponent<FishingStarter>().MinigameDone();
        Destroy(gameObject); // Destroy the minigame UI
    }
}
