using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTrap : MonoBehaviour
{
    private Animator trap;

    private void Start()
    {
        trap = gameObject.GetComponentInParent<Animator>();

        trap.speed = Random.Range(.2f, 3f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            trap.SetBool("isStuck", true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        trap.SetBool("isStuck", false);
    }
}
