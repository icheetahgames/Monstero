using System.Collections.Generic;
//using UnityEditor.Rendering.Experimental.LookDev;
using UnityEngine;

public class StageMGN : MonoBehaviour
{

    
    public static StageMGN instance = null;

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


    private bool _midBoss = false;
    private bool _angle = false;
    private bool _endBoss = false;
    public int CurrentStage = 0;
    private int _room = 0;
    private int lastRoom ; // Total number of room including angle
    
    //public GameObject player;
    public Camera camera;


    private Vector3 cameraPosWRTPlayer;
    [SerializeField] private GameObject _startRoom;
    [System.Serializable]
    public class StartPositionArray
    {
        public List<GameObject> Rooms = new List<GameObject>();
    }

    public StartPositionArray[] Stages;


    private List<GameObject> passedRooms = new List<GameObject>();
        
    public List<GameObject> AngelRooms = new List<GameObject>();
    public List<GameObject> MidBossRooms = new List<GameObject>();

    public GameObject FinalBossRoom;

    

    

    
    private void Start()
    {
        for (int i = 0; i < Stages.Length; i++)
        {
            lastRoom += Stages[i].Rooms.Count;   
        }
        
        lastRoom += AngelRooms.Count + MidBossRooms.Count + 1; // +1 is for last Boos
        cameraPosWRTPlayer = camera.transform.position - GameManager.Instance.Player.transform.position;
        HideAllRooms();
    }



    public void NextStage()
    {
        
        _room++;
        if (_room > lastRoom)
        {
            return;
        }

        if (_room > 1)
        {
            DeleteRooms(passedRooms[0]);

        }
        if (Stages[CurrentStage].Rooms.Count > 0)
        {
            int randomRoom = Random.Range(0, Stages[CurrentStage].Rooms.Count);
            Stages[CurrentStage].Rooms[randomRoom].SetActive(true);
            GameManager.Instance.Player.transform.position = Stages[CurrentStage].Rooms[randomRoom].transform.Find("StartPoint").transform.position;
            camera.transform.position = GameManager.Instance.Player.transform.position + cameraPosWRTPlayer;
            passedRooms.Add(Stages[CurrentStage].Rooms[randomRoom]);
            Stages[CurrentStage].Rooms.RemoveAt(randomRoom);
        }
        else
        {
            if (!_midBoss && !_angle && !_endBoss)
            {
                int randomRoom = Random.Range(0, MidBossRooms.Count);
                MidBossRooms[randomRoom].SetActive(true);
                GameManager.Instance.Player.transform.position = MidBossRooms[randomRoom].transform.Find("StartPoint").transform.position;
                camera.transform.position = GameManager.Instance.Player.transform.position + cameraPosWRTPlayer;
                passedRooms.Add(MidBossRooms[randomRoom]);
                MidBossRooms.RemoveAt(randomRoom);
                _midBoss = true;

            }
            else if (_midBoss && !_angle && !_endBoss)
            {
                int randomRoom = Random.Range(0, AngelRooms.Count);
                AngelRooms[randomRoom].SetActive(true);
                GameManager.Instance.Player.transform.position = AngelRooms[randomRoom].transform.Find("StartPoint").transform.position;
                camera.transform.position = GameManager.Instance.Player.transform.position + cameraPosWRTPlayer;
                passedRooms.Add(AngelRooms[randomRoom]);
                AngelRooms.RemoveAt(randomRoom);
                _angle = true;
            }
            else if (_midBoss && _angle && !_endBoss)
            {
                FinalBossRoom.SetActive(true);
                GameManager.Instance.Player.transform.position = FinalBossRoom.transform.Find("StartPoint").transform.position;
                camera.transform.position = GameManager.Instance.Player.transform.position + cameraPosWRTPlayer;
                _midBoss = false;
                _angle = false;
            }
            
            CurrentStage += 1;
            

        }

//        if (Stages[_currentStage].startRoomPosition.Count == 0)
//        {
//            _currentStage += 1;
//        }



        if (_room == 1)
        {
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

        for (int i = 0; i < MidBossRooms.Count; i++)
        {
            MidBossRooms[i].SetActive(false);
        }
        
        
        for (int i = 0; i < AngelRooms.Count; i++)
        {
            AngelRooms[i].SetActive(false);
        }
      
        FinalBossRoom.SetActive(false);
        
    }

    private void DeleteRooms(GameObject Room)
    {
        Room.SetActive(false);
        passedRooms.RemoveAt(0);
    }
}
    
    
    
    
    
