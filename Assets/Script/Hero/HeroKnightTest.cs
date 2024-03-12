using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HeroKnightTest : MonoBehaviour {

    [SerializeField] private    float       speed = 8.0f;
    [SerializeField] private    float       jumpForce = 12f;
    [SerializeField] private    float       rollForce = 7.0f;
    [SerializeField] private    BoxCollider2D playerIdle;
    [SerializeField] private    BoxCollider2D playerJump;
    [SerializeField] private    BoxCollider2D playerRoll;
    [SerializeField] private    BoxCollider2D ereaAttack;
    [SerializeField] private    BoxCollider2D ereaShield;
    [SerializeField] GameObject jump;
    [SerializeField] GameObject landing;

    private float range = 2;
    private int damage;
    private PlayerControl controls;

    private float colliderDistance = 0.89f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private HealthHero healthHero;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask wallLayerMask;

    private Animator            anim;
    private Rigidbody2D         rb;
    private bool                isWallSliding = false;
    private bool                rolling = false;
    private bool                facingRight = true;
    private bool                isShield = false;
    private int                 currentAttack = 0;
    public float                inputX;
    private float               timeSinceAttack = 0.0f;
    private float               rollDuration = 8.0f / 14.0f;
    private float               rollCurrentTime;
    private float               wallJumpingDirection;
    private float               wallJumpingTime = 0.2f;
    private float               wallJumpingCounter;
    private GameObject          currentOnWayPlatform;
    private HealthEnemy         healthEnemy;
    private float wallSlideSpeed = 3f;
    private AudioManager audioManager;
    [SerializeField] private AudioSource runSound;
    private void Awake()
    {
        controls = new PlayerControl();
        controls.Enable();
        controls.Action.Attack.performed += ctx => Attack();
        controls.Action.Roll.performed += ctx => Roll();
        controls.Action.Jump.performed += ctx => Jump();
        controls.Action.Down.performed += ctx => JumpPlatform();

    }

    void Start ()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update ()
    {

        if (IsGrounded())
        {
            anim.SetBool("Grounded", true);
        }
        else
        {
            runSound.enabled = false;
            anim.SetBool("Grounded", false);
        }

        anim.SetFloat("AirSpeedY", rb.velocity.y);

        // Tăng bộ đếm thời gian để kiểm tra thời lượng lăn
        if (rolling)
        {
            rollCurrentTime += Time.deltaTime;
        }

        // Tắt tính năng lăn nếu bộ hẹn giờ kéo dài thời lượng
        if (rollCurrentTime > rollDuration)
        {
            rolling = false;
        }

        // Tăng bộ đếm thời gian điều khiển combo tấn công
        timeSinceAttack += Time.deltaTime;

        // Flip
        if (inputX > 0 && !facingRight || inputX < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        Move();
        WallSliding();
        UnWallSliding();
    }
    private void Move()
    {

        if (!rolling && !isShield)
        {
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        }

        if (Mathf.Abs(inputX) > 0)
        {
            anim.SetInteger("AnimState", 1);
            if (IsGrounded() && rolling == false)
            {
                runSound.enabled = true;
            }
        }
        else
        {
            anim.SetInteger("AnimState", 0);
            runSound.enabled = false;
        }
    }
    private void Attack() 
    {
        if (timeSinceAttack > 0.25f && !rolling && !isShield)
        {
            rb.velocity = Vector2.zero;
            currentAttack++;

            //Lặp lại lần sau lần tấn công thứ ba
            if (currentAttack > 3)
            {
                currentAttack = 1;
            }

            // Đặt lại combo tấn công nếu thời gian kể từ lần tấn công cuối cùng quá lớn
            if (timeSinceAttack > 1.0f)
            {
                currentAttack = 1;
            }

            // Gọi một trong ba hoạt ảnh tấn công "Attack1", "Attack2", "Attack3"
            anim.SetTrigger("Attack" + currentAttack);

            // Đặt lại bộ hẹn giờ
            timeSinceAttack = 0.0f;
            if(currentAttack == 1)
            {
                audioManager.PlaySFX(audioManager.attack1);
            }
            else if(currentAttack == 2)
            {
                audioManager.PlaySFX(audioManager.attack2);
            }
            else
            {
                audioManager.PlaySFX(audioManager.attack3);
            }

        }
    }
    private void Jump()
    {
        if (IsGrounded() && !rolling)
        {
            anim.SetTrigger("Jump");
            audioManager.PlaySFX(audioManager.jump);
            anim.SetBool("Grounded", false);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            SpawnDustEffect(jump);
        }

        WallJump();
    }
    private void Roll()
    {
        if (!rolling && !IsWalled())
        {
            rolling = true;
            Rolling();
            anim.SetTrigger("Roll");
            audioManager.PlaySFX(audioManager.roll);
            rb.velocity = new Vector2(inputX * rollForce, rb.velocity.y);
        }
    }
    private void Rolling()
    {
        playerIdle.enabled = false;
        playerJump.enabled = false;
        playerRoll.enabled = true;
    }
    private void Jumping()
    {
        playerIdle.enabled = false;
        playerJump.enabled = true;
        playerRoll.enabled = false;
    }
    private void Idle()
    {
        playerIdle.enabled = true;
        playerJump.enabled = false;
        playerRoll.enabled = false;
    }
    private void UnWallSliding()
    {
        if (IsGrounded()|| !IsWalled())
        {
            isWallSliding = false;
            anim.SetBool("WallSlide", isWallSliding);
        }
    }
    public void Shield()
    {
        if (!rolling)
        {
            rb.velocity = Vector2.zero;
            isShield = true;
            ereaShield.enabled = true;
            anim.SetTrigger("Block");
            audioManager.PlaySFX(audioManager.block);
            anim.SetBool("IdleBlock", true);
            gameObject.tag = "Shield";
        }
    }
    public void UnShield()
    {
        isShield = false;
        ereaShield.enabled = false;
        anim.SetBool("IdleBlock", false);
        gameObject.tag = "Player";
    }
    public void SetMoving(float inputX)
    {
        this.inputX = inputX;
    }
    private void JumpPlatform()
    {
        if(!rolling && IsGrounded())
        {
            if(currentOnWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }
    private void WallSliding()
    {
        if (IsWalled() && !IsGrounded())
        {
            isWallSliding = true;
            anim.SetBool("WallSlide", isWallSliding);
            rb.velocity = new Vector2(0f, -wallSlideSpeed);
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallSliding = false;
            anim.SetBool("WallSlide", isWallSliding);
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (wallJumpingCounter > 0f)
        {
            rb.velocity = new Vector2(wallJumpingDirection * 3.5f, 8f);
            anim.SetTrigger("Jump");
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                facingRight = !facingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
            else
            {
                isWallSliding = false;
                anim.SetBool("WallSlide", isWallSliding);
            }
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(1.3f, 0.015f), 0.1f, groundLayerMask);
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapBox(wallCheck.position, new Vector2(0.02f, 1.8f), 0.5f, wallLayerMask);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentOnWayPlatform = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("Elevator"))
        {
            transform.parent = collision.gameObject.transform;
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            healthHero.RestoreHealth(20);
            audioManager.PlaySFX(audioManager.itemHealth);
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            currentOnWayPlatform = null;
        }

        if (collision.gameObject.CompareTag("Elevator"))
        {
            transform.parent = null;
        }
    }
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOnWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerIdle, platformCollider);
        yield return new WaitForSeconds(0.8f);
        Physics2D.IgnoreCollision(playerIdle, platformCollider, false);
    }
    private bool EnemyInSight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(ereaAttack.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(ereaAttack.bounds.size.x * range, ereaAttack.bounds.size.y, ereaAttack.bounds.size.z), 0, Vector2.left, 0, enemyLayer);

        if (hit.collider != null)
        {
            healthEnemy = hit.transform.GetComponent<HealthEnemy>();
        }

        return hit.collider != null;
    }
    private void DamageEnemy()
    {
        if (EnemyInSight())
        {
            healthEnemy.TakeDamage(damage= Random.Range(10, 15));
        }
    }

    void SpawnDustEffect(GameObject dust, float dustXOffset = 0)
    {
        if (dust != null)
        {
            // Đặt vị trí sing ra hiệu ứng
            Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * inputX, 0.0f, 0.0f);
            GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
            newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(1, 1, 1);
        }
    }

    void AE_Landing()
    {
        audioManager.PlaySFX(audioManager.landing);
        SpawnDustEffect(landing);
    }
}
