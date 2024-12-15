using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] fishingShootSpots;  // Array of fishing spots
    [SerializeField] private GameObject fishPrefab;  // Fish prefab to instantiate
    [SerializeField] private float minSpawnTime = 10f;  // Minimum spawn time
    [SerializeField] private float maxSpawnTime = 20f;  // Maximum spawn time

    private void Start()
    {
        // Start the spawning process
        StartCoroutine(SpawnFish());
    }

    private IEnumerator SpawnFish()
    {
        while (true)
        {
            // Wait for a random time between min and max spawn time
            float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(spawnTime);

            // Choose a random fishing spot
            Transform randomSpot = fishingShootSpots[Random.Range(0, fishingShootSpots.Length)];

            // Create a random rotation
            Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

            // Instantiate the fish at the random spot
            GameObject fish = Instantiate(fishPrefab, randomSpot.position, randomRotation);
            fish.GetComponent<FishBehavior>().LaunchFish(randomSpot.up);
        }
    }
}
