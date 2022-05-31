using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttack : GeneralMovementAttack
{
    #region Public Variables
    #endregion
//----------------------------------------------------------------------------------------------- 
    #region SerializeField variables
    [SerializeField] private Transform fireLocation;
    [SerializeField] private GameObject _wazard;
    [SerializeField] private Transform _botFireLocation;
    [SerializeField] private Transform _playerFireLocation;
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private float timeThresholdTorotateToNextEnemy = 0.2f;
    [SerializeField] private GameObject _bot;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Cashed variables(components)
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Class Variables(private-Only related to this class)    
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
        #endregion
        
        _pastClosestEnemy = GetClosestEnemy(GameManager.Instance.EnemiesOnTheGround);
        WeaponPooling();
    }
//----------------------------------------------------------------------------------------------- 
    private void Update()
    {
        Clock();
        if (!GameManager.Instance.GameOver && 
            !GameManager.Instance.IsWalking && 
            !GameManager.Instance.SpawnBot &&  
            GameManager.Instance.EnemiesOnTheGround.Count > 0 &&
            Vector3.Distance(this.transform.position, GetClosestEnemy(GameManager.Instance.EnemiesOnTheGround).position) < 50 && 
            GetClosestEnemy(GameManager.Instance.EnemiesOnTheGround) != null)
        {
            RotateTowardsClosestEnemy();
            if (_timer > _timeBetweenAttacks)
            {
                Attack();
                _timer = 0f;
            }
        }

        if (!GameManager.Instance.GameOver   &&!GameManager.Instance.SpawnBot &&  GameManager.Instance.EnemiesOnTheGround.Count > 0 && this.transform.name != "wizard(Clone)")
        {
            BaseVFX();
        }


#if UNITY_EDITOR
        if (!GameManager.Instance.GameOver &&  !GameManager.Instance.IsWalking )
        {
            if (Input.GetMouseButton(0))
            {
                //Attack();
            }

            if (Input.GetMouseButton(1) || Input.GetKeyDown(KeyCode.G))
            {
                FireBot();
            }
            
            if (Input.GetKeyDown("g"))
            {
                //FireSoldier();
            }
             
        }
#endif
        //BaseVFXManagment();
        //_enemiesInRangeList.Clear();

    }
    
    //----------------------------------------------------------------------------------------------- 
    private void Clock()
    {
        _timer += Time.deltaTime;
        _closestEnemyTimer += Time.deltaTime;
    }
//----------------------------------------------------------------------------------------------- 
    void BaseVFX()
    {
        _currentClosestEnemy = GetClosestEnemy(GameManager.Instance.EnemiesOnTheGround);
        if (_currentClosestEnemy != _pastClosestEnemy && _closestEnemyTimer < timeThresholdTorotateToNextEnemy )
        {
            BaseVFXManagment(_pastClosestEnemy);

        }
        else
        {
            BaseVFXManagment(_currentClosestEnemy);
            _pastClosestEnemy = _currentClosestEnemy;
            _closestEnemyTimer = 0f;
        }
    }
//----------------------------------------------------------------------------------------------- 
    void BaseVFXManagment(Transform closestEnemy)
    {
 //       if (!GameManager.Instance.GameOver && GameManager.Instance.EnemiesOnTheGround.Count > 0 && GetClosestEnemy(EnemiesInRange(GameManager.Instance.EnemiesOnTheGround)) != null)
        {
 //           GameManager.Instance.BaseVfx.gameObject.SetActive(true);

            GameManager.Instance.BaseVfx.transform.position = closestEnemy.position;
        }
        // else
        // {
        //     print("shut base vfx off");
        //     GameManager.Instance.BaseVFX.gameObject.SetActive(false);
        // }
    }
