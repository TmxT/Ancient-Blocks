using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleButton : MonoBehaviour
{
    public Sprite unpressed;
    public Sprite pressed;

    private AudioSource pressedSfx;

    private SpriteRenderer spriteRenderer;

    public bool IsPressed { get; protected set; }

    public ObstacleManager obstacleManager;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        pressedSfx = gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Obstacle"))
        {
            spriteRenderer.sprite = pressed;
            IsPressed = true;
            obstacleManager.Pressed();
            pressedSfx.Play();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        spriteRenderer.sprite = unpressed;
        IsPressed = false;
        obstacleManager.Pressed();
        pressedSfx.Play();
    }
}
