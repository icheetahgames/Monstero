using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBotAndWizardMyHandMoveGesture : MonoBehaviour
{
    [SerializeField] private GameObject _smoke1;
    [SerializeField] private GameObject _bot;
    [SerializeField] private GameObject _smoke2;
    [SerializeField] private GameObject _wizard;

    // Start is called before the first frame update
    void Start()
    {
        _smoke1.SetActive(false);
        _bot.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowBot()
    {
        StartCoroutine(ShowBotSquence());
    }

    IEnumerator ShowBotSquence()
    {
        _smoke1.SetActive(true);
        yield return new WaitForSeconds(.25f);
        _bot.SetActive(true);

    }

}
