using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationButton : MonoBehaviour
{
    public Animator btnWalkLeft;
    public Animator btnWalkRight;

    public Animator btnRotateLeft;
    public Animator btnRotateRight;

    public Animator btnJump;
    public Animator btnSpawn;
    public Animator btnAttack;

    public AudioSource clickSfx;

    public void BtnWalkLeft_Down()
    {
        Clicked();
        btnWalkLeft.SetTrigger("down");
    }

    public void BtnWalkLeft_Up()
    {
        btnWalkLeft.SetTrigger("up");
    }
    public void BtnWalkRight_Down()
    {
        Clicked();
        btnWalkRight.SetTrigger("down");
    }

    public void BtnWalkRight_Up()
    {
        btnWalkRight.SetTrigger("up");
    }

    public void BtnRotateLeft()
    {
        Clicked();
        btnRotateLeft.SetTrigger("down");
    }

    public void BtnRotateRight()
    {
        Clicked();
        btnRotateRight.SetTrigger("down");
    }

    public void BtnJump()
    {
        Clicked();
        btnJump.SetTrigger("down");
    }

    public void BtnSpawn()
    {
        Clicked();
        btnSpawn.SetTrigger("down");
    }

    public void BtnAttack()
    {
        Clicked();
        btnAttack.SetTrigger("down");
    }

    public void BtnPause()
    {
        Clicked();
    }

    private void Clicked()
    {
        clickSfx.Play();
    }
}
