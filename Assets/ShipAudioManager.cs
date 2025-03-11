using UnityEngine;
using UnityEngine.Audio;

public class ShipAudioManager : MonoBehaviour
{
    public AudioMixer shipMixer;
    public Transform player;
    public float transitionHeight = 0f; // Y position where sounds switch
    public float fadeSpeed = 2f;

    private float targetAboveVolume;
    private float targetBelowVolume;

    void Update()
    {
        float t = Mathf.Clamp01((player.position.y - transitionHeight) / 2f); // 0 (below) to 1 (above)

        targetAboveVolume = Mathf.Lerp(-80f, 0f, t); // -80dB is silent, 0dB is full volume
        targetBelowVolume = Mathf.Lerp(0f, -80f, t);

        shipMixer.SetFloat("AboveDeckVolume", Mathf.Lerp(targetAboveVolume, 0f, fadeSpeed * Time.deltaTime));
        shipMixer.SetFloat("BelowDeckVolume", Mathf.Lerp(targetBelowVolume, 0f, fadeSpeed * Time.deltaTime));
    }
}
