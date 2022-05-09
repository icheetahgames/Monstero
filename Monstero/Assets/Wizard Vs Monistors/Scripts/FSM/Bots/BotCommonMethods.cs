using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotCommonMethods : MonoBehaviour
{
    
    private BoxCollider[] weaponColliders;
    private Animator _anim;
    private AudioSource _attackAudio, _fireEnemiesAduio;
    private  ParticleSystem _attackParticle, _fireEnemiesPartilce;

    public float _distanceToFollowEnemy = 10f;
    public BotType _botType;
    private void Start()
    {
        _anim = GetComponent<Animator>();
        print("Bot type : " + _botType);
        if (_botType == BotType.shooter)
        {
            FireBallObjectPooling();
        }
        else
        {
            weaponColliders = GetComponentsInChildren<BoxCollider>();
            if (transform.Find("WeaponParticles"))
            {
                _attackParticle = transform.Find("WeaponParticles").GetComponent<ParticleSystem>();
                _attackAudio = transform.Find("WeaponParticles").GetComponent<AudioSource>();
                _attackParticle.Pause();
                print("Weapon Particle is founded");
            }



            if (transform.Find("FireEnemies"))
            {
                _fireEnemiesPartilce = transform.Find("FireEnemies").GetComponent<ParticleSystem>();
                _fireEnemiesAduio = transform.Find("FireEnemies").GetComponent<AudioSource>();
                _fireEnemiesPartilce.Pause();
            }
        }
        
    }

    [SerializeField] private Transform _fireLocation;
    #region variables reated to next function
    private Transform _tMin;
    private Double _minDist;
    private Vector3 _currentPos;
    private Double _dist;
    #endregion
    public Transform GetClosestEnemy(Animator animator, List<Transform> enemies)
    {
        _tMin = null;
        _minDist = Mathf.Infinity;
        _currentPos = animator.transform.position;
        foreach (Transform t in enemies)
        {
            _dist = Math.Pow(Vector3.Distance(t.position, _currentPos), 2);
            if (_dist < _minDist)
            {
                _tMin = t;
                _minDist = _dist;
            }
        }
        return _tMin;
    }
    
//----------------------------------------------------------------------------------------------- 

    public void RotateTowards(Animator animator, Transform player, float speed)
    {
        Vector3 direction = (player.position - animator.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, Time.deltaTime * speed);
    }
 //-------------------------------------------------------------------------------------------------------------------   

 public void Fire()
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

    public void Fire2()
    {
        for (int i = 0; i < _fireBallsList.Count; i++)
        {
            if (!_fireBallsList[i].activeInHierarchy)
            {
                _fireBallsList[i].transform.position = _fireLocation.position;
                _fireBallsList[i].transform.rotation = transform.rotation;
                _fireBallsList[i].SetActive(true);
                break;
            }
        }
    }

    public void EnemyStartAttack() //Animation Event
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = true;
        }

        if (_attackParticle )
        {
            _attackParticle.Play();
        }

        if (_attackAudio)
        {
            _attackAudio.Play();
        }

    }
//----------------------------------------------------------------------------------------------- 

    public void EnemyEndAttack() //Animation Event
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = false;
        }
    }
//----------------------------------------------------------------------------------------------- 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            _anim.Play("Idle");
            GameManager.Instance.PlayerTrailer.SetActive(true);
            this.transform.LookAt(GameManager.Instance.PlayerTrailer.transform.position);
        }

    }
}

public  enum BotType
{
    shooter,
    Fighter
};

