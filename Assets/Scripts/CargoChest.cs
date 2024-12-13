using UnityEngine;
using TMPro;

public class CargoChest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fishingTaskText;
    [SerializeField] private GameObject[] caughtFishModels;
    [SerializeField] private PlayerInteract pi;

    private int fishCount = 2;
    public int requiredFish = 5;

    private bool taskDone = false;

    private void Update()
    {
        if(!taskDone) { fishingTaskText.text = "Catch Fish: " + fishCount + "/" + requiredFish; }
        else { fishingTaskText.text = "Fishing Done!"; }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish") && fishCount < requiredFish)
        {
            if (pi.holdingFish == true) { pi.DropCurrentFish(); }

            // Increment the fish count and destroy the fish
            fishCount++;
            caughtFishModels[fishCount - 3].gameObject.SetActive(true);
            Destroy(other.gameObject);            

            // Check if task is complete
            if (fishCount >= requiredFish)
            {
                CompleteTask();
            }
        }
    }

    void CompleteTask()
    {
        taskDone = true;
    }
}
