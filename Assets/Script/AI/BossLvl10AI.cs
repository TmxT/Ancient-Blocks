using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLvl10AI : MonoBehaviour
{
    public BoxCollider2D weakness;

    public Transform detectorPlayer;
    public Transform attackPos;
    private Transform playerPos;

    public AudioSource swordSfx;
    public AudioSource runSfx;

    private new Rigidbody2D rigidbody2D;

    private Animator bossAnim;

    private bool battle = false;
    private bool isFliped = false;
    [HideInInspector]public bool isJump = false;
    private int isRun = 0;
    
    public float atkCooldown = 10f, jumpCooldown = 3f, safeRange = 3f, followRange = 5f, attackRange = 1f, idleRange = 10f, speed = 15f;
    private float distance;

    [SerializeField] private int hp;

    private ResultGame resultGame;
    private HealthBarManager healthBarManager;
    private Controller2D controller2D;
    [SerializeField] private Player player;

    private void Start()
    {
        bossAnim = gameObject.GetComponent<Animator>();

        resultGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResultGame>();
        healthBarManager = GameObject.FindGameObjectWithTag("BossHP").GetComponent<HealthBarManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerPos = player.transform;
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        controller2D = gameObject.GetComponent<Controller2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D isTriggered = Physics2D.Raycast(detectorPlayer.position, Vector2.left, 50f);

        if (isTriggered.collider != null)
        {
            if (isTriggered.collider.transform.CompareTag("Player") && !battle)
            {
                weakness.isTrigger = true;
                battle = true;
                hp = 300;

                healthBarManager.ShowHealthBar(hp / 100);
            }
        }

        if (battle && !resultGame.isPause)
        {
            distance = Vector2.Distance(playerPos.position, rigidbody2D.position);

            LookAtPlayer();

            atkCooldown -= Time.fixedDeltaTime;
            jumpCooldown -= Time.fixedDeltaTime;

            if ((atkCooldown < 0 && distance > attackRange) || distance >= followRange)
            {
                bossAnim.SetTrigger("run");
                isRun = 1;
            }
            else if (distance < safeRange && jumpCooldown <= 0 && atkCooldown > 0)
            {
                bossAnim.SetTrigger("jump");
                jumpCooldown = 3;
            }
            else if (distance <= idleRange)
            {
                if (transform.position.y < playerPos.position.y && jumpCooldown <= 0)
                {
                    if (atkCooldown <= 0 && distance < attackRange)
                    {
                        bossAnim.SetTrigger("jumpAtk");
                        swordSfx.Play();
                        atkCooldown = 10;
                        jumpCooldown = 3;
                        isJump = true;
                    }
                    else
                    {
                        bossAnim.SetTrigger("jump");
                        jumpCooldown = 3;
                        isJump = true;
                    }
                }
                else if (atkCooldown <= 0 && distance < attackRange)
                {
                    bossAnim.SetTrigger("atk");
                    swordSfx.Play();
                    atkCooldown = 10;
                    jumpCooldown = 3;
                }
                else
                {
                    bossAnim.SetTrigger("idle");
                    isRun = 0;
                }
            }

            controller2D.Move(isRun * speed * Time.fixedDeltaTime, false, isJump);
            /*if (isRun)
            {
                controller2D.Move(speed * Time.fixedDeltaTime, false, isJump);
            }
            else
            {
                controller2D.Move(0, false, isJump);
            }*/

            isJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle") && jumpCooldown <= 0)
        {
            bossAnim.SetTrigger("jump");
            jumpCooldown = 3;
            isJump = true;
        }
    }

    private void LookAtPlayer()
    {
        if (transform.position.x > playerPos.position.x && !isFliped)
        {
            speed *= -1;
            isFliped = true;
            transform.localScale = new Vector3(-10, 10, 0);
        }
        else if (transform.position.x < playerPos.position.x && isFliped)
        {
            speed *= -1;
            isFliped = false;
            transform.localScale = new Vector3(10, 10, 0);
        }
    }

    public void Attack()
    {
        Collider2D colInfo = Physics2D.OverlapCircle(attackPos.position, 2f);

        if (colInfo != null)
        {
            StartCoroutine(player.Death());
        }
    }

    public void TakeDamage(int _damage)
    {
        hp -= _damage;

        if (hp <= 0)
        {
            StartCoroutine(Death());
        }
        else
        {
            bossAnim.SetTrigger("hurt");
        }

        healthBarManager.Damaged(hp % 100);
    }

    private IEnumerator Death()
    {
        bossAnim.SetTrigger("die");

        yield return new WaitForSeconds(bossAnim.runtimeAnimatorController.animationClips.Length - 1f);

        Destroy(gameObject);
    }
}
