using UnityEngine;

public class RumStorageZone : MonoBehaviour
{
    public StoreRumTaskManager taskManager;

    private void OnTriggerEnter(Collider other)
    {
        RumBottleInteractable barrel = other.GetComponent<RumBottleInteractable>();
        if (barrel != null)
        {
            taskManager.RegisterStoredRum();
        }
    }
}
