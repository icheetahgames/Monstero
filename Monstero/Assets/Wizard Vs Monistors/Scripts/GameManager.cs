using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
        
    #region Public Variables
    public static GameManager Instance; // Singleton
    public List<GameObject> MonsterList = new List<GameObject>();
    #endregion
    
    //----------------------------------------------------------------------------------------------- 
    #region SerializeField variables
    [Header("Player")]
    [SerializeField] private GameObject player;
    public GameObject Player
    {
        get => player;
        set => player = value;
    }

    [SerializeField] private GameObject _playerTrialer;

    public GameObject PlayerTrialer
    {
        get => _playerTrialer;
        set => _playerTrialer = value;
    }

    [SerializeField] private GameObject _cameras;
    [SerializeField] private Transform _firePos; public Transform FirePos => _firePos;
    [SerializeField] private GameObject magicBall; public GameObject MagicBall => magicBall;
    
    //[SerializeField] private GameObject[] spawnPoints; // this represents the point to spawn monistors. It is not used in this game
    
    // Enemies used to be spwan in previous game and it is not used in this game
    // [Header("Enemies")] 
    // [SerializeField] private GameObject tanker;
    // [SerializeField] private GameObject ranger;
    // [SerializeField] private GameObject solider;
    // [SerializeField] private GameObject dragon;
    // [SerializeField] private GameObject microZombie;
    [SerializeField] private GameObject arrow;        public GameObject Arrow => arrow;
    [SerializeField] private GameObject dragonBall;        public GameObject DragonBall => dragonBall;
    
    [SerializeField] private GameObject cannonBall;        public GameObject CannonBall => cannonBall;
 
    
    [Header("VFXs")]
    [SerializeField] private ParticleSystem _baseVFX;   public ParticleSystem BaseVfx => _baseVFX;

    

    //private ParticleSystem _baseVFXInstantiated;    public ParticleSystem BaseVFX => _baseVFXInstantiated; // I do not think that the instantiated base vfx used in this part

    [Header("UI Elements")] 
    [SerializeField] private Transform _camer;    public Transform Camera => _camer;
    //All the below variable are not utilized yet
    [SerializeField] private Text coinText;
    [SerializeField] private Text redGemText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text endGameText;
    [SerializeField] private GameObject _bigStick;
    [SerializeField] private GameObject _plane;
    [SerializeField] private GameObject _fireStick;
    
    [SerializeField] private GameObject _fireSoldier;
    
    [SerializeField] private GameObject _levelTransition; // Game object have the canvas that create the fade out - fade in at level transition

    

    [SerializeField] private Text _lightActive;
    

    [Header("Misc variables")]
    private float joyStickSensitiviy = 0.01f; public float JoyStickSensitiviy => joyStickSensitiviy;
    [SerializeField] private int finalLevel = 20;
    
    [SerializeField] private GameObject _coin;
    [SerializeField] private GameObject _redGem;
    [SerializeField] private GameObject _healthGem;
    #endregion
    //----------------------------------------------------------------------------------------------- 
    #region UI variables
    [HideInInspector]
    public Vector3 JoyVec; //joyVec is always between 0 and 1(x, y, 0)
                            // also JoyVec effecting the player's move direction
    public Vector3 Bias;
    #endregion
    
    //----------------------------------------------------------------------------------------------- 

    #region Getters & Setters functions

    private bool _isMuted = false;

    public bool IsMuted
    {
        get => _isMuted;
        set => _isMuted = value;
    }

    private bool _isWalking; // used for targeting perpuses

    public bool IsWalking
    {
        get => _isWalking;
        set => _isWalking = value;
    }

    private  bool _gameIsPaused = false;

    public  bool GameIsPaused
    {
        get => _gameIsPaused;
        set => _gameIsPaused = value;
    }
    #endregion

    #region Class Variables(private-Only related to this class)  
    private bool gameOver = false; public bool GameOver => gameOver; // get function
    private int _currentLevel;
    private readonly float generatedSpawnTime = 1; // I think this variable is not used yet
    private float _currentSpawnTime = 0;    // I think this variable is not used yet
    private GameObject _newEnemy;   // I think this variable is not used yet

    private int _randomNumber;  // I think this variable is not used yet
    private GameObject _spawnLocation;  // I think this variable is not used yet
    private int _randomEnemy;   // I think this variable is not used yet

    //private readonly List<Transform> _enemies = new List<Transform>();
    
    //private readonly List<Transform> _killedEnemies = new List<Transform>();

    private  float _playerHealth; public  float PlayerHealth{get => _playerHealth;set => _playerHealth = value;}


    [SerializeField] private  int _level;

    private bool _gameManagerStarted; // this variable is used to check is game manager is started. Using this variable we can open portal if game manager is started, since when moving to next level enemies number = 0
    public  int Level
    {
        get => _level;
        set => _level = value;
    }
    
    private float _spawnTimer = 0f;

    private Animator _levelTransitionAnimator;
    private bool isClearRoom = false; public bool IsClearRoom{get => isClearRoom;set => isClearRoom = value;} // This variable used to make bots follow player after killing all enemies
    private bool _nextLevel;

    public bool NextLevel
    {
        get => _nextLevel;
        set => _nextLevel = value;
    }

    

    Queue<float> FPSQ = new Queue<float>(10);

    private int _coinsCount = 0;

    public int CoinsCount
    {
        get => _coinsCount;
        set => _coinsCount = value;
    }

    private int _healthGemCout;

    public int HealthGemCout
    {
        get => _healthGemCout;
        set => _healthGemCout = value;
    }

    private int _redGemsCount = 0;

    public int RedGemsCount
    {
        get => _redGemsCount;
        set => _redGemsCount = value;
    }
    
    [SerializeField]private bool _invincible = false;

    public bool Invincible
    {
        get => _invincible;
        set => _invincible = value;
    }

    private bool _levelIsClear;

    public bool LevelIsClear
    {
        get => _levelIsClear;
        set => _levelIsClear = value;
    }
    
    private GameObject _botEncluser; // This variable is used to enclose the bots objects

    public GameObject BotEncluser => _botEncluser;
    #endregion
    //----------------------------------------------------------------------------------------------- 
    #region Variables Imigrated from Level Script
    // This part is doing the following tasks
    //1. take all the enemies in current room
    //2. check if number of enemies in this room is equal zero after each kill
    //3. open portal
    [Header("Level Managment")]
    [SerializeField] private  List<Transform> enemiesOnTheGround = new List<Transform>(); public List<Transform> EnemiesOnTheGround => enemiesOnTheGround;

    //[SerializeField] private GameObject _portal;
    
    [FormerlySerializedAs("_portal")] private GameObject _nextStagePortal;
    private GameObject _portal;
    [SerializeField]private Transform _startPostion; public Transform StartPostion => _startPostion;

    [SerializeField] private GameObject Lights;

    #endregion
    //----------------------------------------------------------------------------------------------- 
    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
        #endregion
        
        #region get saved data
        // PlayerData data = SystemSave.LoadPlayer(); This did not work with android
        // _coinsCount = data.CoinsCount;
        _coinsCount = PlayerPrefs.GetInt("CoinsCount", 0);
        #endregion

        //_portal.SetActive(true);
        _baseVFX.Play();
    }
    //----------------------------------------------------------------------------------------------- 
    void Start()
    {
        _spawnBot = false;    
        ArrowObjectPooling();
        DragonBallObjectPooling();
        CannonBallObjectPooling();
        CoinsPooling();
        HealthGemsPooling();
        RedGemsPooling();
        
        _levelTransitionAnimator = _levelTransition.GetComponent<Animator>();
        _gameManagerStarted = true;
        _nextLevel = false;
        
        
        _botEncluser = new GameObject("Bot Encluser");
        _botEncluser.transform.SetParent(this.transform);
        
        DisableAllEnemies(_level);
       
    }

