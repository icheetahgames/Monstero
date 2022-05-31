using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerBotHealth : MonoBehaviour
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
    [SerializeField] private Slider _healthSlider;
  //  [SerializeField] private ParticleSystem _baseVFX;
#endregion
//-----------------------------------------------------------------------------------------------
    #region Cashed variables(components)
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Class Variables(private-Only related to this class)    
    private float _timer = 0f;
    private CharacterController _cC;
    private CapsuleCollider _capsuleC;
    private Animator _anim;
    //private int _currentHealth;
    private AudioSource _audio;
    //private ParticleSystem _blood;
    private int _health;
    private bool dissapearPlayerBot = false;
    private bool _iskillPlayerBotApplied = false; // this variable is used to apply bot killing once and when game is over(player is killed) and not apply  KillBot() method each update frame
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
        
        GameObject.DontDestroyOnLoad(this.gameObject);
        
#region Cahsed Variables Definition
        _anim = GetComponent<Animator>();
        _cC = GetComponent<CharacterController>();
        _capsuleC = GetComponent<CapsuleCollider>();
        _audio = GetComponent<AudioSource>();
        //_blood = GetComponentInChildren<ParticleSystem>(); 
#endregion
        
        _healthSlider.value = _startingHealth;
        _health = _startingHealth;
        //_blood.Stop();
        GameManager.Instance.RegisterAlly(transform);
        GameManager.Instance.RegisterPlayer(this.transform);
       // _baseVFX.Play();
        
    }
//----------------------------------------------------------------------------------------------- 

    // Update is called once per frame
    void Update()
    {
        Clock();
        if (dissapearPlayerBot) // اتوقع يجب ان يكون هذا الشرط في دالة removeEnemy
        {
            this.transform.Translate(-Vector3.up * 2 * Time.deltaTime);
        }

        if (GameManager.Instance.GameOver && !_iskillPlayerBotApplied)
        {
            KillPlayer();
            _iskillPlayerBotApplied = true;
        }
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
            if (other.tag == "Weapon")
            {
                TakeHit();
                _timer = 0;
            }
        }
    }
//----------------------------------------------------------------------------------------------- 

    void TakeHit()
    {
        
        _audio.PlayOneShot(_audio.clip);
        if (_health> 0)
        {
            _anim.Play("gethit front");
            _health-= 10;
            _healthSlider.value = _health;        
        }
        else 
        {
            KillPlayer();
        }
       // _blood.Play();
    }
//----------------------------------------------------------------------------------------------- 

    void KillPlayer()
    {
       GameManager.Instance.RemovePlayer(this.transform);
       GameManager.Instance.RemoveAlly(this.transform);
        _anim.SetTrigger("HeroDie");
        _cC.enabled = false;
        _capsuleC.enabled = false;
        StartCoroutine(RemovePlayerBot());
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
    
    IEnumerator RemovePlayerBot()
    {
        yield return new WaitForSeconds(4f);
        dissapearPlayerBot = true;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
//    public  void SoldierFire()
//    {
//        if (!GameManager.Instance.GameOver  && !GameManager.Instance.IsWalking)
//        {
//            FireSoldier();
//        }
//    }
//#endif
}
