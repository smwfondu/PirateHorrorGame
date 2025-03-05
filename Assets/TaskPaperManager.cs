using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPaperManager : MonoBehaviour
{
    public Vector3 raisedPosition; // Target position when Tab is held
    private Vector3 startPosition; // Original position
    public float speed = 5f; // Speed of movement
    private bool isRising = false;

    void Start()
    {
        startPosition = transform.localPosition; // Store the starting position in local space
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            isRising = true;
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isRising = false;
        }

        MoveObject();
    }

    void MoveObject()
    {
        Vector3 targetPosition = isRising ? raisedPosition : startPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);
    }
}
