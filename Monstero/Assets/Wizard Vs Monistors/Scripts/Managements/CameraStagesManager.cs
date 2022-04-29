using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStagesManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemiesOnTheGround;
    [SerializeField] private GameObject _pauseBTN;
    [SerializeField] private GameObject _coints;
    [SerializeField] private GameObject _redGems;
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trigger 1")
        {


            StartCoroutine(squence());
            StartCoroutine(EmergePlayerTrailer());
        }
    }

    IEnumerator EmergePlayerTrailer()
    {
        yield return new WaitForSeconds(1);
        GameManager.Instance.PlayerTrialer.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        _anim.SetTrigger("Next");
        yield return new WaitForSeconds(3);
        _pauseBTN.SetActive(true);
        _coints.SetActive(true);
        _redGems.SetActive(true);
    }
    
    IEnumerator squence()
    {
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < _enemiesOnTheGround.transform.childCount; i++)
        {
            if (_enemiesOnTheGround.transform.GetChild(i).gameObject.activeSelf)
            {
                    
                if ( _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<EnemyHealth>() != null)
                {
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<Animator>().Play("Idle");
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<SpotsLinker>().RotateTowards(_enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<Animator>(), GameManager.Instance.PlayerTrialer.transform, 5f);
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<EnemyHealth>().enabled =
                        false;
                }
                else
                {
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<Animator>().Play("Idle");
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<BotCommonMethods>().RotateTowards(_enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<Animator>(), GameManager.Instance.PlayerTrialer.transform, 5f);
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<BotHealth>().enabled =
                        false;
                }
            }
        }
    }
}
