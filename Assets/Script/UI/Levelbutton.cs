using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelbutton : MonoBehaviour
{
    public int sceneIndex;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void OpenScene()
    {
        audioManager.PlaySFX(audioManager.btn);
        SceneManager.LoadScene(sceneIndex);
    }
}
