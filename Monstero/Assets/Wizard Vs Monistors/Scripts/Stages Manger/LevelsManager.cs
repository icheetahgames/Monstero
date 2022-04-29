using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This code is created after StageMGN for operating StageMGN step by step with expination
 * Note that all rooms will be on top each other with fixed stationary protions
 */
/*
 * Update 2121-4-15
 * this approach cancelled and each level was put in a separated scene
 */
public class LevelsManager : MonoBehaviour
{
   public static LevelsManager instance = null; //singlton to deal with this manager
   private bool isClearRoom = false; public bool IsClearRoom{get => isClearRoom;set => isClearRoom = value;}
   private void Awake()
    {
      if (instance == null)
      {
        instance = this;
      }
      else if(instance != null)
      {
        Destroy(gameObject);
      }
    }

      [SerializeField] private Transform _botsEnery;
      [SerializeField] private GameObject _startRoom; // قد أستفيد منها للأنتقال من الواجهه الرئيسية الى اللعبة
      [System.Serializable]
      public class StartPositionArray
      {
       public List<GameObject> Rooms = new List<GameObject>();
      }
      public StartPositionArray[] Stages; //every stage has several rooms
      
      
      private int _room = 0;
      
      public int CurrentStage = 0;
      
      //public GameObject player;
      [SerializeField] private Camera camera;


      private Vector3 cameraPosWRTPlayer;
      private List<GameObject> passedRooms = new List<GameObject>(); //I think passed rooms will only have a dimension of 1 since it will be removed in next room movement
      
      private bool _nextStage;

      public bool nextStage
      {
       get => _nextStage;
       set => _nextStage = value;
      }
     // Start is called before the first frame update
     void Start()
     {
      HideAllRooms();
      
      cameraPosWRTPlayer = camera.transform.position - GameManager.Instance.Player.transform.position; // find the distance between player and camera to keep it constant when moving to next room
     }

     // Update is called once per frame
     void Update()
     {
         
     }

     public void NextStage()
     {
       _room++;
       
       if (_room > 1)
       {
        DeleteRooms(passedRooms[0]); // remove the passed room which will have an index of zero
       }
       
       int randomRoom = Random.Range(0, Stages[CurrentStage].Rooms.Count); // choose a random room inside the current stage
       Stages[CurrentStage].Rooms[randomRoom].SetActive(true); // enable the randomly selected room
       print(Stages[CurrentStage].Rooms[randomRoom].transform.Find("StartPoint").transform.position);
       GameManager.Instance.Player.transform.position = Stages[CurrentStage].Rooms[randomRoom].transform.Find("StartPoint").transform.position; // move player to selected room
       // for (int i = 0; i < GameManager.Instance.Bots.Count; i++)
       // {//I do not know why bots moving is not working
       //  print("move bots" + i);
       //  GameManager.Instance.Bots[i].transform.position = _botsEnery.position; 
       // }
       camera.transform.position = GameManager.Instance.Player.transform.position + cameraPosWRTPlayer; // move camera
       passedRooms.Add(Stages[CurrentStage].Rooms[randomRoom]); // save current room to passed rooms
       Stages[CurrentStage].Rooms.RemoveAt(randomRoom); //remove the current room from this stage so that we will not move to it again
       
       if (_room == 1)
       {
        print("hide first level");
        _startRoom.SetActive(false);
       }
     }
     
     void HideAllRooms()
     {
      for (int i = 0; i < Stages.Length; i++)
      {
       for (int j = 0; j < Stages[i].Rooms.Count; j++)
       {
        Stages[i].Rooms[j].SetActive(false);
       }
      }
     }
     
     
     private void DeleteRooms(GameObject Room)
     {
      Room.SetActive(false);
      passedRooms.RemoveAt(0);
     }
}
