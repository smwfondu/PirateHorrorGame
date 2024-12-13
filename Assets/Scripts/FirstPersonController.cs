using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public Transform spawnPoint;

    public float moveSpeed = 5.0f;         // Movement speed
    public float lookSpeed = 2.0f;         // Mouse sensitivity
    public float maxLookX = 60f;           // Max look up angle
    public float minLookX = -60f;          // Max look down angle
    public float gravity = 9.8f;           // Gravity force
    public float stepOffset = 0.5f;        // Step height for stair stepping

    private float rotX = 0;                // X rotation of the camera (vertical)
    private CharacterController controller;
    private Camera playerCamera;
    private Vector3 moveDirection;

    private void Start()
    {
        gameObject.transform.position = spawnPoint.transform.position;
        
        // Get references to the components
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        // Set step offset in the CharacterController
        controller.stepOffset = stepOffset;

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Move();
        CameraLook();
    }

    private void Move()
    {
        // Get input for movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate direction relative to player orientation
        Vector3 moveDir = transform.right * x + transform.forward * z;

        // Apply movement with gravity
        if (controller.isGrounded)
        {
            moveDirection = moveDir * moveSpeed;
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the player
        controller.Move(moveDirection * Time.deltaTime);
    }

    private void CameraLook()
    {
        // Get mouse input for horizontal and vertical look
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Calculate and clamp vertical rotation
        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        // Apply vertical rotation to the camera only
        playerCamera.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
    }
}
