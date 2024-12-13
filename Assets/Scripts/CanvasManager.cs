using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject telescopeUI;
    [SerializeField] private GameObject eyelidsUI;

    // Start is called before the first frame update
    void Start()
    {
        telescopeUI.SetActive(true);
        eyelidsUI.SetActive(true);
    }
}
