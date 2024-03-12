using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private HealthHero healthHero;

    private void Awake()
    {
        healthHero = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthHero>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            healthHero.TakeDamage(15);
        }
    }
}
