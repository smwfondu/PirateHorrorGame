using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day1CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject telescopeImage;
    
    // Start is called before the first frame update
    void Start()
    {
        telescopeImage.SetActive(true);
    }
}
