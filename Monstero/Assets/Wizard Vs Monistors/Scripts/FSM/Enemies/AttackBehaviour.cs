using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackBehaviour : StateMachineBehaviour
{
    private float _distance;
    private float _alleyRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _alleyRange = animator.GetComponent<NavMeshAgent>().stoppingDistance;
        animator.SetFloat("AttackSpeed",animator.GetComponent<AttackSpecification>().AttackSpeed);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _distance = Vector2.Distance(new Vector2(animator.transform.position.x, animator.transform.position.z), new Vector2(animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position.x, animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies).position.z));
        if (_distance > _alleyRange || GameManager.Instance.GameOver)
        {
            animator.SetBool("alleyInAttaclRange", false);
        }
        animator.GetComponent<SpotsLinker>().RotateTowards(animator ,animator.GetComponent<SpotsLinker>().GetClosestEnemy(animator, GameManager.Instance.Allies), 10f);
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
