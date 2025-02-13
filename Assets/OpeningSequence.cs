using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningSequence : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Image blackBG;
    public AudioSource ambientAudio;
    public AudioSource repVoiceAudio; // Audio for the FronMind Rep
    public AudioSource finalAnimationSound;
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

    private string[] dialogueLines = {
        "R - We understand this is a difficult time for you.",
        "C - I just…, I need the pain to stop. I can't keep living like this.",
        "R - That's exactly what we're here for. Our procedure is designed to relieve, to help you move forward past this.",
        "C - Will I forget everything?",
        "R - Only the memories that cause you distress. The ones tied to your incident.",
        "C - And what happens after? How do I... function?",
        "R - You'll go through a short recovery period.",
        "R - During that time, you'll engage in simple, structured activities designed to help your mind adjust.",
        "R - It's a form of cognitive therapy, very effective.",
        "C - Simple activities?",
        "R - Think of it as a simulation of your mind, something you might have dreamt of as a child.",
        "R - It helps to keep the mind at ease while it recovers.",
        "C - And the quiz?",
        "R - Merely a formality to ensure the procedure has taken effect. You'll do fine.",
        "C - Will I… will there be any chance of remembering?",
        "R - Our success rate is perfect, Mr. *** and we intend to keep it that way.",
    };

    private string[] dialogueLinesInUse = { };

    private void Start()
    {
        if(testingMode) { dialogueLinesInUse = testDialogueLines; }
        else { dialogueLinesInUse = dialogueLines; }
        
        blackBG.color = new Color(0, 0, 0, 1);
        StartCoroutine(OpeningSequenceRoutine());
    }

    private IEnumerator OpeningSequenceRoutine()
    {
        yield return StartCoroutine(FadeAudioIn());
        yield return StartCoroutine(FadeLightIn());
        yield return StartCoroutine(PlayDialogueSequence());
        finalAnimation.SetTrigger("FinalAnimationTrigger");
        finalAnimationSound.Play();
    }

    private IEnumerator FadeAudioIn()
    {
        float timer = 0f;
        while (timer < audioFadeDuration)
        {
            ambientAudio.volume = Mathf.Lerp(0, 0.3f, timer / audioFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        ambientAudio.volume = 0.3f;
    }

    private IEnumerator FadeLightIn()
    {
        float timer = 0f;
        while (timer < audioFadeDuration)
        {
            blackBG.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer / audioFadeDuration));
            timer += Time.deltaTime;
            yield return null;
        }
        blackBG.color = new Color(0, 0, 0, 0);
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
            }

            yield return StartCoroutine(TypeDialogue(currentLine));

            repVoiceAudio.Stop(); // **Stop the Rep's voice when typing stops**
            yield return new WaitForSeconds(dialogueWaitTime + 1);

            dialogueText.text = "";
            yield return new WaitForSeconds(dialogueWaitTime);
        }
    }

    private IEnumerator TypeDialogue(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueSpeed);
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
