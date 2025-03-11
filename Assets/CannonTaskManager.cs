using UnityEngine;
using TMPro;

public class CannonTaskManager : MonoBehaviour
{
    public static CannonTaskManager Instance;
    public TaskPaperManager taskManager;

    private int cannonsPolished = 0;
    private int targetCannonsPolished = 5;
    private bool taskComplete = false;

    private bool taskStarted = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPolishedCannon()
    {
        if (taskComplete) return;

        cannonsPolished++;
        taskManager.UpdateTaskProgress("Clean Cannons");

        if (cannonsPolished == targetCannonsPolished)
        {
            taskComplete = true;
        }
    }

    public void SetTaskStarted(bool isStarted) => taskStarted = isStarted;

    public bool GetTaskStarted() => taskStarted;
}
