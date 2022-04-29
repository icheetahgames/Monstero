using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField] private Transform _pivot;

    [SerializeField] private float _amplitude = 100f;
    [SerializeField] private float _speed = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.eulerAngles = new Vector3(Mathf.Cos(Time.time * _speed / Mathf.PI) * _amplitude, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
