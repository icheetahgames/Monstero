using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class TouchJoystick : MonoBehaviour
{

    //This this the most perfect code I have seen in my life(Do not need any change)
    // يضاف الى camera
    //يقوم بالتحكم بعصى التحكم
    //و ارسال القيم المتغيرات المطلوبة الى PlayerController
 
    #region Public Variables
    #endregion
//----------------------------------------------------------------------------------------------- 
    #region SerializeField variables
    [SerializeField] private GameObject _bigStick;
    [SerializeField] private GameObject _smallStick;
    #endregion
//-----------------------------------------------------------------------------------------------
    #region Class Variables(private - Only related to this class)    

    private RectTransform _bigStickRectTransform;
    private RectTransform _smallStickRectTransform;
    
    
    private float _stickRadius;
    private Vector3 _joyStickFirstPosition;
    private Vector3 _originalPosition;
    
    private RawImage _bigStickImage;
    private RawImage _smallStickImage;

    private Color _startColor;
    private float _endAlpha ; 
    
    private float _stickDistance;

    private Vector3 dragPosition;
    
    #endregion
//-----------------------------------------------------------------------------------------------

    #region ColoringVariables
    private Color _bigcurrColor;
    private Color _smallcurrColor;
    private Color _bigStartColor;
    private Color _smallStartColor;
    private float _onDragAlpha = 0.75f; //1 is opaque, 0 is transparent
    private float EndAlpha ; 
    #endregion
//-----------------------------------------------------------------------------------------------
    private void Awake()
    {
       Assert.IsNotNull(_bigStick); 
            
    }
//-----------------------------------------------------------------------------------------------
    void Start()
    {
        _stickRadius = _bigStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2; // half of the height
        _bigStickRectTransform = _bigStick.GetComponent<RectTransform>();
        _smallStickRectTransform = _smallStick.GetComponent<RectTransform>();
        

        
        _originalPosition = _bigStickRectTransform.position;
        _joyStickFirstPosition = _bigStickRectTransform.position;
        

        
        _bigStickImage = _bigStick.GetComponent<RawImage>();
        _smallStickImage = _smallStick.GetComponent<RawImage>();
        
        
        _bigStartColor = _bigStickImage.color;
        _smallStartColor = _smallStickImage.color;
        EndAlpha = _bigStickImage.color.a;
    }
//-----------------------------------------------------------------------------------------------
    private void Update()
    {
        if (GameManager.Instance.GameOver || GameManager.Instance.GameIsPaused)
        {
            return;
        }
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);


                if (t.phase == TouchPhase.Began)
                {
                    _bigStickRectTransform.position = Input.touches[i].position;
                    _smallStickRectTransform.position = Input.touches[i].position;
                    _joyStickFirstPosition = Input.touches[i].position;
                }

                else if (t.phase == TouchPhase.Moved)
                {
                    dragPosition = Input.touches[i].position;
                    SmallStickToBigStickCalcuations(dragPosition);
                    SmallStickAndPlayerMovement();
                    ColorChangingWhileDraging();
                }
                else if (t.phase == TouchPhase.Ended)
                {
                    GameManager.Instance.JoyVec = Vector3.zero;
                    _smallStick.transform.position = _joyStickFirstPosition;
                    _bigStick.transform.position = _originalPosition;


                    _bigStickImage.color = _bigStartColor;
                    _smallStickImage.color = _smallStartColor;
                }

        }
    }

//-----------------------------------------------------------------------------------------------
    private void SmallStickToBigStickCalcuations(Vector3 dragPosition)
    {
        GameManager.Instance.JoyVec = (dragPosition - _joyStickFirstPosition).normalized; //joyVec is always between 0 and 1(x, y, 0)
                                                                                            // also JoyVec effecting the player's move direction

        _stickDistance = Vector3.Distance(dragPosition, _joyStickFirstPosition); // طول الوتر بين اول نقطة ضغظ الى النقطة التي عندها المؤشر

    }

//-----------------------------------------------------------------------------------------------
    private void SmallStickAndPlayerMovement() 
    {
        if (_stickDistance < _stickRadius)
        { // if the drag value is within big stick
            // move the small stick to StickDistance as a fraction in term of JoyVec(x, y)
            _smallStick.transform.position = _joyStickFirstPosition + GameManager.Instance.JoyVec * _stickDistance;
        }
        else
        {// if the drag value is bigger than big stick size then stay inside the big stick
            _smallStick.transform.position = _joyStickFirstPosition + GameManager.Instance.JoyVec * _stickRadius;
        }
        
        GameManager.Instance.Bias = _smallStick.transform.localPosition 
                                    / _stickRadius; // _bais is used to make the player as fast as the joystick draged
    }

//-----------------------------------------------------------------------------------------------
    private void ColorChangingWhileDraging()
    { // this code will just increase the alpha of the sticks while draging
        _bigcurrColor.r = _bigStickImage.color.r;
        _bigcurrColor.g = _bigStickImage.color.g;
        _bigcurrColor.b = _bigStickImage.color.b;
        _bigcurrColor.a = _onDragAlpha;
        
        _smallcurrColor.r = _smallStickImage.color.r;
        _smallcurrColor.g = _smallStickImage.color.g;
        _smallcurrColor.b = _smallStickImage.color.b;
        
        _smallcurrColor.a = _onDragAlpha;
        _bigStickImage.color = _bigcurrColor;
        _smallStickImage.color = _smallcurrColor;
    }
}
