using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraStagesManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemiesOnTheGround;
    [SerializeField] private Canvas UI;
    [SerializeField] private float _enemiesRotationSpeedinTrailer = 5;
    private Animator _anim;
    
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        UI.GetComponent<CanvasGroup>().alpha = 0f;
        GameManager.Instance.Player.SetActive(false);
        GameManager.Instance.Camera.gameObject.SetActive(false);
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
        GameManager.Instance.PlayerTrailer.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        _anim.SetTrigger("Next"); // to move camera to next postion
        yield return new WaitForSeconds(3);
        //UI.SetActive(true);
        UI.GetComponent<CanvasGroup>().alpha = 1f;
        yield return new WaitForSeconds(3);
        GameManager.Instance.LoadFirstLevelAfterTrailerScene();
        yield return new WaitForSeconds(2);
        
        GameManager.Instance.PlayerTrailer.SetActive(false);
        GameManager.Instance.Camera.gameObject.SetActive(true);
        
        //LoadNextLevel();
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
        GameManager.Instance.Player.SetActive(true);
       // GameManager.Instance.Player.SetActive(true);
        //GameManager.Instance.PlayerTrialer.SetActive(false);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<SpotsLinker>().RotateTowards(_enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<Animator>(), GameManager.Instance.PlayerTrailer.transform, _enemiesRotationSpeedinTrailer);
                    GameManager.Instance.RotateTowardsPlayer = true;
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<EnemyHealth>().enabled =
                        false;
                }
                else
                {
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<Animator>().Play("Idle");
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<BotCommonMethods>().RotateTowards(_enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<Animator>(), GameManager.Instance.PlayerTrailer.transform, _enemiesRotationSpeedinTrailer);
                    _enemiesOnTheGround.transform.GetChild(i).GetChild(0).GetComponent<BotHealth>().enabled =
                        false;
                }
            }
        }
    }


    //-----------------------------------------------------------------------------------------------     
   
    //-----------------------------------------------------------------------------------------------     

     void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
//-----------------------------------------------------------------------------------------------     
    IEnumerator LoadLevel(int levelIndex)
    {
        GameManager.Instance.LevelTransitionAnimator.SetTrigger("Level_Transition");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(levelIndex);
        GameManager.Instance.LevelTransitionAnimator.SetTrigger("Inverse_Level_Transition");
        yield return new WaitForSeconds(0.5f);
    }
    
    //-----------------------------------------------------------------------------------------------
    

}