//----------------------------------------------------------------------------------------------- 
    void DisableAllEnemies(int level)
    {
        for (int i = 0; i < level; i++)
        {
            print("level disabeld is : " + i);
            transform.Find("/Levels/").GetChild(i).Find("Enemies On The Ground").gameObject.SetActive(false);
        }
        for (int i = level+1; i < transform.Find("/Levels/").childCount; i++)
        {
            print("level disabeld is : " + i);
            transform.Find("/Levels/").GetChild(i).Find("Enemies On The Ground").gameObject.SetActive(false);
        }
    }

//----------------------------------------------------------------------------------------------- 

    void PutPlayerandCamerasAtTheSelectedLevel(int level)
    {//Not used yet
        Player.transform.localPosition = transform.Find("/Levels/").GetChild(_level).Find("Entry").position;
    }

//----------------------------------------------------------------------------------------------- 
    // Update is called once per frame
    void Update()
    {
        SpawnClock(); // I think I may need an alternative method for checking if the player is currently spawning instead of using update method 


        
        //StartCoroutine(ShutOffLights());
        coinText.text = _coinsCount.ToString();
        redGemText.text = _redGemsCount.ToString();

        if (transform.Find("/Levels/").GetChild(GameManager.Instance.Level).Find("Enemies On The Ground").childCount <= 0)
        {// All enemies die
            _baseVFX.gameObject.SetActive(false);
            MoveCoinsToPlayer();
            MoveHealthGemsToPlayer();
            MoveRedGemsToPlayer();
        }
        else
        {
            _baseVFX.gameObject.SetActive(true);
        }
        
        
        /*
        if (!_levelIsClear)
        {
            isClearRoom = false;
            _baseVFX.gameObject.SetActive(true);
        }
        else
        { // All enemies die
            isClearRoom = true;
            _baseVFX.gameObject.SetActive(false);


            MoveCoinsToPlayer();
            MoveHealthGemsToPlayer();
            MoveRedGemsToPlayer();
        }
        if (EnemiesOnTheGround.Count <= 0 & _gameManagerStarted)
        {
            isClearRoom = true;
            _portal.SetActive(true);
            _baseVFX.Stop();
            //print("Clear");
        }
        */
        
    }
    //----------------------------------------------------------------------------------------------- 
    public void MoveCoinsToPlayer()
    {
        for (int i = 0; i < _coinList.Count; i++)
        {
            if (GameManager.Instance.CoinList[i].activeInHierarchy)
            {
                GameManager.Instance.CoinList[i].transform.position = Vector3.MoveTowards(
                    GameManager.Instance.CoinList[i].transform.position,
                    player.transform.position,
                    10 * Time.deltaTime);
            }
        }
    }
    //----------------------------------------------------------------------------------------------- 
    public void MoveHealthGemsToPlayer()
    {
        for (int i = 0; i < _healthGemsList.Count; i++)
        {
            if (GameManager.Instance._healthGemsList[i].activeInHierarchy)
            {
                GameManager.Instance._healthGemsList[i].transform.position = Vector3.MoveTowards(
                    GameManager.Instance._healthGemsList[i].transform.position,
                    player.transform.position,
                    10 * Time.deltaTime);
            }
        }
    }

    //----------------------------------------------------------------------------------------------- 
    public void MoveRedGemsToPlayer()
    {
        for (int i = 0; i < _redGemsList.Count; i++)
        {
            if (GameManager.Instance._redGemsList[i].activeInHierarchy)
            {
                GameManager.Instance._redGemsList[i].transform.position = Vector3.MoveTowards(
                    GameManager.Instance._redGemsList[i].transform.position,
                    player.transform.position,
                    10 * Time.deltaTime);
            }
        }
    }
    //----------------------------------------------------------------------------------------------- 
    private void LateUpdate()
    {
        
    }


    //----------------------------------------------------------------------------------------------- 
    private void SpawnClock() // This method is used to calculate a time in which player is invincible while spawning
    {
        if (SpawnBot && _spawnTimer < _spawnBotInvincibelTime)
        {
            _spawnTimer  += Time.deltaTime;
        }
        else
        {
            _spawnTimer = 0f;
            SpawnBot = false;
        }
    }

    //----------------------------------------------------------------------------------------------- 
    public void RegisterEnemyOnTheGround(Transform enemy)
    {
        enemiesOnTheGround.Add(enemy);
    }

    public void RemoveEnemyOnTheGround(Transform enemy)
    {
        enemiesOnTheGround.Remove(enemy);
    }

    /*public void RegisterEnemy(Transform enemy)
    {
        _enemies.Add(enemy);
    }*/

    /*public void KilledEnemy(Transform enemy)
    {
        _killedEnemies.Add(enemy);
    }*/
    //----------------------------------------------------------------------------------------------- 
    //Bot stufrf
    [Header("Bots")] [SerializeField] private int _numberOfGemsToInstantiateBot = 10;

    public int NumberOfGemsToInstantiateBot => _numberOfGemsToInstantiateBot;
    
    [SerializeField] private float _spawnBotInvincibelTime = 0.5f;
    [SerializeField] private GameObject dragonBot;    public GameObject DragonBot => dragonBot;   
    [SerializeField] private GameObject botDragonBall;    public GameObject BotDragonBall => botDragonBall;
    
    [SerializeField] private GameObject soldierBot;    public GameObject SoldierBot => soldierBot;  
    
    [SerializeField] private GameObject _fireBot;
    
    
    private bool _spawnBot; // This variable is used to check if the player is currently spawning a bot, where the player should be invincible when spawning bots

    public bool SpawnBot
    {
        get { return _spawnBot; }
        set { _spawnBot = value; }
    }

    //----------------------------------------------------------------------------------------------- 
    //Allies stuff
    [FormerlySerializedAs("_allies")]
    [Header("Instantiated Allies")]
    [SerializeField] private  List<Transform> _allies = new List<Transform>(); public List<Transform> Allies => _allies;
    //private readonly List<Transform> _killedBots = new List<Transform>();

    public void RegisterAlly(Transform ally)
    {
        _allies.Add(ally);
    }

    public void RemoveAlly(Transform ally)
    {
        _allies.Remove(ally);
    }

