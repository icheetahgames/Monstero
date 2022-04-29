using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class Enemy : GeneralMovementAttack
{ // this class deals with the movement of the enemy
    
    
    
        
    #region Public Variables
    #endregion
//----------------------------------------------------------------------------------------------- 
    #region SerializeField variables

    #endregion
//-----------------------------------------------------------------------------------------------
    #region Cashed variables(components)
    private Animator anim;
    private NavMeshAgent _navMeshAgent;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Class Variables(private-Only related to this class)    
    private EnemyHealth _enemyHealth;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Getters & Setters functions
    #endregion
//----------------------------------------------------------------------------------------------- 


    private void Start()
    {
        #region Cahsed Variables Definition
        _enemyHealth = GetComponent<EnemyHealth>();
        _navMeshAgent =GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        #endregion
        
        //player = GameManager.Instance.Player.transform;

    }
//----------------------------------------------------------------------------------------------- 
    // Update is called once per frame
      void Update()
      {
          FollowPlayer();
      }
//----------------------------------------------------------------------------------------------- 

    protected  void FollowPlayer()
    {
        if (!GameManager.Instance.GameOver && _enemyHealth.IsAlive)
        {
            _navMeshAgent.SetDestination(GetClosestEnemy(GameManager.Instance.Allies).position);
        }
        else if (!_enemyHealth.IsAlive)
        {//In case of dead enemy
            DontFollowPlayer();
        }

        else
        {//Game Over Case
            DontFollowPlayer();
            anim.Play("Idle"); 
        }
    }
//----------------------------------------------------------------------------------------------- 

    void DontFollowPlayer()
    {
        _navMeshAgent.enabled = false;
    }
//-----------------------------------------------------------------------------------------------     

}
