using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    #region Public Variables
    #endregion
//----------------------------------------------------------------------------------------------- 
    #region SerializeField variables
    [SerializeField] private float _smoothing = 5.0f;
    [SerializeField] private float _zLimit = 19f;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Cashed variables(components)
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Class Variables(private-Only related to this class)    
    private Vector3 _offset;
    private Vector3 targetCamPos;
    private Transform _target;
    private float _lastZPos; // This variable used to limit camera in z direction
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Getters & Setters functions
    #endregion
//----------------------------------------------------------------------------------------------- 

    private void Awake()
    {
        // Assert.IsNotNull(_target);
        
        
    }
//----------------------------------------------------------------------------------------------- 

    private void Start()
    {
        GameObject.DontDestroyOnLoad(this.transform.parent.gameObject);
        _target = GameManager.Instance.Player.transform;
        _offset = transform.position - _target.position;
    }
//----------------------------------------------------------------------------------------------- 
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (_target.position.z > _zLimit)
        {
            targetCamPos = _target.position + _offset;
            targetCamPos.z = _lastZPos;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, _smoothing * Time.deltaTime); 
        }
        else
        {
            
            targetCamPos = _target.position + _offset;
            _lastZPos = targetCamPos.z;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, _smoothing * Time.deltaTime); 
        }
    }
}
