using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotAttackBehaviour : StateMachineBehaviour
{
    private float _distance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!GameManager.Instance.GameOver && animator.GetComponent<BotHealth>().IsAlive && GameManager.Instance.EnemiesOnTheGround.Count > 0)
        {
            _distance = Vector2.Distance(new Vector2(animator.transform.position.x, animator.transform.position.z), 
                new Vector2(animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator, GameManager.Instance.EnemiesOnTheGround).position.x, animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator, GameManager.Instance.EnemiesOnTheGround).position.z));

            if (_distance > animator.GetComponent<NavMeshAgent>().stoppingDistance +0.1f)
            { // In case of the closest enemy is died
                animator.SetBool("enemyInAttackRange", false);
            }
            animator.GetComponent<BotCommonMethods>().RotateTowards(animator, animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator,GameManager.Instance.EnemiesOnTheGround), 10f);
        }
        else if (GameManager.Instance.EnemiesOnTheGround.Count <= 0)
        {
            animator.SetBool("enemyInAttackRange", false);
        }

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
