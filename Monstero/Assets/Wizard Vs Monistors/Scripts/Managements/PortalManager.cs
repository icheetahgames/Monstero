using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    private GameObject _portal;

    private GameObject _nextStagePortal;
    // Start is called before the first frame update
    void Start()
    {
        _portal = GameObject.Find("/Level/Stationary Portions/Portal group/Portal");
        _nextStagePortal = GameObject.Find("/Level/Stationary Portions/Portal group/Next Stage Portal");
        _nextStagePortal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        print("Clear : " + GameManager.Instance.LevelIsClear);
        if (!GameManager.Instance.LevelIsClear)
        {
            _nextStagePortal.SetActive(false);
            _portal.SetActive(true);
        }
        else
            // All enemies die
        {
            _nextStagePortal.SetActive(true);
            _portal.SetActive(false);
        }
    }
}
