using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorManager : MonoBehaviour
{
    public TextMeshProUGUI displayText;  // Assign in Inspector
    public bool isActive = false;  // Controlled by another script
    private bool hasStarted = false;

    void Update()
    {
        if (isActive && !hasStarted)
        {
            hasStarted = true;
            StartCoroutine(LoadingSequence());
        }

        if (hasStarted && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Game Start Triggered!");
        }
    }

    IEnumerator LoadingSequence()
    {
        displayText.text = "";
        yield return new WaitForSeconds(3f);

        // Boot-up Sequence
        displayText.text = ">> INITIALIZING FRONMIND OS v3.27";
        yield return new WaitForSeconds(1f);
        displayText.text += "\n>> SYSTEM CHECK...";
        yield return new WaitForSeconds(1f);
        displayText.text += " OK\n";

        yield return new WaitForSeconds(0.5f);
        displayText.text = ">> BOOTING CORE PROCESSES";
        yield return new WaitForSeconds(0.5f);
        displayText.text += ".";
        yield return new WaitForSeconds(0.5f);
        displayText.text += ".";
        yield return new WaitForSeconds(0.5f);
        displayText.text += ".\n";
        yield return new WaitForSeconds(0.5f);
        displayText.text = ">> BOOTING CORE PROCESSES.";
        yield return new WaitForSeconds(0.5f);
        displayText.text += ".";
        yield return new WaitForSeconds(0.5f);
        displayText.text += ".\n";
        yield return new WaitForSeconds(0.5f);
        displayText.text = ">> BOOTING CORE PROCESSES.";
        yield return new WaitForSeconds(0.5f);
        displayText.text += ".";
        yield return new WaitForSeconds(0.5f);
        displayText.text += ".\n";

        yield return new WaitForSeconds(1f);
        displayText.text = ">> LOADING SECURITY PROTOCOLS...";
        yield return new WaitForSeconds(1f);
        displayText.text += " AUTHORIZED\n";

        yield return new WaitForSeconds(1f);
        displayText.text = ">> ESTABLISHING FRONMIND NETWORK CONNECTION...\n";
        yield return new WaitForSeconds(1f);
        displayText.text += "[ 1/4 ] CONNECTING TO INTERNAL SYSTEMS\n";
        yield return new WaitForSeconds(1f);
        displayText.text += "[ 2/4 ] SYNCING MEMORY MODULES\n";
        yield return new WaitForSeconds(1f);
        displayText.text += "[ 3/4 ] VERIFYING OPERATIONAL STABILITY\n";
        yield return new WaitForSeconds(1f);
        displayText.text += "[ 4/4 ] NEURAL LINK STATUS: STABLE\n";

        yield return new WaitForSeconds(1.5f);
        displayText.text = ">> SYSTEM ONLINE - FRONMIND INTERFACE READY\n";
        yield return new WaitForSeconds(2f);

        // Simulate a blank screen transition
        displayText.text = "";
        yield return new WaitForSeconds(2.5f);

        // Final prompt
        displayText.text = "PRESS SPACE TO START";
    }
}