//public void KilledBot(Transform bot)
//{
//    _killedBots.Add(bot);
//}
//----------------------------------------------------------------------------------------------- 

//Instantiated players stuff
    [Header("Instantiated players")]
    [SerializeField] private  List<Transform> _players = new List<Transform>();

    public List<Transform> Players => _players;
    //private readonly List<Transform> _killedBots = new List<Transform>();

    public void RegisterPlayer(Transform player)
    {
        _players.Add(player);
    }

    public void RemovePlayer(Transform player)
    {
        _players.Remove(player);
    }
//----------------------------------------------------------------------------------------------- 
//Instantiated Bots stuff
[Header("Instantiated Bots")]
[SerializeField] private  List<Transform> _bots = new List<Transform>();

public List<Transform> Bots => _bots;

public void RegisterBot(Transform bot)
{
    _bots.Add(bot);
}

public void RemoveBot(Transform bot)
{
    _bots.Remove(bot);
}
//----------------------------------------------------------------------------------------------- 


    public void PlayerHit(float currentHP) // function for checking the game over state 
        // HP is health point
    {
        if (currentHP > 0)
        {
            gameOver = false;
        }
        else
        {
            gameOver = true;
            _baseVFX.Stop();
            print("deactiviate base vfx");
            //StartCoroutine(EndGame("Game Over!"));
        }
    }

