using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCigaretteManager : MonoBehaviour
{
    public Transform tablePosition;  // Starting position
    public Transform mouthPosition;  // Position near mouth

    public ParticleSystem idleSmoke;  // Constantly burning smoke
    public ParticleSystem bigExhaleSmoke;  // Large puff when released early
    public ParticleSystem smallPuffSmoke;  // Three small puffs if held too long

    public AudioSource inhaleSound;
    public AudioSource coughSound;

    public float moveSpeed = 5f;  // Speed of movement
    public float maxHoldTime = 3f; // Time before coughing
    public float minExhaleTime = 1f; // Minimum time before exhaling a big puff

    private bool isHolding = false;
    private bool isAtMouth = false;
    private float holdTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Left-click start
        {
            StartSmoking();
        }

        if (Input.GetMouseButton(0))  // Holding left-click
        {
            holdTime += Time.deltaTime;

            if (isAtMouth && idleSmoke.isPlaying)
            {
                idleSmoke.Stop();  // Remove idle smoke when drawing
            }
        }

        if (Input.GetMouseButtonUp(0))  // Released left-click
        {
            StopSmoking();
        }

        if (isHolding)
        {
            MoveCigaretteToMouth();
        }
        else
        {
            MoveCigaretteToTable();
        }
    }

    void StartSmoking()
    {
        isHolding = true;
        inhaleSound.Play();
    }

    void StopSmoking()
    {
        isHolding = false;

        if (holdTime >= maxHoldTime)
        {
            StartCoroutine(ExhaleMultiplePuffs());
        }
        else if (holdTime >= minExhaleTime)
        {
            bigExhaleSmoke.Play();
        }

        inhaleSound.Stop();
        holdTime = 0f;
    }

    void MoveCigaretteToMouth()
    {
        transform.position = Vector3.Lerp(transform.position, mouthPosition.position, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, mouthPosition.position) < 0.1f)
        {
            isAtMouth = true;
        }
    }

    void MoveCigaretteToTable()
    {
        transform.position = Vector3.Lerp(transform.position, tablePosition.position, Time.deltaTime * moveSpeed);

        if (Vector3.Distance(transform.position, tablePosition.position) < 0.1f)
        {
            isAtMouth = false;
            if (!idleSmoke.isPlaying)
                idleSmoke.Play();
        }
    }

    IEnumerator ExhaleMultiplePuffs()
    {
        for (int i = 0; i < 3; i++)
        {
            smallPuffSmoke.Play();
            yield return new WaitForSeconds(0.5f);
        }

        coughSound.Play();
    }
}
