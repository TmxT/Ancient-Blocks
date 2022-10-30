using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarAI : MonoBehaviour
{
    public GameObject bullet;

    public bool isLeft;

    private float fire;

    private int destroyed = 2;

    public Transform bulletPos;
    
    private Vector3 posisiton;

    private AudioSource fireSfx;

    private ResultGame resultGame;

    private void Start()
    {
        resultGame = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ResultGame>();

        fireSfx = gameObject.GetComponent<AudioSource>();

        bullet.GetComponent<BulletAI>().isLeft = isLeft;

        fire = Random.Range(3f, 25f);
        InvokeRepeating("Fire", fire, fire);
    }

    private void Fire()
    {
        if (!resultGame.isPause)
        {
            posisiton = new Vector3(bulletPos.position.x, bulletPos.position.y, 0);

            fireSfx.Play();

            Instantiate(bullet, posisiton, Quaternion.identity);
        }
    }
    
    public void TakeDamage(int _damage)
    {
        destroyed -= _damage;

        if (destroyed <= 0)
        {
            Destroy(gameObject);
        }
    }
}