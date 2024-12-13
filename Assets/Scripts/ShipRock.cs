using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRock : MonoBehaviour
{
    public float rockingSpeed = 1.0f; // Speed of the rocking motion
    public float rockingAngle = 1.0f; // Maximum angle of tilt

    private void Update()
    {
        float angle = Mathf.Sin(Time.time * rockingSpeed) * rockingAngle;
        transform.localRotation = Quaternion.Euler(angle, 0, angle / 2); // Tilt along the x-axis for front-back rocking
    }
}
