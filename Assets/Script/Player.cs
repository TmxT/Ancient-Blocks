using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform attackPos;

    [SerializeField] private Rigidbody2D rb2D;

    [SerializeField] private Animator characterAnimation;

    private float move;

    private bool jump = false;
    [SerializeField] private bool isGround = false;
    public bool IsAlive { get; protected set; }
    public bool isAttacking;
    private bool isFinish = false;
    [HideInInspector]public bool isKeyboardDevice;

    public float speed = 20;

    private int level;

    public Controller2D controller2D;
    [SerializeField] private ResultGame resultGame;
    [SerializeField] private CameraFollow cameraFollow;

    public AudioSource killingSfx;
    public AudioSource deathSfx;
    public AudioSource swordSfx;
    public AudioSource walkSfx;

    private void Start()
    {
        resultGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResultGame>();
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        characterAnimation = gameObject.GetComponent<Animator>();
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        cameraFollow.Freeze = false;
        IsAlive = true;

        level = resultGame.thisLevel;
    }
    
    private void FixedUpdate()
    {
        if (isKeyboardDevice)
        {
            if (Input.GetKey(KeyCode.W) && isGround)
            {
                isGround = false;
                jump = true;

                characterAnimation.SetTrigger("jump");
            }

            if (Input.GetKey(KeyCode.A))
            {
                BtnMoveLeftDown();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                BtnMoveRightDown();
            }
            else
            {
                move = 0;
                characterAnimation.SetTrigger("idle");
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && !isAttacking && level >= 3)
            {
                characterAnimation.SetTrigger("attack");
                swordSfx.Play();
            }
        }

        if (!IsAlive)
        {
            move = 0;
            characterAnimation.SetTrigger("death");
        }

        if (move == 0 && !jump)
        {
            characterAnimation.SetTrigger("idle");
        }
        else if ((move > 0 || move < 0) && !jump)
        {
            characterAnimation.SetTrigger("walk");
        }

        controller2D.Move(move * Time.fixedDeltaTime, false, jump);

        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsAlive)
        {
            if (collision.collider.CompareTag("Weakness"))
            {
                killingSfx.Play();
            }
            
            if (collision.transform.CompareTag("Poison") || collision.collider.CompareTag("Enemy"))
            {
                StartCoroutine(Death());
            }

            if (collision.transform.CompareTag("Ground") || collision.transform.CompareTag("Mortar") || collision.transform.CompareTag("Obstacle"))
            {
                isGround = true;
            }

            if (collision.transform.CompareTag("Finish") && !isFinish)
            {
                resultGame.Result();
                isFinish = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("BossSkill") && !isAttacking)
        {
            StartCoroutine(Death());
        }
        
        if (collision.transform.CompareTag("Fog"))
        {
            speed = 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Fog"))
        {
            speed = 20;
        }
    }

    public IEnumerator Death()
    {
        deathSfx.Play();

        cameraFollow.Freeze = true;
        IsAlive = false;

        resultGame.isPause = true;
        resultGame.killed = resultGame.enemy;
        resultGame.sec = 0;
        resultGame.min = 0;

        yield return new WaitForSeconds(2f);
        resultGame.Result();

        yield return new WaitForSeconds(2f);
        rb2D.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    
    public void Attack()
    {
        Collider2D colInfo = Physics2D.OverlapCircle(attackPos.position, .1f);

        if (colInfo != null)
        {
            if (colInfo.CompareTag("Mortar"))
            {
                colInfo.GetComponent<MortarAI>().TakeDamage(1);
            }

            if (colInfo.CompareTag("Boss"))
            {
                try
                {
                    if (level == 3)
                    {
                        colInfo.GetComponent<BossLvl3AI>().TakeDamage(10);
                    }else if (level == 6)
                    {
                        colInfo.GetComponent<BossLvl6AI>().TakeDamage(5);
                    }else if (level == 10)
                    {
                        colInfo.GetComponent<BossLvl10AI>().TakeDamage(5);
                    }
                }
                catch { }
            }
            else if (colInfo.CompareTag("Obstacle"))
            {
                try
                {
                    colInfo.GetComponent<BlocksManager>().BlockCracking();
                }
                catch { }
            }
        }
    }

    public void BtnAttack()
    {
        if (!isAttacking)
        {
            characterAnimation.SetTrigger("attack");
            swordSfx.Play();
        }
    }

    public void BtnJump()
    {
        if (isGround)
        {
            jump = true;
            isGround = false;
            characterAnimation.SetTrigger("jump");
        }
    }

    public void BtnMoveLeftDown()
    {
        move = speed * -1;
    }
    public void BtnMoveRightDown()
    {
        move = speed * 1;
    }
    public void BtnMoveLeftUp()
    {
        move = 0;
    }
    public void BtnMoveRightUp()
    {
        move = 0;
    }
}
