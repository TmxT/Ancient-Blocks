using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainMenuAI : MonoBehaviour
{
    public Transform groundDetector;

    private Rigidbody2D rb2D;

    private Vector3 velocity = Vector3.zero;

    private readonly float smooth = .5f;
    private float speed = 20;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        RaycastHit2D isGround = Physics2D.Raycast(groundDetector.position, Vector2.down, 1f);

        Vector3 targetVelocity = new Vector2(speed * Time.fixedDeltaTime * 10f, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, smooth);

        if (isGround.collider == null)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Flip();
        }
    }


    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        speed *= -1;
        transform.localScale = scale;
    }
}
