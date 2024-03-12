using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackCV : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rangeIn;
    [SerializeField] private float rangeOut;
    private int damage;

    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private HealthHero healthHero;
    private HealthEnemy healthEnemy;
    private HeroKnightTest heroKnightTest;
    private AudioManager audioManager;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        heroKnightTest = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroKnightTest>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack 2");

            }
        }
        else if(PlayeOutSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0.3f;
                anim.SetTrigger("Attack 1");

            }
        }


    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeIn * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeIn, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            healthHero = hit.transform.GetComponent<HealthHero>();
        }

        return hit.collider != null;
    }

    private bool PlayeOutSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeOut * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeOut, boxCollider.bounds.size.y * rangeOut * 0.6f, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            healthHero = hit.transform.GetComponent<HealthHero>();
        }

        return hit.collider != null;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeIn * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeIn, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangeOut * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeOut, boxCollider.bounds.size.y * rangeOut * 0.6f, boxCollider.bounds.size.z));

    }

    private void DamagePlayer()
    {
        if (PlayerInSight() && heroKnightTest.tag == ("Player"))
        {
            healthHero.TakeDamage(damage = Random.Range(12, 15));
            audioManager.PlaySFX(audioManager.attackEnemy1);
        }
        else if(PlayeOutSight() && heroKnightTest.tag == ("Player"))
        {
            healthHero.TakeDamage(damage = Random.Range(15, 18));
            audioManager.PlaySFX(audioManager.attackEnemy3);
        }
        else if(PlayerInSight() && heroKnightTest.tag == ("Shield"))
        {
            healthHero.TakeDamage(5);
            audioManager.PlaySFX(audioManager.attackEnemy1);
        }
        else
        {
            healthHero.TakeDamage(5);
            audioManager.PlaySFX(audioManager.attackEnemy3);
        }
    }

}
