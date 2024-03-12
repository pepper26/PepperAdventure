using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-----------audio source----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("-----------SFX source------------")]
    public AudioClip background;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip death;
    public AudioClip block;
    public AudioClip jump;
    public AudioClip roll;
    public AudioClip hurt;
    public AudioClip itemHealth;
    public AudioClip landing;
    public AudioClip attackEnemy1;
    public AudioClip attackEnemy2;
    public AudioClip attackEnemy3;
    public AudioClip attackEnemy4;
    public AudioClip attackEnemy5;
    public AudioClip btn;

    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

}
