using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Import for TextMeshPro

public class TypingTextEffect : MonoBehaviour
{
    public TextMeshProUGUI loadingText; // Reference to the TextMeshPro text component
    public string dayPrefix = "Day ";  // Prefix text
    public int dayNumber = 1;          // Example day number
    public string dayTitle = "Paloma"; // Example title
    public float typingSpeed = 0.1f;  // Delay between each character
    public float displayDuration = 3f; // Time the text remains on the screen

    public string nextSceneName = "NextScene"; // Name of the next scene to load

    private void Start()
    {
        // Start the typing effect coroutine
        StartCoroutine(ShowLoadingText());
    }

    private IEnumerator ShowLoadingText()
    {
        string fullText = $"{dayPrefix}{dayNumber}: {dayTitle}";
        loadingText.text = "";

        yield return new WaitForSeconds(displayDuration);

        // Type out the text letter by letter
        foreach (char letter in fullText)
        {
            loadingText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait for the specified display duration
        yield return new WaitForSeconds(2f);

        // Clear the text and load the next scene
        loadingText.text = "";

        yield return new WaitForSeconds(2f);
    }
}
