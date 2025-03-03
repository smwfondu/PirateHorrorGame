using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonitorManager : MonoBehaviour
{
    public TextMeshProUGUI displayText;  // Assign in Inspector
    public Image blackBG;
    public bool isActive = false;  // Controlled by another script

    [Header("Computer Sounds")]
    public AudioSource bootUpSound;

    private bool hasStarted = false;
    private bool hasFinished = false;

    private string[] systemChecks = {
        "Memory Check.......................[ OK ]",
        "Processor Cores....................[ OK ]",
        "Disk Integrity.....................[ OK ]",
        "BIOS Verification..................[ OK ]",
        "Neural Link Calibration............[ OK ]",
        "Cognitive Partition Integrity......[ OK ]",
        "Firewall Status....................[ SECURE ]",
        "Data Encryption Modules............[ ACTIVE ]",
        "Intrusion Detection System.........[ ENABLED ]",
        "Network Uplink.....................[ STABLE ]"
    };

    private string[] loadingProcesses = {
        "Initializing system daemons",
        "Parsing security policies",
        "Validating cryptographic signatures",
        "Synchronizing AI cognitive modules",
        "Verifying user identity",
        "Decrypting neural pathways",
        "Loading executive control functions",
        "Scanning for memory anomalies",
        "Finalizing OS boot process"
    };

    private string[] networkSync = {
        "[ 1/5 ] Authenticating user credentials",
        "[ 2/5 ] Synchronizing encrypted memory clusters",
        "[ 3/5 ] Verifying distributed processing integrity",
        "[ 4/5 ] Establishing neural command link",
        "[ 5/5 ] Handshake successful - System stable"
    };

    private void Start()
    {
        displayText.text = "";
    }

    void Update()
    {
        if (isActive && !hasStarted)
        {
            hasStarted = true;
            StartCoroutine(LoadingSequence());
        }

        if(hasFinished && Input.GetKeyDown(KeyCode.Space))
        {
            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>(); // Find all AudioSources in the scene

            foreach (AudioSource audioSource in allAudioSources)
            {
                audioSource.Stop(); // Stop each one
            }

            blackBG.color = new Color(0, 0, 0, 1);
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator LoadingSequence()
    {
        displayText.text = "";
        yield return new WaitForSeconds(1f);

        bootUpSound.Play();
        yield return new WaitForSeconds(1f);

        // Boot-up sequence
        displayText.text = ">> INITIALIZING FRONMIND OS v3.27\n";
        yield return new WaitForSeconds(0.5f);
        displayText.text += ">> SYSTEM DIAGNOSTICS...\n";
        yield return new WaitForSeconds(0.2f);

        foreach (string check in systemChecks)
        {
            displayText.text += check + "\n";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }

        yield return new WaitForSeconds(0.5f);
        displayText.text = "\n>> CORE SYSTEMS BOOTING...\n";
        yield return new WaitForSeconds(0.3f);

        foreach (string process in loadingProcesses)
        {
            displayText.text += ">> " + process + "... ";
            yield return new WaitForSeconds(0.2f);
            displayText.text += "[ OK ]\n";
            yield return new WaitForSeconds(Random.Range(0.1f, 0.25f));
        }

        yield return new WaitForSeconds(0.5f);
        displayText.text = "\n>> LOADING SECURITY PROTOCOLS... ";
        yield return new WaitForSeconds(0.3f);
        displayText.text += "AUTHORIZED\n";
        yield return new WaitForSeconds(0.5f);

        displayText.text += "\n>> ESTABLISHING FRONMIND NETWORK CONNECTION...\n";
        yield return new WaitForSeconds(0.5f);

        foreach (string syncStep in networkSync)
        {
            displayText.text += syncStep + "\n";
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);
        displayText.text = "\n>> SYSTEM ONLINE - FRONMIND INTERFACE READY\n";
        yield return new WaitForSeconds(1.5f);

        // Simulate a blank screen transition
        displayText.text = "";
        yield return new WaitForSeconds(1f);

        // Final prompt
        displayText.alignment = TextAlignmentOptions.Center;
        displayText.alignment = TextAlignmentOptions.Midline;
        displayText.fontSize = 20f;
        hasFinished = true;

        while (true)
        {
            displayText.text = "PRESS SPACE TO START SIMULATION";
            yield return new WaitForSeconds(2f);
            displayText.text = "";
            yield return new WaitForSeconds(0.4f);
        }
    }
}
