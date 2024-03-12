using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackFN : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float rangeIn;
    [SerializeField] private float rangeOut;
    [SerializeField] private float rangeAbove;
    private int damage;

    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private HealthHero healthHero;
    private AudioManager audioManager;
    private HeroKnightTest heroKnightTest;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        heroKnightTest = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroKnightTest>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
       
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
            }
        }
        else if (PlayerOutSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0.5f;
                anim.SetTrigger("Attack 2");
            }
        }
        else if (PlayeAboveSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0.5f;
                anim.SetTrigger("Attack 3");
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

    private bool PlayerOutSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * rangeOut * transform.localScale.x *  colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeOut, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            healthHero = hit.transform.GetComponent<HealthHero>();
        }

        return hit.collider != null;
    }

    private bool PlayeAboveSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.up * rangeAbove * 1 * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeAbove, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

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
            new Vector3(boxCollider.bounds.size.x * rangeOut, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.up * rangeAbove * 1 * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * rangeAbove, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

    }

    private void DamagePlayer()
    {
        if (PlayerInSight() && heroKnightTest.tag == ("Player"))
        {
            healthHero.TakeDamage(damage = Random.Range(12, 15));
        }
        else if(PlayerOutSight() && heroKnightTest.tag == ("Player") || PlayeAboveSight() && heroKnightTest.tag == ("Player"))
        {
            healthHero.TakeDamage(damage = Random.Range(15, 18));
        }
        else if (PlayerInSight() && heroKnightTest.tag == ("Shield"))
        {
            healthHero.TakeDamage(damage = 5);
        }
        else if (PlayerOutSight() && heroKnightTest.tag == ("Shield"))
        {
            healthHero.TakeDamage(damage = 5);
        }
        else
        {
            healthHero.TakeDamage(damage = 5);
        }
    }

}
