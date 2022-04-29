using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpotsLinker : MonoBehaviour
{
    public Transform[] Spots;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //----------------------------------------------------------------------------------------------- 
    #region variables reated to next function
    private Transform _tMin;
    private Double _minDist;
    private Vector3 _currentPos;
    private Double _dist;
    #endregion
    public Transform GetClosestEnemy(Animator animator, List<Transform> enemies)
    {
        _tMin = null;
        _minDist = Mathf.Infinity;
        _currentPos = animator.transform.position;
        foreach (Transform t in enemies)
        {
            _dist = Math.Pow(Vector3.Distance(t.position, _currentPos), 2);
            if (_dist < _minDist)
            {
                _tMin = t;
                _minDist = _dist;
            }
        }
        return _tMin;
    }
    
//----------------------------------------------------------------------------------------------- 

    public void RotateTowards(Animator animator, Transform player, float speed)
    {
        Vector3 direction = (player.position - animator.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, lookRotation, Time.deltaTime * speed);
    }
}
