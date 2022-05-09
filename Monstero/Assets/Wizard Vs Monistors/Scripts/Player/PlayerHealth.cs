using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour
{
    /*
     * This code is clean
     */
    #region Public Variables
    #endregion
//----------------------------------------------------------------------------------------------- 
    #region SerializeField variables
    [SerializeField] private int _startingHealth = 100;
    [SerializeField] private float _timeSinceLastHit = 2f;
    [SerializeField] private Slider _healthSlider; public Slider HealthSlider => _healthSlider;
    //[SerializeField] private Text _healthText;
    [SerializeField] private ParticleSystem _baseVFX;
    [SerializeField] private ParticleSystem _startMagicVFX;
    [SerializeField] private float _healthGemAddPoints = 5f;
#endregion
//-----------------------------------------------------------------------------------------------
    #region Cashed variables(components)
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Class Variables(private-Only related to this class)    
    private float _timer = 0f;
    private CharacterController _cC;
    private Animator _anim;
    //private int _currentHealth;
    private AudioSource _audio;
    private ParticleSystem _blood;
    private float _health;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Getters & Setters functions
    #endregion
//----------------------------------------------------------------------------------------------- 

    private void Awake()
    {
        Assert.IsNotNull(_healthSlider);
    }

//----------------------------------------------------------------------------------------------- 
    private void Start()
    {

        //GameManager.Instance.Player = this.gameObject;
        
        //GameObject.DontDestroyOnLoad(this.gameObject);
        
#region Cahsed Variables Definition
        _anim = GetComponent<Animator>();
        _cC = GetComponent<CharacterController>();
        _audio = GetComponent<AudioSource>();
        _blood = GetComponentInChildren<ParticleSystem>(); 
#endregion
        
        _healthSlider.value = _startingHealth;
        //_healthText.text = _startingHealth.ToString();
        _health = _startingHealth;
        _blood.Stop();
        GameManager.Instance.RegisterAlly(transform);
        _baseVFX.Play();
        GameManager.Instance.RegisterPlayer(this.transform);
        StartCoroutine(PlayMagecVFX(3));
    }
//----------------------------------------------------------------------------------------------- 

    // Update is called once per frame
    void Update()
    {
        Clock();
    }
//----------------------------------------------------------------------------------------------- 
    private void Clock()
    {
        _timer += Time.deltaTime;
    }
    
//----------------------------------------------------------------------------------------------- 

    private void OnTriggerEnter(Collider other)
    {
        if (_timer >= _timeSinceLastHit && !GameManager.Instance.GameOver && !GameManager.Instance.SpawnBot)
        {
            if (other.tag == "Weapon" && !GameManager.Instance.Invincible)
            {
                TakeHit(other);
                _timer = 0;
            }

            if (other.tag == "HealthGem")
            {
                print("Health gem");
                HealthGemMethod(_healthGemAddPoints);
            }
            if (other.tag == "RedGem")
            {
                print("Health gem");
                //HealthGemMethod(_healthGemAddPoints);
            }
        }
    }
//----------------------------------------------------------------------------------------------- 

    void TakeHit(Collider collider)
    {
        _audio.PlayOneShot(_audio.clip);
        // if (_health> 0)
        // {
        //     GameManager.Instance.PlayerHit(_health);
        //     _anim.Play("gethit front");
        //     _health-= 10;
        //     _healthSlider.value = _health;        
        // }
        //
        // else 
        // {
        //     print("Kill player");
        //     KillPlayer();
        // }

        if (_health <= 0)
        {
            KillPlayer();
        }
        else
        {
            GameManager.Instance.PlayerHit(_health);
            _anim.Play("gethit front");
            if (collider.transform.GetComponentInParent<AttackSpecification>())
            {
                _health -= collider.transform.GetComponentInParent<AttackSpecification>().HP;
            }
            else
            {
                _health -= 10;
            }
            _healthSlider.value = _health;
            //_healthText.text = _health.ToString();
        }
        _blood.Play();
    }
//----------------------------------------------------------------------------------------------- 

    void KillPlayer()
    {
        _baseVFX.gameObject.SetActive(false);
       GameManager.Instance.PlayerHit(_health);
       GameManager.Instance.RemovePlayer(this.transform);
        _anim.SetTrigger("HeroDie");
        _cC.enabled = false;
        
    }
    
//----------------------------------------------------------------------------------------------- 
//#if PLATFORM_ANDROID
//    public  void Fire()
//    {
//        if (!GameManager.Instance.GameOver  && !GameManager.Instance.IsWalking)
//        {
//            Attack(); 
//        }
//    }
    public  void BotFire()
    {
        if (!GameManager.Instance.GameOver  && !GameManager.Instance.IsWalking)
        {
            _anim.Play("charge attack");
            print("Fire Bot");
        }
    }
    
//    public  void SoldierFire()
//    {
//        if (!GameManager.Instance.GameOver  && !GameManager.Instance.IsWalking)
//        {
//            FireSoldier();
//        }
//    }
//#endif

    IEnumerator PlayMagecVFX(float time)
    {
        _startMagicVFX.Play();
        yield return new WaitForSeconds(time);
        _startMagicVFX.Stop();
    }


    public void HealthGemMethod(float addheatlh)
    {
        _health += addheatlh;
        _healthSlider.value = _health;
    }
    
    public void IncreaseHealthWith(float addheatlh)
    {
        _health += addheatlh;
        _healthSlider.value = _health;
    }
}
