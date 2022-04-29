using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class BotIdleBehaviour : StateMachineBehaviour
{
    [SerializeField] private float timer = 2;
    private float _distanceToFollowEnemy;
    [SerializeField] private float _distanceToStopFollowingPlayer = 3f;

    private float time = 0;

    private float _playerDistance;

    private float _enemyDistance;

    private int _enemiesOnTheGround;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _distanceToFollowEnemy = animator.GetComponent<BotCommonMethods>()._distanceToFollowEnemy;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerDistance = Vector2.Distance(new Vector2(animator.transform.position.x, animator.transform.position.z), 
            new Vector2(GameManager.Instance.Player.transform.position.x, 
                GameManager.Instance.Player.transform.position.x));
        if (GameManager.Instance.EnemiesOnTheGround.Count > 0)
        {
            _enemyDistance = Vector2.Distance(new Vector2(animator.transform.position.x, animator.transform.position.z), 
                new Vector2(animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator, GameManager.Instance.EnemiesOnTheGround).position.x, animator.GetComponent<BotCommonMethods>().GetClosestEnemy(animator, GameManager.Instance.EnemiesOnTheGround).position.z));
        }


        // if (_playerDistance > animator.GetComponent<NavMeshAgent>().stoppingDistance + 0.1f)
        // { //Follow Alleyssss
        //     animator.SetBool("FollowPlayer", true);
        // }
        // else  if (_enemyDistance > animator.GetComponent<NavMeshAgent>().stoppingDistance + 2f && GameManager.Instance.EnemiesOnTheGround.Count > 0)
        // {
        //     animator.SetBool("FollowEnemy", true);
        // }
        // else
        // {
        //     animator.SetBool("FollowPlayer", false);
        //     animator.SetBool("FollowEnemy", false);
        // }

        Debug.Log("_enemiesOnTheGround : " +  animator.transform.Find("/Levels/").GetChild(GameManager.Instance.Level).Find("Enemies On The Ground").childCount );
        if (_enemyDistance < _distanceToFollowEnemy  && !GameManager.Instance.NextLevel && animator.transform.Find("/Levels/").GetChild(GameManager.Instance.Level).Find("Enemies On The Ground").childCount > 0)
        {
            animator.SetBool("FollowEnemy", true);
        }
        else if (!GameManager.Instance.NextLevel)
        {
            animator.SetBool("FollowPlayer", true);
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
