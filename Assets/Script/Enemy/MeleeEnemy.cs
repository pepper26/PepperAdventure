using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    public int attack;
    private Animator anim;
    private HealthHero healthHero;
    private EnemyPatrol enemyPatrol;
    private AudioManager audioManager;
    private HeroKnightTest heroKnightTest;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        heroKnightTest = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroKnightTest>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack 1");
                switch (attack)
                {
                    case 1:
                        audioManager.PlaySFX(audioManager.attackEnemy1);
                        break;
                    case 2:
                        audioManager.PlaySFX(audioManager.attackEnemy2);
                        break;
                    case 3:
                        audioManager.PlaySFX(audioManager.attackEnemy3);
                        break;
                    case 4:
                        audioManager.PlaySFX(audioManager.attackEnemy4);
                        break;

                }
            }

        }

        if(enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }



    }


    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            healthHero = hit.transform.GetComponent<HealthHero>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight() && heroKnightTest.tag == ("Player"))
        {
            healthHero.TakeDamage(damage = Random.Range(10, 15));
        }
        else if(PlayerInSight() && heroKnightTest.tag == ("Shield"))
        {
            healthHero.TakeDamage(3);
        }
    }

}
