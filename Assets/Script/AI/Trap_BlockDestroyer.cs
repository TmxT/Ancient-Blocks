using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_BlockDestroyer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            collision.transform.GetComponent<BlocksManager>().BlockCracking();
            collision.transform.GetComponent<BlocksManager>().BlockCracking();
            collision.transform.GetComponent<BlocksManager>().BlockCracking();
        }
    }
}
