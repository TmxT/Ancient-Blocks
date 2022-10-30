using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAI : MonoBehaviour
{
    public bool isLeft;
    
    private float speed = 20;
    private readonly float smooth = .5f;
    private float timeDestroy = 20;

    private Vector3 velocity = Vector3.zero;

    private Rigidbody2D rb2D;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(speed * Time.fixedDeltaTime * 10f, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, smooth);

        timeDestroy -= Time.fixedDeltaTime;

        if (timeDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("Boss"))
        {
            Destroy(gameObject);
        }

        if (collision.collider.CompareTag("Mortar"))
        {
            isLeft = collision.collider.GetComponent<MortarAI>().isLeft;

            if (isLeft)
            {
                speed *= -1;
                gameObject.transform.localScale = new Vector3(-.5f, .5f, 1f);
            }
        }
    }
}