//-----------------------------------------------------------------------------------------------     
/*
     * next method is used to apply object pooling for ranger's arrows
     */
    private List<GameObject> _arrowList; //object pooling
    private GameObject _arrows; // empty gameobject for enclosing arrows

    public List<GameObject> ArrowList => _arrowList;

    private void ArrowObjectPooling()
    {
        //object pooling
        _arrowList = new List<GameObject>();
        _arrows = new GameObject("Arrows");
        _arrows.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 10; i++)
        {
            GameObject objArrow = (GameObject) Instantiate(Arrow);
            objArrow.SetActive(false);
            objArrow.name = "Arrow" + i;
            _arrowList.Add(objArrow);
            objArrow.transform.parent = _arrows.transform;
        }
    }
//-----------------------------------------------------------------------------------------------     

    
    //----------------------------------------------------------------------------------------------- 
/*
     * next method is used to apply object pooling for MicroDragon's dragonBall
     */
    private List<GameObject> _dragonBallsList; //object pooling
    private GameObject _dragonBalls; // empty gameobject for enclosing arrows

    public List<GameObject> DragonBallsList => _dragonBallsList;

    private void DragonBallObjectPooling()
    {
        //object pooling
        _dragonBallsList = new List<GameObject>();
        _dragonBalls = new GameObject("Dragon Balls");
        _dragonBalls.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 10; i++)
        {
            GameObject objDragonBall = (GameObject) Instantiate(dragonBall);
            objDragonBall.SetActive(false);
            _dragonBallsList.Add(objDragonBall);
            objDragonBall.transform.parent = _dragonBalls.transform;
        }
    }

