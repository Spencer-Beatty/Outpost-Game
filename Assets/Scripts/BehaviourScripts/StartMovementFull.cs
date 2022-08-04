using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMovementFull : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    /*override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
     {
         animator.gameObject.GetComponent<Unit>().grabMovementFlag();

     }*/


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state

    //OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       

        //These resets should be based on the light attack and heavy stagger triggers
        animator.ResetTrigger("stagger");
        animator.ResetTrigger("slash");
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
