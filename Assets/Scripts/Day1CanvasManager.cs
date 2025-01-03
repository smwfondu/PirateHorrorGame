using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject telescopeImage;
    [SerializeField] private GameObject eyelids;

    // Start is called before the first frame update
    void Start()
    {
        telescopeImage.SetActive(true);
        eyelids.SetActive(true);
    }
}
