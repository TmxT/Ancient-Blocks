using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class BlocksManager : MonoBehaviour
{
    public Sprite creckedSprite;

    private float timeLeft = 0;

    private int blockCrack = 3;

    private RandomizedBlock randomizedBlock;

    private void Start()
    {
        randomizedBlock = GameObject.Find("GameManager").GetComponent<RandomizedBlock>();
        if (gameObject.transform.tag == "Obstacle")
        {
            timeLeft = 3;
        }
    }

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }

        Vector3 eulerRotation = transform.rotation.eulerAngles;

        if ((Input.GetKeyDown(KeyCode.RightArrow) || randomizedBlock.rotateLeft) && timeLeft > 0)
        {
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z + 90);
            randomizedBlock.rotateLeft = false;
        }
        else if ((Input.GetKeyDown(KeyCode.LeftArrow) || randomizedBlock.rotateRight) && timeLeft > 0)
        {
            transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z - 90);
            randomizedBlock.rotateRight = false;
        }
    }
    
    public void BlockCracking()
    {
        blockCrack--;

        if (blockCrack <= 0)
        {
            Destroy(gameObject);
        }else if (blockCrack == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = creckedSprite;
        }
        else if (blockCrack == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(150, 150, 150, 255);
        }
    }

    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Boss") || collision.collider.CompareTag("Poison"))
        {
            yield return new WaitForSeconds(.2f);
            BlockCracking();
            yield return new WaitForSeconds(.2f);
            BlockCracking();
            yield return new WaitForSeconds(.2f);
            BlockCracking();
        }

        if (collision.collider.CompareTag("Mortar"))
        {
            collision.transform.GetComponent<MortarAI>().TakeDamage(3);
        }

        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Obstacle"))
        {
            timeLeft = 0;
        }
    }
}
