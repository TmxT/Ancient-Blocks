using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGuardianLvl9Walk : StateMachineBehaviour
{
    private Transform groundDetector;
    private Transform guardian;

    private Rigidbody2D rb2D;

    private Vector3 velocity = Vector3.zero;

    private readonly float smooth = .5f;
    private float speed = 20;

    private GuardianLvl9 guardianLvl9;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        guardianLvl9 = animator.GetComponent<GuardianLvl9>();
        rb2D = animator.GetComponent<Rigidbody2D>();
        groundDetector = animator.GetComponent<GuardianLvl9>().groundDetector;
        guardian = animator.transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 targetVelocity = new Vector2((speed * guardianLvl9.speed * guardianLvl9.isStop) * Time.fixedDeltaTime * 10f, rb2D.velocity.y);
        rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, smooth);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