//----------------------------------------------------------------------------------------------- 

    void RotateTowardsClosestEnemy()
    {
        _currentClosestEnemy = GetClosestEnemy(GameManager.Instance.EnemiesOnTheGround);
        if (_currentClosestEnemy != _pastClosestEnemy && _closestEnemyTimer < timeThresholdTorotateToNextEnemy )
        {
            RotateTowards(_pastClosestEnemy);
            BaseVFXManagment(_pastClosestEnemy);

        }
        else
        {
            RotateTowards(_currentClosestEnemy);
            BaseVFXManagment(_currentClosestEnemy);
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
    List<Transform> EnemiesInRange(List<Transform> enemies) // This mehtod returens the enemies that has no barrer in between with player
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
        EnableUnActiveDragonBot();
        GameManager.Instance.AvailableNumberofBots -= 1;
        GameManager.Instance.BotNumberText.text = GameManager.Instance.AvailableNumberofBots.ToString();
    }
    //----------------------------------------------------------------------------------------------- 
    void EnableUnActiveDragonBot()
    {
        GameObject bot = Instantiate(_bot) as GameObject;
        bot.transform.SetParent(GameManager.Instance.BotEncluser.transform);
        bot.transform.position = _botFireLocation.position;
        bot.transform.rotation = transform.rotation;
        bot.SetActive(true);
        bot.GetComponent<Animator>().enabled = false; // This and next line to make the bot be able to instantiate in another levels
        bot.GetComponent<NavMeshAgent>().enabled = false;
    }
//----------------------------------------------------------------------------------------------- 
    public void SpanwPlayer()    // animation Trigger
    {
        CheckIfAnotherBotPlayerIsInFireLocationPosition();
        GameObject _newPlayer = Instantiate(_wazard, _playerFireLocation.position, this.transform.rotation) as GameObject;
        _newPlayer.SetActive(true);
        GameManager.Instance.AvailableNumberofDoublicated -= 1;
        GameManager.Instance.DoublicateNumberText.text = GameManager.Instance.AvailableNumberofDoublicated.ToString();
    }
//----------------------------------------------------------------------------------------------- 
    void CheckIfAnotherBotPlayerIsInFireLocationPosition()
    {
        for (int i = 0; i < GameManager.Instance.Players.Count; i++)
        {
            if (Vector3.Distance(GameManager.Instance.Players[i].transform.position, _playerFireLocation.position) <= 1f)
            {
                print("Same Position and index : " + i);
                GameManager.Instance.Players[i].transform
                    .SetPositionAndRotation(new Vector3(GameManager.Instance.Players[i].transform.position.x,
                            GameManager.Instance.Players[i].transform.position.y, 
                            GameManager.Instance.Players[i].transform.position.z + 2), 
                        GameManager.Instance.Players[i].transform.rotation);
            }
        }
    }
    //----------------------------------------------------------------------------------------------- 


    void EnableUnActiveBulletThenFireBullet()
    {
        // object pooling
        for (int i = 0; i < _fireBallsList.Count; i++)
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
    private void WeaponPooling()
    {
        //object pooling
        _fireBallsList = new List<GameObject>();
        _FireBalls = new GameObject("Bot Weapon");
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

   
//#if PLATFORM_ANDROID
    public  void Fire()
    {
        if (!GameManager.Instance.GameOver  && !GameManager.Instance.IsWalking)
        {
            Attack(); 
        }
    }
    public  void BotFire()
    {
        if (!GameManager.Instance.GameOver  && !GameManager.Instance.IsWalking && GameManager.Instance.AvailableNumberofBots > 0)
        {
            FireBot();
            GameManager.Instance.SpawnBot = true;
        }
    }
    
    public  void SoldierFire()
    {
        if (!GameManager.Instance.GameOver  && !GameManager.Instance.IsWalking)
        {
//            FireSoldier();
        }
    }
    
    public  void PlayerFire()
    {
        if (!GameManager.Instance.GameOver  && !GameManager.Instance.IsWalking && GameManager.Instance.AvailableNumberofDoublicated > 0)
        {
            FirePlayer();
            GameManager.Instance.SpawnBot = true;
        }
    }
//#endif
}
