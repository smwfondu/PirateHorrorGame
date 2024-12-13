using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimelineManager : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    
    public void LoadNextScene()
    {
        // Replace with your scene loading logic
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ChangeText(string newText)
    {
        textMeshPro.text = newText;
    }
}
