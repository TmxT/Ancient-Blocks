using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLvl6AI : MonoBehaviour
{
    public GameObject breathCollider;

    public Transform detectorPlayer;

    public AudioSource laserWarningAudioSource;

    private Animator bossSkillAnim;

    private bool battle = false;

    [SerializeField] private float atk = 10, delay = 3, raged = 0;
    [SerializeField] private int hp;
    
    private ResultGame resultGame;
    private HealthBarManager healthBarManager;
    [SerializeField] private Player player;

    private void Start()
    {
        bossSkillAnim = gameObject.GetComponent<Animator>();

        resultGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResultGame>();
        healthBarManager = GameObject.FindGameObjectWithTag("BossHP").GetComponent<HealthBarManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D isTriggered = Physics2D.Raycast(detectorPlayer.position, Vector2.left, 15f);

        if (isTriggered.collider != null)
        {
            if ((isTriggered.collider.transform.CompareTag("Player") || isTriggered.collider.transform.CompareTag("Obstacle")) && !battle)
            {
                battle = true;
                hp = 100;

                healthBarManager.ShowHealthBar(hp / 100);
            }
        }

        if (battle && !resultGame.isPause)
        {
            atk -= Time.fixedDeltaTime;
            delay -= Time.fixedDeltaTime;

            if (delay <= 0 && atk <= 0)
            {
                Collider2D colInfo = Physics2D.OverlapCircle(detectorPlayer.position, 1f);

                if (colInfo != null)
                {
                    atk = 10 - raged;
                    delay = 3;

                    bossSkillAnim.SetTrigger("atk");
                    StartCoroutine(ColliderSkillOn());
                }
            }
        }
    }

    public void TakeDamage(int _damage)
    {
        hp -= _damage;

        if (hp == 50)
        {
            raged = 6;
        }
        else if (hp <= 0)
        {
            StartCoroutine(Death());
        }

        healthBarManager.Damaged(hp);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            TakeDamage(15);
        }
    }

    public IEnumerator ColliderSkillOn()
    {
        yield return new WaitForSeconds(bossSkillAnim.runtimeAnimatorController.animationClips.Length * .3f);
        breathCollider.SetActive(true);
    }

    private IEnumerator Death()
    {
        delay = 100;
        bossSkillAnim.SetTrigger("die");

        yield return new WaitForSeconds(bossSkillAnim.runtimeAnimatorController.animationClips.Length - 1f);

        Destroy(gameObject);
    }
}
