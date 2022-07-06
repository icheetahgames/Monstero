using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
private GameObject _portal;

    private GameObject _nextStagePortal;

     public GameObject Entry;
    // Start is called before the first frame update
    void Start()
    {
        if (this.transform.GetSiblingIndex() + 1 < this.transform.parent.childCount)
        {
            Entry = this.transform.parent.GetChild(this.transform.GetSiblingIndex() + 1).transform.Find("Entry")
                .gameObject;
        }
        _portal = transform.Find("Level/Stationary Portions/Portal group/Portal").gameObject;
        _nextStagePortal = transform.Find("Level/Stationary Portions/Portal group/Next Stage Portal").gameObject;
        _nextStagePortal.SetActive(false);
    }
    

    // Update is called once per frame
    void Update()
    {
        if (transform.Find("Enemies On The Ground").childCount > 0)
        {
            _nextStagePortal.SetActive(false);
            _portal.SetActive(true);
            GameManager.Instance.LevelIsClear = false;


        }
        else
            // All enemies die
        {
            _nextStagePortal.SetActive(true);
            _portal.SetActive(false);
            GameManager.Instance.LevelIsClear = true;
        }
        
    }
}
