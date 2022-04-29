using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour 
{//Attached To the fireBall
   private void OnEnable()
   {
      transform.GetComponent<Rigidbody>().WakeUp();
      Invoke("HideFireBall", 4.0f);
   }
//----------------------------------------------------------------------------------------------- 

   private  void HideFireBall()
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
