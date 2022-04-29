using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 20;

    [SerializeField] private float timeSinceLastHit = 0.75f;

    [SerializeField] private float dissapearSpeed = 2f;
    
    [SerializeField] private GameObject _hurtCollider;
    [SerializeField] private Level _level;
    
    [HideInInspector]public Slider _slider;
    [SerializeField] protected float HC; // Hurt collider. How much current enemy will be effected by player weapon, if the enemy(monistor) is strong it will have a lower Hurt Coefficient(0<HC <= 1). HC is highest for monster plant.
    [SerializeField] public Canvas _canvas;
    [SerializeField] protected Transform _coinsEmergePoint;
    [SerializeField] protected float _deathCoinsNumber;
    [SerializeField] protected float _deathRedGemsNumber;
    [SerializeField] protected float _deathHeathGemsNumber;
    
    
    private AudioSource audio;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    [HideInInspector] public bool isAlive;
    private Rigidbody rb;
    private CapsuleCollider cC;
    [HideInInspector]public bool dissapearEnemy = false;
    [HideInInspector]public float curretHealth;     public float CurretHealth => curretHealth;
    public bool invincible = false;

    private ParticleSystem blood;
    public bool IsAlive => isAlive; // getfun

    void Start()
    {
        //GameManager.Instance.RegisterEnemy(this.transform);
        GameManager.Instance.RegisterEnemyOnTheGround(gameObject.transform);
        //_level.AddNewMonisterToGround(this.gameObject); // this variable used when level management was done in level.cs script and now GameManager is managing level transition
//        GameManager.Instance.EnemyInRange.Add(false);
        MonistorStart();
        isAlive = true;
        curretHealth = startingHealth;
        _slider = _canvas.transform.GetChild(0).GetComponent<Slider>();
        _slider.value = startingHealth;
        EnableHurtCollider();
        this.gameObject.SetActive(false); // I do not know the benefit ot these two lines
        this.gameObject.SetActive(true);
        
    }

    public virtual void EnableHurtCollider()
    {
        _hurtCollider.SetActive(true);
    }
    public virtual void MonistorStart()
    {
        rb = GetComponent<Rigidbody>();
        cC = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        blood = GetComponentInChildren<ParticleSystem>();          blood.Stop();
        timer = timeSinceLastHit;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (dissapearEnemy) // اتوقع يجب ان يكون هذا الشرط في دالة removeEnemy
        {
            MoveEnemyDownAfterDeath();
        }

      
    }

    public virtual void MoveEnemyDownAfterDeath()
    {
        this.transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (timer >= timeSinceLastHit && !GameManager.Instance.GameOver && isAlive && !invincible)
        {
            if (other.tag == "PlayerWeapon")
            {
                TakeHit(other);
                PlayBloodParticleSystem();
                timer = 0f;
            }
        }
    }
    
    
    

    public virtual void PlayBloodParticleSystem()
    {
        blood.Play();
    }

    public virtual void TakeHit(Collider collider)
    {
        if (curretHealth > 0)
        {
            audio.PlayOneShot(audio.clip);
            anim.Play("Hit");
            if (collider.transform.parent.GetComponent<PlayerWeaponStatus>() != null)
            {
                curretHealth -= HC*collider.transform.parent.GetComponent<PlayerWeaponStatus>().HP; 
            }
            else
            {
                curretHealth -= 10; 
            }
            

        }
        _slider.value = curretHealth;
        
        if (curretHealth <= 0)
        {        
            //GameManager.Instance.RemoveEnemyOnTheGround(gameObject.transform);
            
            KillEnemy();
        }

        
    }

    public virtual void KillEnemy()
    {
        isAlive = false;
        GameManager.Instance.RemoveEnemyOnTheGround(this.transform);
        _canvas.enabled = false;
        nav.baseOffset = 0;
        //GameManager.Instance.EnemyInRange.Remove(true);
        //GameManager.Instance.KilledEnemy(this.transform);
        //_level.RemoveMonistorFromGround(this.gameObject); // this variable used when level management was done in level.cs script and now GameManager is managing level transition
        cC.enabled = false;
        nav.enabled = false;
        anim.SetTrigger("Die");
        rb.isKinematic = true;
        _hurtCollider.SetActive(false);
        for (int i = 0; i < _deathCoinsNumber; i++)
        {
            EnableUnActiveCoins(GameManager.Instance.CoinList.Count);           
        }
        for (int i = 0; i < _deathHeathGemsNumber; i++)
        {
            print("HI");
            EnableUnActiveHealthGems(GameManager.Instance.HealthGemsList.Count);           
        }

        for (int i = 0; i < _deathRedGemsNumber; i++)
        {
            EnableUnActiveRedGems(GameManager.Instance.RedGemsList.Count);
        }
        StartCoroutine(RemoveEnemy());
    }

    IEnumerator RemoveEnemy()
    {
        yield return new WaitForSeconds(1f);
        dissapearEnemy = true;
        yield return new WaitForSeconds(1f);
        Destroy(this.transform.parent.gameObject);
    }
    void EnableUnActiveCoins(float coinsNumber)
    {
        // object pooling
        for (int i = 0; i < coinsNumber; i++)
        {
            if (!GameManager.Instance.CoinList[i].activeInHierarchy)
            {
                
                GameManager.Instance.CoinList[i].transform.position = _coinsEmergePoint.position;
                GameManager.Instance.CoinList[i].SetActive(true);
                GameManager.Instance.CoinList[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10f, 10f), 1, Random.Range(-10f, 10f)), ForceMode.Impulse);
                break;
            }
        }
    }
    
    protected void EnableUnActiveRedGems(float gemsNumber)
    {
        // object pooling
        for (int i = 0; i < gemsNumber; i++)
        {
            if (!GameManager.Instance.RedGemsList[i].activeInHierarchy)
            {
                
                GameManager.Instance.RedGemsList[i].transform.position = _coinsEmergePoint.position;
                GameManager.Instance.RedGemsList[i].SetActive(true);
                GameManager.Instance.RedGemsList[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), ForceMode.Impulse);
                break;
            }
        }
    }
    void EnableUnActiveHealthGems(float healthGemNumber)
    {
        // object pooling
        for (int i = 0; i < healthGemNumber; i++)
        {
            if (!GameManager.Instance.HealthGemsList[i].activeInHierarchy)
            {
                
                GameManager.Instance.HealthGemsList[i].transform.position = _coinsEmergePoint.position;
                GameManager.Instance.HealthGemsList[i].SetActive(true);
                GameManager.Instance.HealthGemsList[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), ForceMode.Impulse);
                break;
            }
        }
    }
}
