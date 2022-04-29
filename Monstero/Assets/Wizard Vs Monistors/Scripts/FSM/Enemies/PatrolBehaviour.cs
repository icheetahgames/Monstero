using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrolBehaviour : StateMachineBehaviour
{


    private int randomSpot;
    private float distance;
    private float _distanceToSpot;

    [SerializeField] private float _distanceToFollow = 10f ;

    [SerializeField] private float _patrollingSpeed = 2f;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        randomSpot = Random.Range(0, animator.GetComponent<SpotsLinker>().Spots.Length);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        distance = Vector2.Distance(new Vector2(animator.transform.position.x, animator.transform.position.z), 
            new Vector2(animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position.x, 
                animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position.z));
        
        if (distance <= _distanceToFollow && !GameManager.Instance.GameOver) // Alley in range
        {
            animator.SetBool("isFollowing", true);
            return;
        }
        else if (animator.GetComponent<SpotsLinker>().Spots.Length <= 0)
        {// this is a problem, where I got no spots at all some times
            return;
        }
        else // Pattroling
        {
            animator.SetBool("isFollowing", false);
            animator.GetComponent<NavMeshAgent>().speed = _patrollingSpeed;
            animator.GetComponent<NavMeshAgent>()
                .SetDestination(animator.GetComponent<SpotsLinker>().Spots[randomSpot].position);


            _distanceToSpot = Vector3.Distance(animator.transform.position,
                animator.GetComponent<SpotsLinker>().Spots[randomSpot].position);
            if (_distanceToSpot <= animator.GetComponent<NavMeshAgent>().stoppingDistance)
            {
                animator.SetBool("isPatrolling", false);
            }
        }


        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        randomSpot = Random.Range(0, animator.GetComponent<SpotsLinker>().Spots.Length);
    }

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
