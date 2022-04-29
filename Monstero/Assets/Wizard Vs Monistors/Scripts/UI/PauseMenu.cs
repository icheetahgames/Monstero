using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class PauseMenu : MonoBehaviour
{



    [SerializeField]private GameObject PauseMenuUI;

    [SerializeField] private GameObject _pauseBtn;

    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private GameObject _playSound;
    
    [SerializeField] private GameObject _mute;
    
    [SerializeField] private GameObject _invincibleBTN;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        //Pause();
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Stop the game
        GameManager.Instance.GameIsPaused = true;
        _pauseBtn.SetActive(false);
    }

    public void Resume()
    {        
        PauseMenuUI.SetActive(false);
        Time.timeScale =1f; // Return the game to play
        GameManager.Instance.GameIsPaused = false; 
        _pauseBtn.SetActive(true);
    }
    

    public void Home()
    {
        print("Home");
        Application.Quit();
    }

    public void Mute()
    {
        if (GameManager.Instance.IsMuted == false)
        {
            _audioMixer.SetFloat("Volume", -100);
            GameManager.Instance.IsMuted = true;
            _playSound.SetActive(false);
            _mute.SetActive(true);
            
        }
        else
        {
            _audioMixer.SetFloat("Volume", 0);
            GameManager.Instance.IsMuted = false;
            _mute.SetActive(false);
            _playSound.SetActive(true);
        }
    }

    public void Invincible()
    {
        if (GameManager.Instance.Invincible == false)
        {
            GameManager.Instance.Invincible = true;
            _invincibleBTN.GetComponent<Image>().color = Color.Lerp(Color.white, Color.red, 0.5f);
        }
        else
        {
            GameManager.Instance.Invincible = false;
            _invincibleBTN.GetComponent<Image>().color = Color.Lerp(Color.white, Color.blue, 0.5f);
        }
    }

}
