using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianLvl9 : MonoBehaviour
{
    public Transform groundDetector;
    public Transform eyeView;

    private Animator guardianAnim;
    
    public AudioSource runSfx;

    Vector2 curDir = Vector2.right;

    public float speed = 1;
    public float isStop = 1;

    public bool thereIsPlayer;

    private ResultGame resultGame;

    private void Awake()
    {
        resultGame = GameObject.Find("GameManager").GetComponent<ResultGame>();
        guardianAnim = gameObject.GetComponent<Animator>();

        guardianAnim.SetTrigger("walk");
    }

    private void FixedUpdate()
    {
        if (!resultGame.isPause)
        {
            RaycastHit2D isTriggered = Physics2D.Raycast(eyeView.position, curDir, 35f);
            RaycastHit2D isGround = Physics2D.Raycast(groundDetector.position, Vector2.down, 1f);

            if (isGround.collider == null && !thereIsPlayer)
            {
                isStop = 0;
                guardianAnim.SetTrigger("idle");
            }else if (isTriggered.collider != null)
            {
                if (isTriggered.collider.CompareTag("Player"))
                {
                    guardianAnim.SetTrigger("run");
                    thereIsPlayer = true;
                }
                else
                {
                    thereIsPlayer = false;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Obstacle"))
        {
            isStop = 0;
            guardianAnim.SetTrigger("idle");
        }
    }
    
    public void Flip()
    {
        curDir = -curDir;
        isStop = 1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        speed *= -1;
        transform.localScale = scale;
    }
}
