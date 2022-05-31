using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField] private float timer = 2;
    [FormerlySerializedAs("_followDis")] [SerializeField] private float _distanceToFollow = 10f;

    private float time = 0;

    private float distance;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (GameManager.Instance.Allies.Count > 0)
        {
            
        
            distance = Vector2.Distance(new Vector2(animator.transform.position.x, animator.transform.position.z), 
                new Vector2(animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position.x, 
                    animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position.z));
            if (distance <= _distanceToFollow)
            { //Follow Alley
                animator.SetBool("isFollowing", true);
            }
        }
        if (time >= timer)
        { // Move to New patrolling spot is there is no alley and the timer is more than timer
            animator.SetBool("isPatrolling", true);
            time = 0;
        }
        time += Time.deltaTime;
    

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
}
