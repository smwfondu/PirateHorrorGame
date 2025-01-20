using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepDialogueMovement : MonoBehaviour
{
    public Animator animator;
    public string[] animationNames;

    private string prevAnimation;

    public void PlayRandomAnimation()
    {
        if (animationNames.Length == 0) return;

        string randomAnimation = animationNames[Random.Range(0, animationNames.Length)];
        while (randomAnimation == prevAnimation)
        {
            randomAnimation = animationNames[Random.Range(0, animationNames.Length)];
        } 

        // Play the selected animation
        animator.Play(randomAnimation);

        prevAnimation = randomAnimation;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PlayRandomAnimation();
        }
    }
}
