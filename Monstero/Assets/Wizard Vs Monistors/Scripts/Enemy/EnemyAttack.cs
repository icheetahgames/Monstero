using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : GeneralMovementAttack
{
    
    
    #region Public Variables
    #endregion
//----------------------------------------------------------------------------------------------- 
#region SerializeField variables
        
    [SerializeField] private float range;

    [SerializeField] private float timeBetweenAttacks = 1f;
    
    private BoxCollider[] weaponColliders;
    
#endregion
//-----------------------------------------------------------------------------------------------
#region Cashed variables(components)
    private Animator _anim;
    protected EnemyHealth _enemyHealth;
    protected NavMeshAgent nav;
#endregion
//-----------------------------------------------------------------------------------------------
#region Class Variables(private-Only related to this class)   
    
    protected float _navSpeed; // it is protected since it is needed in micro zombie attack script
    protected bool playerInRange;    // it is protected since it is needed in micro zombie attack script

#endregion
//-----------------------------------------------------------------------------------------------
    #region Getters & Setters functions
    #endregion
//----------------------------------------------------------------------------------------------- 


    private void Start()
    {
#region Cahsed Variables Definition
        _anim = GetComponent<Animator>();
        _enemyHealth = GetComponent<EnemyHealth>();
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
#endregion
        
        
        range = nav.stoppingDistance + 1;
        _navSpeed = nav.speed;
    }
//----------------------------------------------------------------------------------------------- 
    // Update is called once per frame
    void Update()
    {
        EnemyInRange();
        //print("PlayerInRange: " + playerInRange);
    }
//----------------------------------------------------------------------------------------------- 
    protected virtual void EnemyInRange()
    {
        if (Vector3.Distance(transform.position, GetClosestEnemy(GameManager.Instance.Allies).position) < range && _enemyHealth.IsAlive)
        {
            playerInRange = true;
            RotateTowards(GetClosestEnemy(GameManager.Instance.Allies));
        }
        else
        {
            playerInRange = false;
        }
    }
//----------------------------------------------------------------------------------------------- 
    IEnumerator Attack()
    {
        
        if (playerInRange && !GameManager.Instance.GameOver)
        {
            _anim.Play("Attack");
            yield return new WaitForSeconds(timeBetweenAttacks);
        }

        yield return null;
        StartCoroutine(Attack());
    }
//----------------------------------------------------------------------------------------------- 

     void EnemyStartAttack() //Animation Event
    {
        print("Start Attack");
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = true;
        }
    }
//----------------------------------------------------------------------------------------------- 

     void EnemyEndAttack() //Animation Event
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = false;
        }
    }
//----------------------------------------------------------------------------------------------- 
    


    void EnemyDie() //Animation Event
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = false;
        }
    }

    private void OnEnable()
    {
        
        StartCoroutine(Attack()); // On enable is used to invoke this coroutine since start method is started with game start even if the enemy is not activated, therefore the attack will not work.
    }
}
