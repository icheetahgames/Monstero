using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGem : MonoBehaviour
{
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            this.GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);
            StartCoroutine(DisableCoin(0.5f));
        }
    }

    IEnumerator DisableCoin(float time)
    {
        yield return new WaitForSeconds(time);
        this.transform.parent.transform.position = Vector3.zero;
        this.transform.parent.gameObject.SetActive(false);
        GameManager.Instance.RedGemsCount += 1;
        if (GameManager.Instance.RedGemsCount >= GameManager.Instance.NumberOfGemsToInstantiateBot)
        {
            GameManager.Instance.Player.GetComponent<PlayerAttack>().BotFire();
            GameManager.Instance.RedGemsCount = 0;
            GameManager.Instance.ShowBotInstructions();
        }
    }
}