//-----------------------------------------------------------------------------------------------     
    //----------------------------------------------------------------------------------------------- 
        /*
     * next method is used to apply object pooling for MicroDragon Bot
     */
    private List<GameObject> _dragonBallsBotList; //object pooling
    private GameObject _dragonBotBalls; // empty gameobject for enclosing arrows

    public List<GameObject> DragonBallsBotList => _dragonBallsBotList;

    private void DragonBallBotObjectPooling()
    {
        //object pooling
        _dragonBallsBotList = new List<GameObject>();
        _dragonBotBalls = new GameObject("Dragon Bot Balls");
        _dragonBotBalls.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 10; i++)
        {
            GameObject objDragonBall = (GameObject) Instantiate(DragonBall);
            objDragonBall.SetActive(false);
            _dragonBallsList.Add(objDragonBall);
            objDragonBall.transform.parent = _dragonBalls.transform;
        }
    }

    //----------------------------------------------------------------------------------------------- 
    /*
 * next method is used to apply object pooling for coins
 */
    private List<GameObject> _coinList; //object pooling
    private GameObject _coins; // empty gameobject for enclosing arrows

    public List<GameObject> CoinList => _coinList;

    private void CoinsPooling()
    {
        //object pooling
        _coinList = new List<GameObject>();
        _coins = new GameObject("Coinss");
        _coins.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 200; i++)
        {
            GameObject objCoin = (GameObject) Instantiate(_coin);
            objCoin.SetActive(false);
            _coinList.Add(objCoin);
            objCoin.transform.parent = _coins.transform;
        }
    }

//-----------------------------------------------------------------------------------------------     
//----------------------------------------------------------------------------------------------- 
    /*
 * next method is used to apply object pooling for Red Gems
 */
    private List<GameObject> _redGemsList; //object pooling
    private GameObject _redGems; // empty gameobject for enclosing arrows

    public List<GameObject> RedGemsList => _redGemsList;

    private void RedGemsPooling()
    {
        //object pooling
        _redGemsList = new List<GameObject>();
        _redGems = new GameObject("Red Gems");
        _redGems.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 200; i++)
        {
            GameObject objCoin = (GameObject) Instantiate(_redGem);
            objCoin.SetActive(false);
            _redGemsList.Add(objCoin);
            objCoin.transform.parent = _redGems.transform;
        }
    }

//-----------------------------------------------------------------------------------------------     
//----------------------------------------------------------------------------------------------- 
    /*
 * next method is used to apply object pooling for health gems
 */
    private List<GameObject> _healthGemsList; //object pooling
    private GameObject _healthGems; // empty gameobject for enclosing arrows

    public List<GameObject> HealthGemsList => _healthGemsList;

    private void HealthGemsPooling()
    {
        //object pooling
        _healthGemsList = new List<GameObject>();
        _healthGems = new GameObject("Health Gems");
        _healthGems.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 50; i++)
        {
            GameObject objGem = (GameObject) Instantiate(_healthGem);
            objGem.SetActive(false);
            _healthGemsList.Add(objGem);
            objGem.transform.parent = _healthGems.transform;
        }
    }

//-----------------------------------------------------------------------------------------------     

    /*
 * next method is used to apply object pooling for Cannon Bolls
 */
    
    private List<GameObject> _cannonBallsList; //object pooling
    private GameObject _cannonBalls; // empty gameobject for enclosing arrows

    public List<GameObject> CannonBallsList => _cannonBallsList;

    private void CannonBallObjectPooling()
    {
        //object pooling
        _cannonBallsList = new List<GameObject>();
        _cannonBalls = new GameObject("Cannon Balls");
        _cannonBalls.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 10; i++)
        {
            GameObject objcannonBall = (GameObject) Instantiate(cannonBall);
            objcannonBall.name = "Cannon Ball";
            objcannonBall.SetActive(false);
            _cannonBallsList.Add(objcannonBall);
            objcannonBall.transform.parent = _cannonBalls.transform;
        }
    }
//-----------------------------------------------------------------------------------------------     

    public void LoadNextLevel()
    {
        // StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        // _level += 1;
        // _gameManagerStarted = false;
        // _nextLevel = true;
        
        StartCoroutine(NextLevelCorotine());
        _level += 1;
        transform.Find("/Levels/").GetChild(_level).Find("Enemies On The Ground").gameObject.SetActive(true);
        
    }
