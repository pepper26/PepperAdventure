using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class HealthHero : MonoBehaviour
{
    public float startingHealth = 100f;
    public float currentHealth { get; private set; }
    public bool death;
    private Animator anim;
    private AudioManager audioManager;
    [SerializeField] protected GameObject block;
    [SerializeField] protected Transform blockPos;
    private HeroKnightTest heroKnightTest;
    [SerializeField] private AudioSource runSound;
    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        heroKnightTest = GetComponent<HeroKnightTest>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0 && damage <= 5)
        {
            audioManager.PlaySFX(audioManager.block);
            Instantiate(block, blockPos.transform);
        }
        else if (currentHealth > 0 && damage > 6)
        {
            anim.SetTrigger("Hurt");
            audioManager.PlaySFX(audioManager.hurt);
        }
        else
        {
            if (!death)
            {
                anim.SetTrigger("Death");
                GetComponent<HeroKnightTest>().enabled = false;
                audioManager.PlaySFX(audioManager.death);
                runSound.enabled = false;
                death = true;
            }
        }

    }

    public void RestoreHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, startingHealth);
    }

}
