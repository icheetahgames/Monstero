using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This code is used to keep the health slider looking at camera
public class HealthBarBillBoard : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(transform.position + GameManager.Instance.Camera.forward);
    }
}
