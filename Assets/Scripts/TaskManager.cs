using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    private bool GrabbedList;

    // Cannon Task

    public void ChangeGrabbedList(bool isListGrabbed)
    {
        GrabbedList = isListGrabbed;
    }

    private void Update()
    {
        
    }
}
