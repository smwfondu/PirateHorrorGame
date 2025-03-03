using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class VisualOpeningSequence : MonoBehaviour
{
    public Image whiteScreen; // UI Image covering screen (White Panel)
    public OpeningSequence os;
    public PostProcessVolume postProcessingVolume; // Post-processing volume for blur effect

    public AudioSource highRingingAudio;
    public AudioSource lightSwitchAudio;

    [Header("Sounds To Fade In")]
    public AudioSource lightRingingAudio;
    public AudioSource distortionAudio;

    private DepthOfField dof;
    private float fadeDuration = 10f;
    private float blurDuration = 12f;
    private float audioFadeDuration = 10f;

    void Start()
    {
        // Ensure white screen is fully visible at start
        whiteScreen.gameObject.SetActive(true);
        whiteScreen.color = new Color(0, 0, 0, 1);

        if (postProcessingVolume.profile != null)
        {
            postProcessingVolume.profile.TryGetSettings(out dof);
            if (dof != null)
            {
                dof.active = true;
                dof.focalLength.value = 300f; // Maximum blur
            }
        }

        // Start the opening sequence
        StartCoroutine(FadeInSequence());
    }

    IEnumerator FadeInSequence()
    {
        float timer = 0f;

        yield return new WaitForSeconds(5f);

        StartCoroutine(UnBlurVision());

        lightSwitchAudio.Play();
        highRingingAudio.Play();
        StartCoroutine(FadeOutAudio(highRingingAudio));

        lightRingingAudio.Play();
        StartCoroutine(FadeInAudio(lightRingingAudio, 0.3f));

        distortionAudio.Play();
        StartCoroutine(FadeInAudio(distortionAudio, 0.1f));

        // Fade out the white screen
        while (timer < fadeDuration)
        {
            whiteScreen.color = new Color(Mathf.Lerp(255f, 0f, timer / fadeDuration), Mathf.Lerp(255f, 0f, timer / fadeDuration), 
                Mathf.Lerp(255f, 0f, timer / fadeDuration), Mathf.Lerp(1f, 0f, timer / fadeDuration));
            timer += Time.deltaTime;
            yield return null;
        }
        whiteScreen.color = new Color(0, 0, 0, 0);

        yield return new WaitForSeconds(2f);
        os.StartDialgoue();        
    }

    IEnumerator UnBlurVision()
    {
        float timer = 0f;
        float startBlur = 100f;
        float endBlur = 25f; // Normal vision

        while (timer < blurDuration)
        {
            if (dof != null)
            {
                dof.focalLength.value = Mathf.Lerp(startBlur, endBlur, timer / blurDuration);
            }
            timer += Time.deltaTime;
            yield return null;
        }
        if (dof != null)
        {
            dof.focalLength.value = endBlur;
        }
    }

    IEnumerator FadeOutAudio(AudioSource audioSource)
    {
        float timer = 0f;
        float startVolume = audioSource.volume;

        while (timer < audioFadeDuration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / audioFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }

    IEnumerator FadeInAudio(AudioSource audioSource, float targetVolume)
    {
        float timer = 0f;

        while (timer < audioFadeDuration)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, timer / audioFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }
}
