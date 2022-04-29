using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BotHealth : MonoBehaviour
{
    [SerializeField] public int startingHealth = 100;

    [SerializeField] private float timeSinceLastHit = 0.5f;

    [SerializeField] private float dissapearSpeed = 2f;
    
    [SerializeField] private Canvas _canvas;
    [SerializeField] private float HC = 1;
    private AudioSource audio;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    private bool isAlive;
    private Rigidbody rb;
    private CapsuleCollider cC;
    private bool dissapearEnemy = false;
    private float curretHealth;
    private ParticleSystem blood;
    public bool IsAlive => isAlive; // getfun
    private Slider _slider;
    private bool _iskillBotApplied = false; // this variable is used to apply bot killing once and when game is over(player is killed) and not apply  KillBot() method each update frame 

    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        GameManager.Instance.RegisterAlly(this.transform);
        GameManager.Instance.RegisterBot(this.transform);
        
        //GameManager.Instance.RegisterEnemyOnTheGround(gameObject);
        //GameManager.Instance.EnemyInRange.Add(false);
        rb = GetComponent<Rigidbody>();
        cC = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        blood = GetComponentInChildren<ParticleSystem>();          blood.Stop();
        isAlive = true;
        curretHealth = startingHealth;
        CanvasMatter();

        nav.enabled = true; // This and next line to make the bot be able to instantiate in another levels
        this.GetComponent<Animator>().enabled = true;
    }

    void CanvasMatter()
    {
        //_canvas = (Canvas) Instantiate(_canvas);
        //_canvas.transform.parent = this.transform;
        //_canvas.transform.localPosition = new Vector3(0, 2f, 0);
        _slider = _canvas.transform.GetChild(0).GetComponent<Slider>();
        _slider.value = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (dissapearEnemy) // اتوقع يجب ان يكون هذا الشرط في دالة removeEnemy
        {
            this.transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
        }

        if (GameManager.Instance.GameOver && !_iskillBotApplied)
        {
            KillBot();
            _iskillBotApplied = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.Instance.GameOver)
        {
            if (other.tag == "Weapon")
            {
                TakeHit(other);
                blood.Play();
                timer = 0f;
            }
        }
    }

    void TakeHit(Collider collider)
    {
        
        if (curretHealth > 0 & isAlive)
        {
            audio.PlayOneShot(audio.clip);
            anim.SetBool("FollowEnemy", true);
            anim.Play("Hit");
            if (collider.transform.GetComponentInParent<AttackSpecification>() != null)
            {
                curretHealth -= HC * collider.transform.GetComponentInParent<AttackSpecification>().HP;
            }
            else
            {
                curretHealth -= 10;
            }
           
        }

        if (curretHealth <= 0 & isAlive)
        {
            KillBot();
        }
        _slider.value = curretHealth;
    }

    void KillBot()
    {
        
        isAlive = false;
        _canvas.enabled = false;
        GameManager.Instance.RemoveBot(this.transform);
        GameManager.Instance.RemoveAlly(this.transform);
        //GameManager.Instance.RemoveEnemyOnTheGround(gameObject);
        //GameManager.Instance.KilledBot(this.transform);
        cC.enabled = false;
        nav.enabled = false;
        anim.SetTrigger("Die");
        rb.isKinematic = true;
        _slider.value = 0;
        StartCoroutine(RemoveEnemy());
    }

    IEnumerator RemoveEnemy()
    {
        yield return new WaitForSeconds(4f);
        dissapearEnemy = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
