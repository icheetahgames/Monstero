using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This class is doing the following tasks
//1. take all the enemies in current room
//2. check if number of enemies in this room is equal zero after each kill
//3. open portal
// important note : this scirpt is merged with the GameManagerScript after putting each level is a separated scene
public class Level : MonoBehaviour
{
    [SerializeField] private List<GameObject> _monsterListInRoom = new List<GameObject>();
    private GameObject _portal;
    public bool isClearRoom = false;
    // Start is called before the first frame update
    void Start()
    {
        _portal =  transform.Find("Portal").gameObject;
        _portal.SetActive(false);
        if (_monsterListInRoom.Count <= 0)
        {
            isClearRoom = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_monsterListInRoom.Count <= 0)
        {
            //LevelsManager.instance.IsClearRoom = true;
            _portal.SetActive(true);
  //          print("Clear");
        }
    }
    
    public void AddNewMonisterToGround(GameObject monsitor) // Monistors Loading will be from enemy health script
    {
        _monsterListInRoom.Add(monsitor);
    }

    public void RemoveMonistorFromGround(GameObject monsitor) // Monistors removal will be from enemy health script
    {
        _monsterListInRoom.Remove(monsitor);
    }
}
