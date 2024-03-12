using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void NextBtn()
    {
        audioManager.PlaySFX(audioManager.btn);
        SceneManager.LoadScene("StartMenu");
    }

}
