using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepDialogueMovement : MonoBehaviour
{
    public Animator animator;
    public string[] animationNames;
    public TextMeshProUGUI dialogueText;

    private int animNum = 0;
    private int diaNum = 0;
    private string[] dialogue = { "We understand this is a difficult time for you.",
                                    "I just…, I need the pain to stop. I can't keep living like this.",
                                    "That's exactly what we're here for. Our procedure is designed to relieve, to help you move forward past this.",
                                    "Will I forget everything?",
                                    "Only the memories that cause you distress. The ones tied to your incident.",
                                    "And what happens after? How do I... function?",
                                    "You'll go through a short recovery period.",
                                    "During that time, you'll engage in simple, structured activities designed to help your mind adjust.",
                                    "It's a form of cognitive therapy, very effective.",
                                    "Simple activities?",
                                    "Think of it as a simulation of your mind, something you might have dreamt of as a child.",
                                    "It helps to keep the mind at ease while it recovers.",
                                    "And the quiz?",
                                    "Merely a formality to ensure the procedure has taken effect. You'll do fine.",
                                    "Will I… will there be any chance of remembering?",
                                    "Our success rate is perfect, Mr. *** and we intend to keep it that way.",
                                    "Alright… if it’ll help."};

    private void Start()
    {
        dialogueText.text = dialogue[diaNum];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayNextDialogue();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            PlayNextRepAnimation();
        }
    }

    public void PlayNextRepAnimation()
    {
        string randomAnimation = animationNames[animNum];
        animator.Play(randomAnimation);

        animNum++;
        if(animNum == 5)
        {
            animNum = 0;
        }
    }

    public void PlayNextDialogue()
    {
        diaNum++;

        if(dialogue.Length >= diaNum)
        {
            dialogueText.text = dialogue[diaNum];
        }
    }
}
