using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public float startingHealthEnemy;
    public float currentHealthEnemy { get; private set; }
    private bool death;
    [SerializeField] private GameObject enemy;
    private Animator anim;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        currentHealthEnemy = startingHealthEnemy;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHealthEnemy = Mathf.Clamp(currentHealthEnemy - damage, 0, startingHealthEnemy);
        if (currentHealthEnemy > 0) 
        {
            anim.SetTrigger("Hurt");
            audioManager.PlaySFX(audioManager.hurt);
        }
        else
        {
            if (!death)
            {
                anim.SetTrigger("Die");
                audioManager.PlaySFX(audioManager.death);
                death = true;
                Invoke("DestroyEnemy", 1f);
            }
        }
    }
    private void DestroyEnemy()
    {
        enemy.gameObject.SetActive(false);
    }

}
