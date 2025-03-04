using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningSequence : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public MonitorManager mm;
    public AudioSource repVoiceAudio; // Audio for the FronMind Rep
    public AudioSource finalAnimationSound;
    public AudioSource distortionSound;
    public float audioFadeDuration = 3.0f;
    public float dialogueSpeed = 0.05f;
    public float dialogueWaitTime = 2.0f;
    public Animator finalAnimation;
    public Animator repAnimator; // FronMind Rep animator
    public Light sceneLight;
    public string[] repAnimations; // Animation names
    public bool testingMode = false; // Animation names

    private int animNum = 0;

    private string[] testDialogueLines = {
        "R - Test line 1",
        "C - Test line 2",
    };

    private readonly string[] dialogueLines = {
        "R - Welcome back, Mr. Armstrong. How do you feel?",
        "M - Who are you? Where am I?",
        "R - Take your time. No need to rush for answers. You’ve just undergone the procedure. Some disorientation is normal.",
        "M - The procedure… It’s done?",
        "R - Yes. It went exactly as planned. How are you feeling?",
        "M - I… don’t know. Strange. Heavy.",
        "R - That will pass. Your mind is still adjusting.",
        "M - …What did you take?",
        "R - Only what was necessary. Know that you may notice gaps. That’s expected. But you are free now, Mr. Armstrong. No more pain.",
        "M - I don’t feel so good.",
        "R - Recovery takes time. We will start you off today with something easy. Please, begin when you are ready."
    };

    private string[] dialogueLinesInUse = { };

    private void Start()
    {
        if(testingMode) { dialogueLinesInUse = testDialogueLines; }
        else { dialogueLinesInUse = dialogueLines; }
    }

    public void StartDialgoue()
    {
        StartCoroutine(OpeningSequenceRoutine());
    }

    private IEnumerator OpeningSequenceRoutine()
    {
        yield return StartCoroutine(PlayDialogueSequence());
        finalAnimation.SetTrigger("FinalAnimationTrigger");
        finalAnimationSound.Play();
        distortionSound.Stop();
        yield return new WaitForSeconds(3f);
        Debug.Log("mm is activated.");
        mm.isActive = true;
    }

    private IEnumerator PlayDialogueSequence()
    {
        for (int i = 0; i < dialogueLinesInUse.Length; i++)
        {
            string currentLine = dialogueLinesInUse[i];

            // **Check if Rep is speaking**
            bool isRepSpeaking = currentLine.StartsWith("R - ");

            if (isRepSpeaking)
            {
                PlayNextRepAnimation();
                repVoiceAudio.Play();
                yield return StartCoroutine(TypeDialogueRep(currentLine));
            } 
            else
            {
                yield return StartCoroutine(TypeDialogueMike(currentLine));
            }

            repVoiceAudio.Stop(); // **Stop the Rep's voice when typing stops**
            yield return new WaitForSeconds(dialogueWaitTime + 1);

            dialogueText.text = "";
            yield return new WaitForSeconds(dialogueWaitTime);
        }
    }

    private IEnumerator TypeDialogueRep(string line)
    {
        int numChar = 0;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            if ((letter == '.' || letter == '?') && numChar != line.Length - 1)
            {
                repVoiceAudio.Stop();
                yield return new WaitForSeconds(dialogueSpeed * 10);
                repVoiceAudio.Play();
            }

            yield return new WaitForSeconds(dialogueSpeed);
            numChar++;
        }
    }

    private IEnumerator TypeDialogueMike(string line)
    {
        int numChar = 0;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;

            if ((letter == '.' || letter == '?') && numChar != line.Length - 1)
            {
                yield return new WaitForSeconds(dialogueSpeed * 10);
            }

            yield return new WaitForSeconds(dialogueSpeed);
            numChar++;
        }
    }   

    private void PlayNextRepAnimation()
    {
        if (repAnimations.Length == 0) return;
        repAnimator.Play(repAnimations[animNum]);

        animNum++;
        if (animNum >= repAnimations.Length)
        {
            animNum = 0;
        }
    }
}
