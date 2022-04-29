using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GeneralMovementAttack
{
    // Start is called before the first frame update
    [SerializeField] private Rigidbody _bulletPrefab;
    [SerializeField] private GameObject _cursor;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _timeBetweenShoots = 1f;
    private float _timer;

    private CannonHealth _cannonHealth;
    void Start()
    {
        _cannonHealth = this.GetComponent<CannonHealth>();
        CannonBallObjectPooling();
    }

    // Update is called once per frame
    void Update()
    {
        if (_cannonHealth._isalive && !GameManager.Instance.GameOver)
        {
            Clock();
            LaunchProjectile();
        }
    }

    void Clock()
    {
        _timer += Time.deltaTime;
    }
    void LaunchProjectile()
    {
 //       _cursor.SetActive(true);
 //       _cursor.transform.position = GameManager.Instance.Player.transform.position;

        Vector3 Vo = CalculateVelocity(GetClosestEnemy(GameManager.Instance.Allies).position, _shotPoint.position, 1f);

        
        
        if (_timer >= _timeBetweenShoots)
        {
            transform.rotation = Quaternion.LookRotation(Vo);
            Fire(Vo);
            _timer = 0;
        }
    }

    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        // define the distance x and y first
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;
        
        // create  a float the represent our distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }


    private GameObject _cannonBallTothrow;
    void Fire(Vector3 throwVeloctiy )
    {
        // Object pooling
        for (int i = 0; i < _cannonBallsList.Count; i++)
        {
            if (!_cannonBallsList[i].activeInHierarchy)
            {
                _cannonBallTothrow = _cannonBallsList[i];
                _cannonBallTothrow.transform.position = _shotPoint.position;
                _cannonBallTothrow.transform.rotation = transform.rotation;
                _cannonBallTothrow.SetActive(true);
                _cannonBallTothrow.GetComponent<Rigidbody>().velocity = throwVeloctiy;
                break;
            }
        }
    }
    
    
    //-----------------------------------------------------------------------------------------------     
    /*
 * next method is used to apply object pooling for Cannon Bolls
 */
    [SerializeField] private GameObject cannonBall;
    private List<GameObject> _cannonBallsList; //object pooling
    private GameObject _cannonBalls; // empty gameobject for enclosing arrows

    public List<GameObject> CannonBallsList => _cannonBallsList;

    private void CannonBallObjectPooling()
    {
        //object pooling
        _cannonBallsList = new List<GameObject>();
        _cannonBalls = new GameObject("Cannon Balls");
        _cannonBalls.transform.parent = this.gameObject.transform;
        for (int i = 0; i < 5; i++)
        {
            GameObject objcannonBall = (GameObject) Instantiate(cannonBall);
            objcannonBall.name = "Cannon Ball";
            objcannonBall.SetActive(false);
            _cannonBallsList.Add(objcannonBall);
            objcannonBall.transform.parent = _cannonBalls.transform;
        }
    }
//-----------------------------------------------------------------------------------------------    
}
