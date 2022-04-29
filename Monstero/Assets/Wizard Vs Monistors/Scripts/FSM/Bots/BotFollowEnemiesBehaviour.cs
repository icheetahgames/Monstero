using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotFollowEnemiesBehaviour : StateMachineBehaviour
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
        if (!GameManager.Instance.GameOver && animator.GetComponent<BotHealth>().IsAlive && animator.transform.Find("/Levels/").GetChild(GameManager.Instance.Level).Find("Enemies On The Ground").childCount > 0 && GameManager.Instance.EnemiesOnTheGround.Count > 0)
        {
            _distance = Vector2.Distance(new Vector2(animator.transform.position.x, animator.transform.position.z), 
                new Vector2(animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator, GameManager.Instance.EnemiesOnTheGround).position.x, animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator, GameManager.Instance.EnemiesOnTheGround).position.z));

            if (_distance <= animator.GetComponent<NavMeshAgent>().stoppingDistance)
            {
                animator.SetBool("enemyInAttackRange", true);
            }
            animator.GetComponent<NavMeshAgent>()
                .SetDestination(animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator,GameManager.Instance.EnemiesOnTheGround).position);

            RotateTowards(animator, animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator,GameManager.Instance.EnemiesOnTheGround));
        }
        else if (animator.transform.Find("/Levels/").GetChild(GameManager.Instance.Level).Find("Enemies On The Ground").childCount <= 0)
        {
            animator.SetBool("FollowEnemy", false);
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

    #region variables reated to next function
    private Vector3 _direction;
    private Quaternion _lookRotation;
    #endregion
    
    private void RotateTowards(Animator animator, Transform target)
    {
        if (!GameManager.Instance.GameOver && target != null)
        {            
            _direction = (target.position - animator.transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, _lookRotation, Time.deltaTime * 10f); 
        }
        
    }
}
