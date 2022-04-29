using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

/**
 * This class manage the movements of Bots that can shot
 * This class used with Bots that can shots such as dragon shots Ball
 */
public class DragonBotMovementAndShot : GeneralMovementAttack
{
    private Vector3 dir;
    private Animator _anim;
    private NavMeshAgent _navMeshAgent;
    //private List<Transform> _enemyies = new List<Transform>();  // I think this variable used before and now I do not use it
    private GameObject _DragonBall;
    private float timer = 0f;
    private BotHealth _botHealt;
    [SerializeField] private float _attackDistance = 5;
    [SerializeField] private float _timeBetweenAttacks = 2;
    [SerializeField] private Transform _fireLocation;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent =GetComponent<NavMeshAgent>();
        _navMeshAgent.stoppingDistance = _attackDistance;
        _anim = GetComponent<Animator>();
        _botHealt = GetComponent<BotHealth>();
        FireBallObjectPooling();
    }

    // Update is called once per frame
    void Update()
    {
        Clock();
        if (!GameManager.Instance.GameOver && !GameManager.Instance.IsClearRoom && GameManager.Instance.EnemiesOnTheGround.Count > 0 && _botHealt.IsAlive && !GameManager.Instance.NextLevel && !GameManager.Instance.NextLevel)
        {
            if (Vector3.Distance(transform.position, GetClosestEnemy(GameManager.Instance.EnemiesOnTheGround).position) <= _attackDistance && timer >= _timeBetweenAttacks)
            {
                _anim.Play("Arrow Attack");
                timer = 0;
            }
            else
            {
                MoveToEnemy(); 
            }
            RotateTowards(GetClosestEnemy(GameManager.Instance.EnemiesOnTheGround)) ;
        }
        else if (!GameManager.Instance.GameOver && GameManager.Instance.IsClearRoom && _botHealt.IsAlive && !GameManager.Instance.NextLevel)
        {
            MoveToPlayer();
        }
        else
        {
            //_navMeshAgent.SetDestination(this.transform.position);
        }
        
    }
//----------------------------------------------------------------------------------------------- 
    private void Clock()
    {
        timer += Time.deltaTime;
    }
//----------------------------------------------------------------------------------------------- 
    void MoveToEnemy()
    {
        _navMeshAgent.SetDestination(GetClosestEnemy(GameManager.Instance.EnemiesOnTheGround).position);
    }   
    
    void MoveToPlayer()
    {
        _navMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
    } 
//----------------------------------------------------------------------------------------------- 

    public void FireArrow()
    {
        for (int i = 0; i < _fireBallsList.Count; i++)
        {
            if (!_fireBallsList[i].activeInHierarchy)
            {
                _fireBallsList[i].transform.position = _fireLocation.position;
                _fireBallsList[i].transform.rotation = transform.rotation;
                _fireBallsList[i].SetActive(true);
                Rigidbody temRigidBodyBullet = _fireBallsList[i].GetComponent<Rigidbody>();
                temRigidBodyBullet.GetComponent<Rigidbody>().velocity = transform.forward * 8f;
                break;
            }
        }
    }
    
//----------------------------------------------------------------------------------------------- 
//-----------------------------------------------------------------------------------------------     

    /*
     * next method is used to apply object pooling for Player Fire Balls
     */
    private List<GameObject> _fireBallsList; //object pooling
    private GameObject _FireBalls; // empty gameobject for enclosing arrows


    [SerializeField] private GameObject _botWeapon;
    private void FireBallObjectPooling()
    {
        //object pooling
        _fireBallsList = new List<GameObject>();
        _FireBalls = new GameObject("Bot Weapon");
        _FireBalls.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 10; i++)
        {
            GameObject objFireBall = (GameObject) Instantiate(_botWeapon);
            objFireBall.SetActive(false);
            _fireBallsList.Add(objFireBall);
            objFireBall.transform.parent = _FireBalls.transform;
        }
    }
    
    //----------------------------------------------------------------------------------------------- 
}
