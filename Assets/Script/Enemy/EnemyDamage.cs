using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] protected float damageShield;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<HealthHero>().TakeDamage(damage);
        }

        if (collision.tag == "Shield")
        {
            collision.GetComponent<HealthHero>().TakeDamage(damageShield);
        }
    }
}