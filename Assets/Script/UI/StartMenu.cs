using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{   
    private AudioManager audioManager;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject levelUI;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void StartBtn()
    {
        levelUI.SetActive(true);
        audioManager.PlaySFX(audioManager.btn);
    }

    public void Option()
    {
        optionUI.SetActive(true);
        audioManager.PlaySFX(audioManager.btn);
    }

    public void Back()
    {
        optionUI.SetActive(false);
        levelUI.SetActive(false);
        audioManager.PlaySFX(audioManager.btn);
    }

    public void QuitBtn()
    {
        Application.Quit();
        audioManager.PlaySFX(audioManager.btn);
    }
}
