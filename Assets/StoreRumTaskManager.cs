using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRumTaskManager : MonoBehaviour
{
    public TaskPaperManager taskManager;

    private int rumStored = 0;
    private int targetRumStored = 5;
    private bool taskComplete = false;
    private bool taskStarted = false;

    public GameObject[] rumBarrels; // Assign these in the Inspector

    private void Start()
    {
        foreach (GameObject barrel in rumBarrels)
        {
            barrel.GetComponent<RumBottleInteractable>().SetInteractable(false);
        }
    }

    public void RegisterStoredRum()
    {
        if (taskComplete) return;

        rumStored++;
        taskManager.UpdateTaskProgress("Store Rum");

        if (rumStored == targetRumStored)
        {
            taskComplete = true;
        }
    }

    public void SetTaskStarted(bool isStarted)
    {
        taskStarted = isStarted;

        if (taskStarted)
        {
            foreach (GameObject barrel in rumBarrels)
            {
                barrel.GetComponent<RumBottleInteractable>().SetInteractable(true);
            }
        }
    }

    public bool GetTaskStarted() => taskStarted;
}
