using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Day1CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject telescopeImage;
    [SerializeField] private Image openingBackground;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private AudioSource blipSound;

    private string[] simulationTitleLines = {
        "Recovery Simulation Day 1:",
        "Pre-heating",
    };

    // Start is called before the first frame update
    void Start()
    {
        telescopeImage.SetActive(true);
        titleText.text = "";

        openingBackground.color = new Color(0, 0, 0, 1);
        StartCoroutine(OpeningSequenceRoutine());
    }

    private IEnumerator OpeningSequenceRoutine()
    {
        yield return new WaitForSeconds(3f);

        blipSound.Play();
        titleText.text = simulationTitleLines[0];
        titleText.text += '\n';

        yield return new WaitForSeconds(3f);
        blipSound.Play();
        titleText.text += simulationTitleLines[1];

        yield return new WaitForSeconds(3f);
        blipSound.Play();
        titleText.text = "";

        titleText.gameObject.SetActive(false);
        openingBackground.gameObject.SetActive(false);
    }
}
