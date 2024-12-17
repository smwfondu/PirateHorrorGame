using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadObject : MonoBehaviour
{
    public int sceneEnd = 1;

    private bool futureDelete = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if(SceneManager.sceneCountInBuildSettings >= sceneEnd)
        {
            futureDelete = true;
        }
    }

    private void Update()
    {
        if(futureDelete)
        {
            if (SceneManager.GetSceneByBuildIndex(sceneEnd) == SceneManager.GetActiveScene())
            {
                Destroy(gameObject);
            }
        }
    }
}
