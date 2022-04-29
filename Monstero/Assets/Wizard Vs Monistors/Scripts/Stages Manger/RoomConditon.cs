using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConditon : MonoBehaviour
{
    // وضيفة هذا الكلاس هو تحديد اذا ما كان الاعب ضمن الحدود المطلوبة
    // يضاف الى كل غرفة
    // تقريبا لا يؤثر على اللعبة 
    // المطلوب في المستقبل جعله يحدد الوحوش الموجودة داخل الغرفة و فتح الغرفة التالية عند اتنهاء جميع الوحوش
    List<GameObject> MonsterListInRoom = new List<GameObject>();

    public bool playerInThisRoom = false;

    public bool isClearRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        if (playerInThisRoom)
        {
            if (MonsterListInRoom.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
                print(" Clear ");
            }
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if (playerInThisRoom)
        {
            if (MonsterListInRoom.Count <= 0 && !isClearRoom)
            {
                isClearRoom = true;
                Debug.Log(" Clear ");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = true;
            MonsterListInRoom =  new List<GameObject>(GameManager.Instance.MonsterList);
            //PlayerTargeting.instance.MonsterList = new List<GameObject>(MonsterListInRoom);
//            Debug.Log("Enter New Room ! Mob Count : " + MonsterListInRoom.Count);
 //           Debug.Log("Player Enter New Room");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInThisRoom = false;
        }

        if (other.CompareTag("Monster"))
        {
            MonsterListInRoom.Remove(other.gameObject);
        }
    }
}
