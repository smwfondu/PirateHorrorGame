using System.Collections;
using System.Text;
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
    public AudioSource blipSound;

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
            displayText.text = "";
            StartCoroutine(LoadingSequence());
        }

        if (hasFinished && Input.GetKeyDown(KeyCode.Space))
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
        Debug.Log("loading sequence starts");
        yield return new WaitForSeconds(1f);

        Debug.Log("bootupSound starts");
        bootUpSound.Play();
        yield return new WaitForSeconds(1f);

        // Use StringBuilder to improve performance
        StringBuilder sb = new StringBuilder();

        sb.AppendLine(">> INITIALIZING FRONMIND OS v3.27");
        yield return new WaitForSeconds(0.5f);
        sb.AppendLine(">> SYSTEM DIAGNOSTICS...");
        yield return new WaitForSeconds(0.2f);

        foreach (string check in systemChecks)
        {
            sb.AppendLine(check);
            displayText.text = sb.ToString();
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }

        yield return new WaitForSeconds(0.5f);
        sb.Clear(); // Reset text efficiently
        sb.AppendLine(">> CORE SYSTEMS BOOTING...");
        yield return new WaitForSeconds(0.3f);

        foreach (string process in loadingProcesses)
        {
            sb.Append($">> {process}... ");
            yield return new WaitForSeconds(0.2f);
            sb.AppendLine("[ OK ]");
            displayText.text = sb.ToString();
            yield return new WaitForSeconds(Random.Range(0.1f, 0.25f));
        }

        yield return new WaitForSeconds(0.5f);
        sb.Clear();
        sb.Append("\n>> LOADING SECURITY PROTOCOLS... ");
        yield return new WaitForSeconds(0.3f);
        sb.AppendLine("AUTHORIZED");
        yield return new WaitForSeconds(0.5f);

        sb.AppendLine("\n>> ESTABLISHING FRONMIND NETWORK CONNECTION...");
        yield return new WaitForSeconds(0.5f);

        foreach (string syncStep in networkSync)
        {
            sb.AppendLine(syncStep);
            displayText.text = sb.ToString();
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);
        sb.AppendLine("\n>> SYSTEM ONLINE - FRONMIND INTERFACE READY");
        displayText.text = sb.ToString();
        yield return new WaitForSeconds(1.5f);

        // Simulate a blank screen transition
        displayText.text = "";
        yield return new WaitForSeconds(3f);

        // Final prompt
        displayText.alignment = TextAlignmentOptions.Center;
        displayText.alignment = TextAlignmentOptions.Midline;
        displayText.fontSize = 20f;
        hasFinished = true;

        while (true)
        {
            blipSound.Play();
            displayText.text = "PRESS SPACE TO START SIMULATION";
            yield return new WaitForSecondsRealtime(2f);
            displayText.text = "";
            yield return new WaitForSecondsRealtime(0.4f);
        }
    }
}
