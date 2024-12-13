using UnityEngine;

public class FishingStarter : MonoBehaviour
{
    public GameObject fishingMinigamePrefab;

    public bool inMinigame = false;

    public void StartMinigame()
    {
        // Instantiate the fishing minigame at a designated UI location
        inMinigame = true;
        Instantiate(fishingMinigamePrefab, GameObject.FindGameObjectWithTag("MainCanvas").transform);
    }

    public void MinigameDone()
    {
        // Instantiate the fishing minigame at a designated UI location
        inMinigame = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteract player = other.GetComponent<PlayerInteract>();
            if (player != null)
            {
                player.inFishingCollider = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteract player = other.GetComponent<PlayerInteract>();
            if (player != null)
            {
                player.inFishingCollider = false;
            }
        }
    }
}
