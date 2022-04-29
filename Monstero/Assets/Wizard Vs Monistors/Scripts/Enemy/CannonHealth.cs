using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonHealth : EnemyHealth
{
    // Start is called before the first frame update

    private BoxCollider bC;
    private Projectile _projectile;

    public bool _isalive;
    
    public override void MonistorStart()
    {
        bC = GetComponent<BoxCollider>();
        _isalive = true;
    }

    public override void PlayBloodParticleSystem()
    {
        
    }

    public override void EnableHurtCollider()
    {
        
    }

    public override void MoveEnemyDownAfterDeath()
    {
       // this.gameObject.SetActive(false);
    }

    public override void TakeHit(Collider collider)
    {
        if (curretHealth > 0)
        {
            //audio.PlayOneShot(audio.clip);  you can put audio to cannon or stationary enemies later
            //anim.Play("Hit");
            curretHealth -= HC *collider.transform.parent.GetComponent<PlayerWeaponStatus>().HP; 
            _isalive = true;

        }
        _slider.value = curretHealth;
        if (curretHealth <= 0)
        {        
            //GameManager.Instance.RemoveEnemyOnTheGround(gameObject.transform);
            
            KillEnemy();
        }
    }

    public override void KillEnemy()
    {
        isAlive = false;
        GameManager.Instance.RemoveEnemyOnTheGround(this.transform);
        _canvas.enabled = false;
        bC.enabled = false;
        _isalive = false;
        //anim.SetTrigger("Die");
        //_hurtCollider.SetActive(false);
        
        for (int i = 0; i < _deathCoinsNumber; i++)
        {
            EnableUnActiveCoins(GameManager.Instance.CoinList.Count);           
        }
        for (int i = 0; i < _deathHeathGemsNumber; i++)
        {
            EnableUnActiveHealthGems(GameManager.Instance.HealthGemsList.Count);           
        }
        for (int i = 0; i < _deathRedGemsNumber; i++)
        {
            EnableUnActiveRedGems(GameManager.Instance.RedGemsList.Count);
        }
        dissapearEnemy = true;
        StartCoroutine(RemoveEnemy());
    }

    IEnumerator RemoveEnemy()
    {
        
        yield return new WaitForSeconds(0.5f);
        Destroy(this.transform.parent.gameObject);
        print("Destory cannon");
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
    
    
    void EnableUnActiveHealthGems(float healthGemNumber)
    {
        // object pooling
        for (int i = 0; i < healthGemNumber; i++)
        {
            if (!GameManager.Instance.HealthGemsList[i].activeInHierarchy)
            {
                
                GameManager.Instance.HealthGemsList[i].transform.position = _coinsEmergePoint.position;
                GameManager.Instance.HealthGemsList[i].SetActive(true);
                GameManager.Instance.HealthGemsList[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10f, 10f), 1, Random.Range(-10f, 10f)), ForceMode.Impulse);
                break;
            }
        }
    }
    
    void EnableUnActiveRedGems(float gemsNumber)
    {
        base.EnableUnActiveRedGems(gemsNumber);
    }
}
