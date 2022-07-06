using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAnimationEvents : MonoBehaviour
{
    [SerializeField] private ParticleSystem _firstParticleSystem;
    [SerializeField] private ParticleSystem _secondParticleSystem;

    private AudioSource _audioSource;
    //This Script is created to put all animation events with empty functionality to be received by the animation clips and do nothing
    //This script is normal used with objects such as trailer enemies
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = _firstParticleSystem.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //----------------------------------------------------------------------------------------------- 

    void EnemyStartAttack() //Animation Event
    {
        if (_firstParticleSystem )
        {
            _firstParticleSystem.Play();
        }

        if (_audioSource)
        {
            _audioSource.Play();
        }
    }
//----------------------------------------------------------------------------------------------- 

    void EnemyEndAttack() //Animation Event
    {

    }
//----------------------------------------------------------------------------------------------- 
}
