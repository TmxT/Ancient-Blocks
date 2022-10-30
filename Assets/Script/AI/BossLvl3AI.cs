using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossLvl3AI : MonoBehaviour
{
    public GameObject dashWindFX;
    public GameObject laserFX;
    public GameObject projectile;

    public Transform detectorPlayer;
    public Transform detectorPlayerAtk2;
    public Transform projectileSpawnPos;

    public AudioSource laserWarningAudioSource;
    public AudioSource laserAudioSource;

    private Vector3 spawnPos;

    private Animator bossSkillAnim;

    private RaycastHit2D hit;

    private bool battle = false;

    [SerializeField] private float atk1 = 10, atk2 = 20, laser = 45, delay = 3, def = 0;
    [SerializeField] private int hp, defTimes = 1;

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
            atk1 -= Time.fixedDeltaTime;
            atk2 -= Time.fixedDeltaTime;
            laser -= Time.fixedDeltaTime;
            delay -= Time.fixedDeltaTime;
            def -= Time.fixedDeltaTime;

            if (delay <= 0 && def <= 0)
            {
                if (laser < 5 && laser > 4.9)
                {
                    laserWarningAudioSource.Play();
                }

                if (atk1 <= 0)
                {
                    atk1 = 10;
                    delay = 3;

                    bossSkillAnim.SetTrigger("atk_1");

                    Invoke("ProjectileSpawn", bossSkillAnim.runtimeAnimatorController.animationClips.Length * .2f);
                }
                else if (atk2 <= 0)
                {
                    Collider2D colInfo = Physics2D.OverlapCircle(detectorPlayerAtk2.position, .1f);

                    if (colInfo != null)
                    {
                        atk2 = 20;
                        delay = 3;

                        StartCoroutine(Atk2Skill());
                    }
                }
                else if (laser <= 0)
                {
                    laser = 65;
                    delay = 3;

                    StartCoroutine(LaserSkill());
                }
            }
        }        
    }
    

    public void TakeDamage(int _damage)
    {
        if (def <= 0)
        {
            if (hp == 50 && defTimes > 0)
            {
                bossSkillAnim.SetTrigger("def");
                def = 30;
                Invoke("IdleAfterDef", 30);
            }
            else if (hp > 0)
            {
                hp -= _damage;
                
                if (hp <= 0)
                {
                    StartCoroutine(Death());
                }
            }
        }
        else if (hp < 100)
        {
            hp += 5;
        }

        healthBarManager.Damaged(hp);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            if (def > 0)
            {
                IdleAfterDef();
            }
            else
            {
                TakeDamage(15);
            }
        }
    }

    private void IdleAfterDef()
    {
        delay = 3;
        def = 0;
        defTimes = 0;
        bossSkillAnim.SetTrigger("idle");
    }

    private void ProjectileSpawn()
    {
        spawnPos = new Vector3(projectileSpawnPos.position.x, projectileSpawnPos.position.y, 0);

        Instantiate(projectile, spawnPos, Quaternion.identity);
    }

    private IEnumerator LaserSkill()
    {
        laserAudioSource.Play();
        laserFX.SetActive(true);
        yield return new WaitForSeconds(1f);
        laserFX.SetActive(false);
    }

    private IEnumerator Atk2Skill()
    {
        bossSkillAnim.SetTrigger("atk_2");
        yield return new WaitForSeconds(bossSkillAnim.runtimeAnimatorController.animationClips.Length * .2f);
        dashWindFX.SetActive(true);
        yield return new WaitForSeconds(.5f);
        dashWindFX.SetActive(false);
    }

    private IEnumerator Death()
    {
        delay = 100;
        bossSkillAnim.SetTrigger("die");

        yield return new WaitForSeconds(bossSkillAnim.runtimeAnimatorController.animationClips.Length - 1f);

        Destroy(gameObject);
    }
}
