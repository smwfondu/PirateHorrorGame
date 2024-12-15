using UnityEngine;

public class FishDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Fish"))
        {
            Destroy(other.gameObject);
        }
    }
}
