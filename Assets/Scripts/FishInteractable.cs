using UnityEngine;

public class FishInteractable : MonoBehaviour
{
    private Transform player;
    private Transform playerCamera;
    private Transform itemHolder;
    private bool isCarried = false;

    public float throwForce = 10f;

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        itemHolder = GameObject.FindGameObjectWithTag("ItemHolder").transform;
    }

    public void DropFish()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = playerCamera.forward * throwForce;
        player.GetComponent<PlayerInteract>().SetPlayerState(new(player.GetComponent<PlayerInteract>().GetPlayerState().x, 1));
        transform.parent = null; // Unparent the fish
        isCarried = false;
    }

    public void PickUpFish()
    {
        Debug.Log("Pickup Fish Called!");
        // Pick up the fish
        if (!isCarried)
        {
            player = GameObject.FindWithTag("Player").transform; // Ensure your player has the "Player" tag
            transform.parent = itemHolder; // Parent the fish to the player
            transform.localPosition = new Vector3(0, -0.5f, 1); // Adjust the carrying position
            transform.localRotation = Quaternion.Euler(0, -90, 0);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true; // Disable physics while carrying
            isCarried = true;
        }
    }
}