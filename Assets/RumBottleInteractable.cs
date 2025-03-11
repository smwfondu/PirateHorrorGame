using UnityEngine;

public class RumBottleInteractable : MonoBehaviour
{
    private Transform player;
    private Transform playerCamera;
    private Transform itemHolder;
    private bool isCarried = false;

    public float throwForce = 10f;
    private bool isInteractable = false; // Controlled by the task manager

    private void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        itemHolder = GameObject.FindGameObjectWithTag("ItemHolder").transform;
    }

    public void SetInteractable(bool state)
    {
        isInteractable = state;
    }

    public void DropBarrel()
    {
        Debug.Log(isCarried);
        if (!isCarried) return;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = playerCamera.forward * throwForce;
        player.GetComponent<PlayerInteract>().SetPlayerState(new(player.GetComponent<PlayerInteract>().GetPlayerState().x, 1));
        transform.parent = null; // Unparent the barrel
        isCarried = false;
    }

    public void PickUpBarrel()
    {
        Debug.Log(isInteractable);
        Debug.Log(isCarried);
        if (!isInteractable || isCarried) return;

        Debug.Log("Picked up rum barrel!");
        player = GameObject.FindWithTag("Player").transform; // Ensure your player has the "Player" tag
        transform.parent = itemHolder; // Parent to the player
        transform.localPosition = new Vector3(0, -0.5f, 1); // Adjust carrying position
        transform.localRotation = Quaternion.Euler(0, -90, 0);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Disable physics while carrying
        isCarried = true;
    }
}
