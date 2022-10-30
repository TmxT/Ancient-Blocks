using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossLvl3ProjectileAI : MonoBehaviour
{
    private AudioSource explosionAudioSource;

    private Transform playerPos;

    private Animator projectileAnim;

    private readonly float projectileSpeed = 5f;
    private readonly float projectileRotateSpeed = 100f;

    private new Rigidbody2D rigidbody;

    private ResultGame resultGame;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        resultGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResultGame>();
        projectileAnim = GetComponent<Animator>();
        explosionAudioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!resultGame.isPause)
        {
            Vector2 dir = (Vector2)playerPos.position - rigidbody.position;

            dir.Normalize();

            float rotation = Vector3.Cross(dir, transform.right).z;

            rigidbody.angularVelocity = rotation * projectileRotateSpeed;

            rigidbody.velocity = -transform.right * projectileSpeed;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            StartCoroutine(Collide());
        }

        if (collision.CompareTag("Obstacle"))
        {
            StartCoroutine(Collide());
            collision.transform.GetComponent<BlocksManager>().BlockCracking();
        }
    }

    private IEnumerator Collide()
    {
        projectileAnim.SetTrigger("collide");
        explosionAudioSource.Play();
        yield return new WaitForSeconds(projectileAnim.runtimeAnimatorController.animationClips.Length * .2f);
        Destroy(gameObject);
    }
}
