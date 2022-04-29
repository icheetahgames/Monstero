using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{//Attached to the arrow 
    private void OnEnable()
    {
        transform.GetComponent<Rigidbody>().WakeUp();
        Invoke("HideArrow", 4.0f);
    }
//----------------------------------------------------------------------------------------------- 

    private  void HideArrow()
    {
        gameObject.SetActive(false);
    }
//----------------------------------------------------------------------------------------------- 

    private void OnDisable()
    {
        transform.GetComponent<Rigidbody>().Sleep();
        CancelInvoke();
    }
//----------------------------------------------------------------------------------------------- 

    private void OnCollisionEnter(Collision other)
    {
        gameObject.SetActive(false);
    }
}
