using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerBotAttack : GeneralMovementAttack
{
    #region Public Variables
    #endregion
//----------------------------------------------------------------------------------------------- 
    #region SerializeField variables
    [SerializeField] private Transform fireLocation;
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private float timeThresholdTorotateToNextEnemy = 0.2f;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Cashed variables(components)
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Class Variables(private-Only related to this class)    
    private CharacterController _cC;
    private float _closestDistance = float.MaxValue;
    private float _prevoiusDis = 0;
    private float _timer = 0;

    private Animator anim;

    private Vector3 dir;    // distance between this character and all enemies


    private List<Transform> _enemiesInRangeList = new List<Transform>();
    private Transform _pastClosestEnemy;
    private Transform _currentClosestEnemy;
    private float _closestEnemyTimer;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Getters & Setters functions
    #endregion
//----------------------------------------------------------------------------------------------- 
    private void Start()
    {
        #region Cahsed Variables Definition
        anim = GetComponent<Animator>();
        _cC = GetComponent<CharacterController>();
        #endregion

        _pastClosestEnemy = GetClosestEnemy(EnemiesInRange(GameManager.Instance.EnemiesOnTheGround));
        WeaponObjectPooling(); 

    }
//----------------------------------------------------------------------------------------------- 
    private void Update()
    {
        
        Clock();
//        print("Timer : " + _closestEnemyTimer);
        if (!GameManager.Instance.GameOver && !GameManager.Instance.IsWalking && !GameManager.Instance.SpawnBot &&  GameManager.Instance.EnemiesOnTheGround.Count > 0 && GetClosestEnemy(EnemiesInRange(GameManager.Instance.EnemiesOnTheGround)) != null && _cC.enabled)
        {
            RotateTowardsClosestEnemy();
            if (_timer > _timeBetweenAttacks)
            {
                Attack();
                _timer = 0f;
            }
        }



        _enemiesInRangeList.Clear();

    }
    
    //----------------------------------------------------------------------------------------------- 
    private void Clock()
    {
        _timer += Time.deltaTime;
        _closestEnemyTimer += Time.deltaTime;
    }
//----------------------------------------------------------------------------------------------- 



    void RotateTowardsClosestEnemy()
    {
        _currentClosestEnemy = GetClosestEnemy(EnemiesInRange(GameManager.Instance.EnemiesOnTheGround));
        if (_currentClosestEnemy != _pastClosestEnemy && _closestEnemyTimer < timeThresholdTorotateToNextEnemy )
        {
            RotateTowards(_pastClosestEnemy);

        }
        else
        {
            RotateTowards(_currentClosestEnemy);
            _pastClosestEnemy = _currentClosestEnemy;
            _closestEnemyTimer = 0f;
        }
//        print(GetClosestEnemy(EnemiesInRange(GameManager.Instance.EnemiesOnTheGround)).gameObject.name);
    }

    //----------------------------------------------------------------------------------------------- 
    #region variables reated to next function
    private Vector3 _direction;
    private Quaternion _lookRotation;
    #endregion
    
    private void RotateTowards(Transform target)
    {
        if (!GameManager.Instance.GameOver && target != null)
        {            
            _direction = (target.position - transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(_direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10f); 
        }
        
    }

//----------------------------------------------------------------------------------------------- 
    List<Transform> EnemiesInRange(List<Transform> enemies)
    { // This code is used to test if enemies is not standing beyond and obsticle
        for (int i = 0; i < enemies.Count; i++)
        {
            dir = enemies[i].position - transform.position;

            RaycastHit hit;
            bool isHit = Physics.Raycast(transform.position, dir, out hit);
 
            if (isHit && hit.transform.CompareTag("Monster")) 
            {

                _enemiesInRangeList.Add(enemies[i]);
            }

        }
        return _enemiesInRangeList;
    }
    

    //----------------------------------------------------------------------------------------------- 
    void Attack()
    {
        anim.Play("default attack");
    }  
    //----------------------------------------------------------------------------------------------- 

    public void FireMagicBall() // animation Trigger
    {
            EnableUnActiveBulletThenFireBullet();
    }
    //----------------------------------------------------------------------------------------------- 
    void FireBot()
    {
        anim.Play("charge attack");
    }
    //-----------------------------------------------------------------------------------------------    
    void FirePlayer()
    {
        anim.Play("summon"); // you can put another animation
    }
//----------------------------------------------------------------------------------------------- 
    public void SpawnBot()    // animation Trigger
    {
        
    }
//----------------------------------------------------------------------------------------------- 
    public void SpanwPlayer()    // animation Trigger
    {
        
    }
    //----------------------------------------------------------------------------------------------- 


    void EnableUnActiveBulletThenFireBullet()
    {
        // object pooling
        for (int i = 0; i <_fireBallsList.Count; i++)
        {
            if (!_fireBallsList[i].activeInHierarchy)
            {
                _fireBallsList[i].transform.position = fireLocation.position;
                _fireBallsList[i].transform.rotation = transform.rotation;
                _fireBallsList[i].SetActive(true);
                Rigidbody temRigidBodyBullet = _fireBallsList[i].GetComponent<Rigidbody>();
                temRigidBodyBullet.GetComponent<Rigidbody>().velocity = transform.forward * 25f;
                break;
            }
        }
    }
    
    //----------------------------------------------------------------------------------------------- 
//    void EnableUnActiveDragonBot()
//    {
//                GameObject _dragonBot = Instantiate(GameManager.Instance.DragonBot) as GameObject;
//                _dragonBot.transform.position = _botFireLocation.position;
//                 _dragonBot.transform.rotation = transform.rotation;
//                _dragonBot.SetActive(true);
//     
//    }
    
    //----------------------------------------------------------------------------------------------- 
/*
 * The next code is for Instatiating a soilder
 */
//    void FireSoldier()
//    {
//        anim.Play("charge attack_1");
//    }
    
//----------------------------------------------------------------------------------------------- 
   
//    public void SpawnSoldierBot()    // animation Trigger
//    {
//        GameObject _soldierBot = Instantiate(GameManager.Instance.SoldierBot) as GameObject;
//        _soldierBot.transform.position = _botFireLocation.position;
//        _soldierBot.transform.rotation = transform.rotation;
//        _soldierBot.SetActive(true);
//    }
//----------------------------------------------------------------------------------------------- 
    //-----------------------------------------------------------------------------------------------     

    /*
     * next method is used to apply object pooling for Player Fire Balls
     */
    private List<GameObject> _fireBallsList; //object pooling
    private GameObject _FireBalls; // empty gameobject for enclosing arrows


    [SerializeField] private GameObject _Weapon;
    private void WeaponObjectPooling()
    {
        //object pooling
        _fireBallsList = new List<GameObject>();
        _FireBalls = new GameObject("Weapon Objects");
        _FireBalls.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 10; i++)
        {
            GameObject objFireBall = (GameObject) Instantiate(_Weapon);
            objFireBall.SetActive(false);
            _fireBallsList.Add(objFireBall);
            objFireBall.transform.parent = _FireBalls.transform;
        }
    }
    
    //----------------------------------------------------------------------------------------------- 
//----------------------------------------------------------------------------------------------- 

}
