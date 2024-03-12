using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPause;
    public GameObject move;
    public GameObject pauseMenuUI;
    public GameObject restartMenuUI;
    public GameObject exitMenuUI;
    public GameObject gameoverUI;
    public GameObject loadingUI;
    public GameObject nextBtn;
    public GameObject volumeUI;
    [SerializeField] private GameObject boss;
    PlayerControl controls;
    [SerializeField] private HealthHero healthHero;
    [SerializeField] private HeroKnightTest heroKnightTest;
    private AudioManager audioManager;

    private void Start()
    {
        heroKnightTest = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroKnightTest>();
        healthHero = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthHero>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Time.timeScale = 1f;
        move.SetActive(!GameIsPause);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            move.SetActive(false);
            if (GameIsPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        else
        {
            move.SetActive(!GameIsPause);
        }

        if(healthHero.death == true)
        {
            Invoke("Gameover", 1f);
        }


        Invoke("BtnStart", 4f);
    }

    public void PauseBTN()
    {
        audioManager.PlaySFX(audioManager.btn);
        if (GameIsPause)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        audioManager.PlaySFX(audioManager.btn);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    public void Pause()
    {
        heroKnightTest.inputX = 0;
        audioManager.PlaySFX(audioManager.btn);
        pauseMenuUI.SetActive(true);
        restartMenuUI.SetActive(false);
        exitMenuUI.SetActive(false);
        volumeUI.SetActive(false);
        Time.timeScale = 0f;
        GameIsPause = true;

    }

    public void RestartLevel()
    {
        audioManager.PlaySFX(audioManager.btn);
        restartMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void YesRestart()
    {
        audioManager.PlaySFX(audioManager.btn);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
        GameIsPause = false;
    }

    public void YesExit()
    {
        audioManager.PlaySFX(audioManager.btn);
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1f;
    }

    public void LoadMenu()
    {
        audioManager.PlaySFX(audioManager.btn);
        SceneManager.LoadScene("Start Scene");
        Time.timeScale = 1f;
    }

    public void Quit()
    {
        audioManager.PlaySFX(audioManager.btn);
        pauseMenuUI.SetActive(false);
        restartMenuUI.SetActive(false);
        exitMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Gameover()
    {
        audioManager.PlaySFX(audioManager.btn);
        gameoverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void NoRestart()
    {
        audioManager.PlaySFX(audioManager.btn);
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1f;
    }

    public void NextUI()
    {
        audioManager.PlaySFX(audioManager.btn);
        loadingUI.SetActive(false);
        boss.SetActive(true);
    }

    public void BtnStart()
    {
        nextBtn.SetActive(true);
    }

    public void Option()
    {
        audioManager.PlaySFX(audioManager.btn);
        volumeUI.SetActive(true);
    }
    private void Awake()
    {
        controls = new PlayerControl();
        controls.Enable();
        controls.Action.Pause.performed += ctx => PauseBTN();
    }

}
