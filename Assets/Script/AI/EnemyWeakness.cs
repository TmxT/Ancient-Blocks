using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakness : MonoBehaviour
{
    private Animator anim;
    
    public bool isCounted = false;
    private bool isDie = false;

    private ResultGame resultGame;

    void Start()
    {
        anim = transform.GetComponentInParent<Animator>();
        resultGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResultGame>();
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.collider.CompareTag("Player") || collision.collider.CompareTag("Obstacle")) && !isDie)
        {
            isDie = true;

            if (isCounted)
            {
                resultGame.killed--;
                transform.parent.tag = "Untagged";
                transform.parent.GetComponent<Collider2D>().isTrigger = true;
            }

            anim.SetTrigger("die");
            yield return new WaitForSeconds(.2f);
            Destroy(transform.parent.gameObject);
        }
    }       
}
