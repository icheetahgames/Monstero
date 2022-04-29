using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NextLevelPortal : MonoBehaviour
{
    [SerializeField] private Transform _entry;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.Level+1 <= transform.Find("/Levels").childCount)
        {
            _entry = transform.parent.parent.parent.parent.GetComponent<LevelManager>().Entry.transform;
        }
        else
        {
            print("Last level");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !GameManager.Instance.NextLevel)
        {
            print("Next Level");
            //other.transform.localPosition = _entry.position;
            StartCoroutine(MoveToNextLevel(other));
            GameManager.Instance.NextLevel = true;
            GameManager.Instance.LoadNextLevel();
            
        }
        else
        {
            GameManager.Instance.NextLevel = false;
        }
    }


    IEnumerator MoveToNextLevel(Collider other)
    {
        yield return new WaitForSeconds(1f);
        other.transform.localPosition = _entry.position;
        MoveBotsToNextLevel();
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.NextLevel = false;
    }

    void MoveBotsToNextLevel()
    {
        for (int i = 0; i < GameManager.Instance.BotEncluser.transform.childCount; i++)
        {
            GameManager.Instance.BotEncluser.transform.GetChild(i).GetComponent<Animator>().enabled = false;
            GameManager.Instance.BotEncluser.transform.GetChild(i).GetComponent<NavMeshAgent>().enabled = false;
            
            GameManager.Instance.BotEncluser.transform.GetChild(i).transform.localPosition = _entry.position;
            
            GameManager.Instance.BotEncluser.transform.GetChild(i).GetComponent<Animator>().enabled = true;
            GameManager.Instance.BotEncluser.transform.GetChild(i).GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