//-----------------------------------------------------------------------------------------------     
    IEnumerator LoadLevel(int levelIndex)
    {
        _levelTransitionAnimator.SetTrigger("Level_Transition");
        //_portal.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(levelIndex);
        _levelTransitionAnimator.SetTrigger("Inverse_Level_Transition");
       
       MoveBotsToStartPostion(_startPostion);
       yield return new WaitForSeconds(0.5f);
       _nextLevel = false;
    }
    
    //-----------------------------------------------------------------------------------------------     
    IEnumerator NextLevelCorotine()
    {
        _levelTransitionAnimator.SetTrigger("Level_Transition");
        yield return new WaitForSeconds(2f);
        _levelTransitionAnimator.SetTrigger("Inverse_Level_Transition");
        yield return new WaitForSeconds(0.5f);
        _nextLevel = false;
    }
    //-----------------------------------------------------------------------------------------------        
    private int points = 0;
    private int j = 0;
    private int _zStartPos = 2;
    void MoveBotsToStartPostion(Transform startPos)
    {
        player.transform.position = startPos.position;
        for (int i = 1; i < _players.Count; i++)
        {
            if(points != 0)
            {
                points = -2 * (((j+1)/2) - (((int)Math.Pow(-1, (j+1))) - 1)/-4) *(points/Math.Abs(points));
            }
            else
            {
                points = (((j+1)/2) - (((int)Math.Pow(-1, (j+1))) - 1)/-4);
            }
            //print("j : " + j + " | Point : " + points);
            Vector3 pos = new Vector3(startPos.position.x + points, startPos.position.y, startPos.position.z + _zStartPos);
            GameManager.Instance.Allies[i].transform.position =
                new Vector3(startPos.position.x + points, startPos.position.y, startPos.position.z + _zStartPos);
           _allies[i].transform.SetPositionAndRotation(pos, Quaternion.identity);
            
            j += 1;
            if(j >= 8) {j = 0; _zStartPos += 2;}
        }
        _zStartPos = 2;
        j = 0;

        for (int i = 0; i < _bots.Count; i++)
        {
            Vector3 botPos = new Vector3(startPos.position.x, startPos.position.y, startPos.position.z + 4.25f);
            //_bots[i].transform.SetPositionAndRotation(botPos, Quaternion.identity);
            _bots[i].GetComponent<NavMeshAgent>().nextPosition = botPos;
        }
    }
//-----------------------------------------------------------------------------------------------      
    [Header("Bot Instructions")]
    [SerializeField]private Canvas _botInstructionCanvas;
    [SerializeField] private GameObject _pauseBtn;
    
    private bool _isBotInstructionsShowedBefore = false;
    public void ShowBotInstructions()
    {
        if (!_isBotInstructionsShowedBefore)
        {
            _botInstructionCanvas.gameObject.SetActive(true);
            Time.timeScale = 0f; // Stop the game
            GameIsPaused = true;
            _pauseBtn.SetActive(false);
            _isBotInstructionsShowedBefore = true;
        }

    }
    
    public void ResumeBotInstruction()
    {        
        print("Invoke...");
        _botInstructionCanvas.gameObject.SetActive(false);
        Time.timeScale =1f; // Return the game to play
        GameManager.Instance.GameIsPaused = false; 
        _pauseBtn.SetActive(true);
    }
//-----------------------------------------------------------------------------------------------       
    [Header("Healer")]
    [SerializeField]private Canvas _healerCanvas;

    private float _scale;
    public Canvas HealerCanvas => _healerCanvas;
    public void Healer()
    {        
        _healerCanvas.gameObject.SetActive(false);
        //Player.GetComponent<PlayerHealth>().IncreaseHealthWith(50f);
        StartCoroutine(IncreaseHealth(50));
        Player.GetComponent<PlayerHealth>().HealthSlider.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    IEnumerator IncreaseHealth(int val)
    {
        for (int i = 0; i < val; i++)
        {
            Player.GetComponent<PlayerHealth>().IncreaseHealthWith(1);
           _scale = Random.Range(1f, 1.5f);
            Player.GetComponent<PlayerHealth>().HealthSlider.GetComponent<RectTransform>().localScale = new Vector3(_scale, _scale, _scale);
            yield return new WaitForSeconds(0.005f);
        }
        
    }
//-----------------------------------------------------------------------------------------------        
    private void OnApplicationQuit()
    {
       // SystemSave.SavePlayer(this);  This did not work with android
       PlayerPrefs.SetInt("CoinsCount", _coinsCount);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        PlayerPrefs.SetInt("CoinsCount", _coinsCount);
        
    }
    
}

