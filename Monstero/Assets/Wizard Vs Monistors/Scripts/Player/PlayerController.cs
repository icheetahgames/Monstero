using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    /* This code is the same as PlayerController Except it will make the player controlled by joystick in android and double the player speed on android platform*/
    
    #region Public Variables
    #endregion
    //----------------------------------------------------------------------------------------------- 
    #region SerializeField variables
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private float _androidMoveSpeed = 10.0f;
    [SerializeField] private Text _text;
    [SerializeField] private float _touchJoystickSpeedMultiplier = 1f;
    [SerializeField]private AudioSource _audioSource;
    #endregion
    //-----------------------------------------------------------------------------------------------
    #region Cashed variables(components)
    private CharacterController _characterController;
    private Animator _anim;
    #endregion
    //-----------------------------------------------------------------------------------------------
    #region Class Variables(private-Only related to this class)    
    private Vector3 _moveDirection;    
    private float _horizontalMove;
    private float _verticalMove;
    private Vector3 _facingRotatoin;
    private int points = 0;
    private int j = 0;
    private int _zStartPos = 2;
    #endregion
    //-----------------------------------------------------------------------------------------------
    #region Getters & Setters functions
    #endregion
    //----------------------------------------------------------------------------------------------- 
    private void Awake()
    {
#if UNITY_EDITOR
        
#elif PLATFORM_ANDROID
        _moveSpeed = _androidMoveSpeed;
#endif
    }
    //----------------------------------------------------------------------------------------------- 
    // Start is called before the first frame update
    void Start()
    {
        #region Cahsed Variables Definition
        _characterController = GetComponent<CharacterController>();
        _anim = this.GetComponent<Animator>();
        #endregion
        // if (GameManager.Instance.Players.Count > 1)
        // {
        //     this.transform.position = GameManager.Instance.FirePos.transform.position;
        // }
        //GameManager.Instance.nextStage = false;
        _anim.Play("Spawn");
    }
    //----------------------------------------------------------------------------------------------- 
    // Update is called once per frame
    void Update()
    {
        // AdditonalAttacks();
    }
    //----------------------------------------------------------------------------------------------- 
    
    void FixedUpdate()
    {
#if UNITY_EDITOR
        _horizontalMove = Input.GetAxis("Horizontal") * _moveSpeed;
        _verticalMove = Input.GetAxis("Vertical") * _moveSpeed;
        _moveDirection = new Vector3(_horizontalMove, 0, _verticalMove);

        if (_moveDirection != Vector3.zero)
        {
            _anim.SetBool("IsWalking", true);
            GameManager.Instance.IsWalking = true;
        }
        else
        {
            _anim.SetBool("IsWalking", false);
            GameManager.Instance.IsWalking = false;
        }
        
        if (!GameManager.Instance.GameOver && GameManager.Instance.IsWalking && _characterController.enabled)
        {
           
            _facingRotatoin = Vector3.Normalize(new Vector3(_horizontalMove,
                0, 
                _verticalMove));

            if (_facingRotatoin != Vector3.zero) //This condition prevents from spamming "Look rotation viewing vector is zero" when not moving.
            {
                transform.forward = _facingRotatoin;
            }   
        }
#elif PLATFORM_ANDROID
        //_text.text = "Bias x" + GameManager.Instance.Bias.x;
if (GameManager.Instance.JoyVec.x >= GameManager.Instance.JoyStickSensitiviy) 
        {   //joyVec is always between 0 and 1(x, y, 0)
            // also JoyVec effecting the player's move direction
            _horizontalMove = _touchJoystickSpeedMultiplier * _moveSpeed * GameManager.Instance.Bias.x; //_bais is used to make the player as fast as the joystick draged
            _anim.SetBool("IsWalking", true);
            GameManager.Instance.IsWalking = true;
        }
        else if (GameManager.Instance.JoyVec.x <= -GameManager.Instance.JoyStickSensitiviy)
        {
            _horizontalMove = _touchJoystickSpeedMultiplier *  _moveSpeed * GameManager.Instance.Bias.x;
            _anim.SetBool("IsWalking", true);
            GameManager.Instance.IsWalking = true;
        }
        else
        {
            _horizontalMove = 0;
            _anim.SetBool("IsWalking", false);
            GameManager.Instance.IsWalking = false;
        }
        
        
        
        if (GameManager.Instance.JoyVec.y >= GameManager.Instance.JoyStickSensitiviy)
        {
            _verticalMove = _touchJoystickSpeedMultiplier *  _moveSpeed * GameManager.Instance.Bias.y;
            _anim.SetBool("IsWalking", true);
            GameManager.Instance.IsWalking = true;
        }
        else if (GameManager.Instance.JoyVec.y <= -GameManager.Instance.JoyStickSensitiviy)
        {
            _verticalMove = _touchJoystickSpeedMultiplier *  _moveSpeed * GameManager.Instance.Bias.y;
            _anim.SetBool("IsWalking", true);
            GameManager.Instance.IsWalking = true;
        }
        else
        {
            _verticalMove = 0;
            _anim.SetBool("IsWalking", false);
            GameManager.Instance.IsWalking = false;
        }
    
    
    if (!GameManager.Instance.GameOver && GameManager.Instance.IsWalking)
        {
             _facingRotatoin = Vector3.Normalize(new Vector3(GameManager.Instance.JoyVec.x,
                0, 
                GameManager.Instance.JoyVec.y));

            if (_facingRotatoin != Vector3.zero) //This condition prevents from spamming "Look rotation viewing vector is zero" when not moving.
            {
                transform.forward = _facingRotatoin;
            } 
        }
#endif
       if(!_anim.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
       {
         _moveDirection = new Vector3(_horizontalMove, 0, _verticalMove);
       }     
           
        
        
        
        
/*
 * the following approach used when all levels was in one scene
 *  if (LevelsManager.instance.nextStage)
        {
            //StageMGN.instance.NextStage();
            LevelsManager.instance.nextStage = false;
        }
        else
        {
//            //_moveDirection = new Vector3(0f, 0f, 15f);
           _characterController.SimpleMove(_moveDirection);
        }
 */
        if (!GameManager.Instance.NextLevel && _characterController.enabled)
        {
            _characterController.SimpleMove(_moveDirection);
        }
    }

  //   private void OnTriggerEnter(Collider other)
  //   {
  //       if (other.tag == "Portal")
  //       {
  //           print("----------------------------------------------------------------------------------------------------------------------------------");
  //           print("Next Stage");
  //           // LevelsManager.instance.NextStage();
  //           // LevelsManager.instance.nextStage = true;
  //           GameManager.Instance.LoadNextLevel();
  // //          StartCoroutine(MoveToStartPostion(0.45f, GameManager.Instance.StartPostion));
  //       }
  //   }

    IEnumerator MoveToStartPostion(float delay, Transform startPos)
    {
        yield return new WaitForSeconds(delay);
        this.transform.position = startPos.position;

        for (int i = 0; i < GameManager.Instance.Allies.Count; i++)
        {
            if(points != 0)
            {
            points = -2 * (((j+1)/2) - (((int)Math.Pow(-1, (j+1))) - 1)/-4) *(points/Math.Abs(points));
            }
            else
            {
                points = (((j+1)/2) - (((int)Math.Pow(-1, (j+1))) - 1)/-4);
            }
            print("j : " + j + " | Point : " + points);
            GameManager.Instance.Allies[i].transform.position =
                new Vector3(startPos.position.x + points, startPos.position.y, startPos.position.z + _zStartPos);

            j += 1;
            if(j >= 8) {j = 0; _zStartPos += 2;}
        }
        _zStartPos += 2;
    }
}
