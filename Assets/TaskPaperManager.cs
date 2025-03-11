using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskPaperManager : MonoBehaviour
{
    public GameObject taskPaperQuad;
    public Vector3 raisedPosition;
    public float speed = 5f;

    [SerializeField] private CannonTaskManager cannonTaskManager;
    [SerializeField] private StoreRumTaskManager rumTaskManager;

    [Header("Task Texts")]
    [SerializeField] private TextMeshPro cannonTaskText;
    [SerializeField] private TextMeshPro storeRumText;
    [SerializeField] private TextMeshPro catchFishText;

    private Vector3 startPosition;
    private bool isRising = false;
    private bool grabbedList = false;

    private int cannonProgress = 0;
    private int storeRumProgress = 0;
    private int catchFishProgress = 0;

    private int cannonGoal = 5;
    private int storeRumGoal = 3;
    private int catchFishGoal = 3;

    private enum TaskState { Inactive, Active, Completed }
    private TaskState cannonTaskState = TaskState.Active;
    private TaskState storeRumTaskState = TaskState.Inactive;
    private TaskState catchFishTaskState = TaskState.Inactive;

    void Start()
    {
        startPosition = transform.localPosition;
        taskPaperQuad.SetActive(false);

        UpdateTaskText();
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

        if (grabbedList)
        {
            taskPaperQuad.SetActive(true);
            cannonTaskManager.SetTaskStarted(true);
        }

        MoveObject();
    }

    void MoveObject()
    {
        Vector3 targetPosition = isRising ? raisedPosition : startPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * speed);
    }

    public void UpdateTaskProgress(string task)
    {
        switch (task)
        {
            case "Clean Cannons":
                if (cannonTaskState == TaskState.Active)
                {
                    cannonProgress++;
                    if (cannonProgress >= cannonGoal)
                    {
                        cannonProgress = cannonGoal;
                        CompleteTask(ref cannonTaskState, cannonTaskText);
                        UnlockNextTask(ref storeRumTaskState, storeRumText);
                    }
                }
                break;

            case "Store Rum":
                if (storeRumTaskState == TaskState.Active)
                {
                    storeRumProgress++;
                    if (storeRumProgress >= storeRumGoal)
                    {
                        storeRumProgress = storeRumGoal;
                        CompleteTask(ref storeRumTaskState, storeRumText);
                        UnlockNextTask(ref catchFishTaskState, catchFishText);
                    }
                }
                break;

            case "Catch Fish":
                if (catchFishTaskState == TaskState.Active)
                {
                    catchFishProgress++;
                    if (catchFishProgress >= catchFishGoal)
                    {
                        catchFishProgress = catchFishGoal;
                        CompleteTask(ref catchFishTaskState, catchFishText);
                    }
                }
                break;
        }

        UpdateTaskText();
    }

    void CompleteTask(ref TaskState taskState, TextMeshPro taskText)
    {
        taskState = TaskState.Completed;
        taskText.text = $"<s>{taskText.text}</s>"; // Strike-through on completion
    }

    void UnlockNextTask(ref TaskState taskState, TextMeshPro taskText)
    {
        taskState = TaskState.Active;
        UpdateTaskText(); // Refresh UI to remove "Locked" state

        if (taskState == storeRumTaskState) // When unlocking "Store Rum" task
        {
            rumTaskManager.SetTaskStarted(true);
        }
    }

    void UpdateTaskText()
    {
        // Formatting for UI clarity
        cannonTaskText.text = GetFormattedTask("1. Clean Cannons", cannonProgress, cannonGoal, cannonTaskState);
        storeRumText.text = GetFormattedTask("2. Store Rum", storeRumProgress, storeRumGoal, storeRumTaskState);
        catchFishText.text = GetFormattedTask("3. Catch Fish", catchFishProgress, catchFishGoal, catchFishTaskState);
    }

    string GetFormattedTask(string taskName, int progress, int goal, TaskState state)
    {
        if (state == TaskState.Completed)
            return $"<s>{taskName}: {progress}/{goal}</s>"; // Strikethrough when completed
        else if (state == TaskState.Inactive)
            return $"<color=#808080><i>{taskName}: Locked</i></color>"; // Gray out inactive tasks
        else
            return $"{taskName}: {progress}/{goal}"; // Normal active task
    }

    public void SetGrabbedList(bool isGrabbed) => grabbedList = isGrabbed;
}
