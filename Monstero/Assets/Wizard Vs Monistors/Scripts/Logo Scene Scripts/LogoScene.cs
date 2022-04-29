using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScene : MonoBehaviour
{
    [SerializeField] private float _logoSceneTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveToNextScene(_logoSceneTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MoveToNextScene(float thisLevelStayingTime)
    {
        yield return new WaitForSeconds(thisLevelStayingTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
