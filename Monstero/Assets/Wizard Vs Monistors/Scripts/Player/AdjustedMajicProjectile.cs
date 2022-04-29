using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AdjustedMajicProjectile : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject impactParticle;
    private GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
 
    private bool hasCollided = false;
 
    void Awake()
    {
        projectileParticle = this.gameObject.transform.GetChild(0).gameObject;
        impactParticle = this.gameObject.transform.GetChild(1).gameObject;
		if (muzzleParticle){
        muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
        Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
		}
    }

    private void OnEnable()
    {
        
        projectileParticle.SetActive(true);
        impactParticle.SetActive(false);
    }

    private void Update()
    {
    }

    void OnCollisionEnter(Collision hit)
    {
        hasCollided = true;
            StartCoroutine(ActivateImpact());
            //transform.DetachChildren();


            /*
            //Debug.DrawRay(hit.contacts[0].point, hit.contacts[0].normal * 1, Color.yellow);

            //yield WaitForSeconds (0.05);
            foreach (GameObject trail in trailParticles)
            {
                GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
                curTrail.transform.parent = null;
                Destroy(curTrail, 3f);
            }
            Destroy(projectileParticle, 3f);
            Destroy(impactParticle, 5f);
            //Destroy(gameObject);
            gameObject.SetActive(false);
            //projectileParticle.Stop();
			
			ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
            //Component at [0] is that of the parent i.e. this object (if there is any)
            for (int i = 1; i < trails.Length; i++)
            {
                ParticleSystem trail = trails[i];
                if (!trail.gameObject.name.Contains("Trail"))
                    continue;

                trail.transform.SetParent(null);
                Destroy(trail.gameObject, 2);
            }*/
    }

    IEnumerator ActivateImpact()
    {
        projectileParticle.SetActive(false);
        impactParticle.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }
    
}
