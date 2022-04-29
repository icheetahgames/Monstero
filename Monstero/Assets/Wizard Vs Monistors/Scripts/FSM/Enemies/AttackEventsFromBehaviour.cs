using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEventsFromBehaviour : MonoBehaviour
{
    private BoxCollider[] weaponColliders;
    private AudioSource _attackAudio, _fireEnemiesAduio;
    private  ParticleSystem _attackParticle, _fireEnemiesPartilce;
    private Animator _anim;
    private void Start()
    {
        weaponColliders = GetComponentsInChildren<BoxCollider>();
        _anim = GetComponent<Animator>();
        if (transform.Find("WeaponParticles"))
        {
            _attackParticle = transform.Find("WeaponParticles").GetComponent<ParticleSystem>();
            _attackAudio = transform.Find("WeaponParticles").GetComponent<AudioSource>();
            _attackParticle.Pause();
            print("Weapon Particle is founded");
        }



        if (transform.Find("FireEnemies"))
        {
            _fireEnemiesPartilce = transform.Find("FireEnemies").GetComponent<ParticleSystem>();
            _fireEnemiesAduio = transform.Find("FireEnemies").GetComponent<AudioSource>();
            _fireEnemiesPartilce.Pause();
        }

    }

    void EnemyStartAttack() //Animation Event
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = true;
        }

        if (_attackParticle )
        {
            _attackParticle.Play();
        }

        if (_attackAudio)
        {
            _attackAudio.Play();
        }

    }
//----------------------------------------------------------------------------------------------- 

    void EnemyEndAttack() //Animation Event
    {
        foreach (var weapon in weaponColliders)
        {
            weapon.enabled = false;
        }
    }
//----------------------------------------------------------------------------------------------- 
    public void FireEnemies()
    {
        _fireEnemiesPartilce.Play();
        _fireEnemiesAduio.Play();
    }
 //----------------------------------------------------------------------------------------------- 
 private void Update()
 {
     if (Input.GetKeyDown(KeyCode.M))
     {
         _anim.Play("Idle");
     }
 }
}
