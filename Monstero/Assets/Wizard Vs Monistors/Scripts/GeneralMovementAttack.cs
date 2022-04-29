using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralMovementAttack : MonoBehaviour
{
    

//----------------------------------------------------------------------------------------------- 
    
    public void RotateTowards(Transform player)
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
//----------------------------------------------------------------------------------------------- 
    
    //----------------------------------------------------------------------------------------------- 
    #region variables reated to next function
    private Transform _tMin;
    private Double _minDist;
    private Vector3 _currentPos;
    private Double _dist;
    #endregion
    public Transform GetClosestEnemy(List<Transform> enemies)
    {
        _tMin = null;
        _minDist = Mathf.Infinity;
        _currentPos = transform.position;
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

}
