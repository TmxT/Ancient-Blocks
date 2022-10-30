using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleButton[] obstacleButtons;

    private Animator obstacleAnim;
    
    [SerializeField]private bool obstacleBreaked;

    private void Start()
    {
        obstacleAnim = GetComponent<Animator>();
    }

    public void Pressed()
    {
        obstacleBreaked = true;

        for (int i = 0; i < obstacleButtons.Length; i++)
        {
            if (!obstacleButtons[i].IsPressed)
            {
                obstacleBreaked = false;
                break;
            }
        }
        
        if (obstacleBreaked)
        {
            obstacleAnim.SetTrigger("breaked");
        }
    }
}
