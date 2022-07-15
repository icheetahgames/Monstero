using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

public class FollowBehaviour : StateMachineBehaviour
{
    [SerializeField] private float _followSpeed = 3.5f;
    private float _alleyRange;

    private float _distance;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _alleyRange = animator.GetComponent<NavMeshAgent>().stoppingDistance;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (!GameManager.Instance.GameOver 
            && animator.GetComponent<EnemyHealth>().IsAlive 
            && GameManager.Instance.Player.activeInHierarchy) // This condition if for ensuring that the player is active to follow it
        {
            animator.GetComponent<NavMeshAgent>().speed = _followSpeed;
            animator.GetComponent<NavMeshAgent>().SetDestination(animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position);
            animator.GetComponent<SpotsLinker>().RotateTowards(animator ,animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies), 10f);
            
            _distance = Vector2.Distance(new Vector2(animator.transform.position.x, animator.transform.position.z), 
                new Vector2(animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position.x, 
                    animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position.z));
            if (_distance <= _alleyRange) //Move to Attack behaviour
            {
                animator.SetBool("alleyInAttaclRange", true);
            }
        }
        else if (!animator.GetComponent<EnemyHealth>().IsAlive) // Current Monistor is dead
        {
            animator.GetComponent<NavMeshAgent>().enabled = false;
        }
        else if (GameManager.Instance.GameOver)
        {
            animator.SetBool("isFollowing", false);
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
    
    //----------------------------------------------------------------------------------------------- 
    
//----------------------------------------------------------------------------------------------- 

//----------------------------------------------------------------------------------------------- 



}
