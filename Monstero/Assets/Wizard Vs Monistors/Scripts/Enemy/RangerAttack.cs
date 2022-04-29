using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangerAttack : GeneralMovementAttack
{
    
    
    
    #region Public Variables
    #endregion
//----------------------------------------------------------------------------------------------- 
    #region SerializeField variables
    [SerializeField] private float _range;
    [SerializeField] private float _timeBetweenAttacks = 1f;
    [SerializeField] private Transform _fireLocation;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Cashed variables(components)
    private NavMeshAgent _nav;
    private Animator _anim;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Class Variables(private-Only related to this class)    
    private GameObject _player;
    private bool _playerInRange;
    private GameObject _arrow;
    private EnemyHealth _enemyHealth;
    private GameObject _arrowTothrow;
    //private BoxCollider[] weaponColliders; // used only with enemis have weapons
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
        _nav = GetComponent<NavMeshAgent>();

        #endregion
        
        _arrow = GameManager.Instance.Arrow;
        _player = GameManager.Instance.Player;
        
        
        _range = _nav.stoppingDistance;
        
        
    }
//----------------------------------------------------------------------------------------------- ]
private void OnEnable()
{
    StartCoroutine(Attack()); // On enable is used to invoke this coroutine since start method is started with game start even if the enemy is not activated, therefore the attack will not work.
}
//----------------------------------------------------------------------------------------------- 

    // Update is called once per frame
    void Update()
    {
        PlayerRangeCheck();
        
    }
//----------------------------------------------------------------------------------------------- 
    private void PlayerRangeCheck()
    {
        if (Vector3.Distance(transform.position,GetClosestEnemy(GameManager.Instance.Allies).position) < _range  && _enemyHealth.IsAlive)
        {
            _playerInRange = true;
            _anim.SetBool("PlayerInRange", true);
            RotateTowards(GetClosestEnemy(GameManager.Instance.Allies));
        }
        else
        {
            _playerInRange = false;            
            _anim.SetBool("PlayerInRange", false);

        }
    }
//----------------------------------------------------------------------------------------------- 

    IEnumerator Attack()
    {
        if (_playerInRange && !GameManager.Instance.GameOver)
        {
            _anim.Play("Arrow Attack");
            yield return new WaitForSeconds(_timeBetweenAttacks);
        }

        yield return null;
        StartCoroutine(Attack());
    }


//----------------------------------------------------------------------------------------------- 

    public void FireArrow()
    {
        {
//        GameObject newArrow = Instantiate(arrow) as GameObject;
//        newArrow.transform.position = fireLocation.position;
//        newArrow.transform.rotation = transform.rotation;
//        newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 25f;


            // Object pooling
            for (int i = 0; i < GameManager.Instance.ArrowList.Count; i++)
            {
                if (!GameManager.Instance.ArrowList[i].activeInHierarchy)
                {
                    _arrowTothrow = GameManager.Instance.ArrowList[i];
                    _arrowTothrow.transform.position = _fireLocation.position;
                    _arrowTothrow.transform.rotation = transform.rotation;
                    _arrowTothrow.SetActive(true);
                    Rigidbody temRigidBodyBullet = _arrowTothrow.GetComponent<Rigidbody>();
                    temRigidBodyBullet.GetComponent<Rigidbody>().velocity = transform.forward * 25f;
                    break;
                }
            }
        }
    }
//----------------------------------------------------------------------------------------------- 
    
  }
