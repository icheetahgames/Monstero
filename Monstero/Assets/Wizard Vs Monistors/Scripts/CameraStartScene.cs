using System;
using System.Collections;
using System.Collections.Generic;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraStartScene : MonoBehaviour
{
    public GameObject PostProcessing;

    private DepthOfField depthOfField;

    private Volume _volume;
    private ClampedFloatParameter focalLength;
    private void Start()
    {
        _volume = PostProcessing.GetComponent<Volume>();
        _volume.profile.TryGet<DepthOfField>(out depthOfField);
        focalLength = depthOfField.focalLength;
    }



    public void CallMakeScreenBlury()
    {
        StartCoroutine(MakeScreenBlurry());
    }

    IEnumerator MakeScreenBlurry()
    {
        for (int i = 1; i < 20; i++)
        {
            focalLength.value = i;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void CallMakeScreenBlurryForBotInstruction()
    {
        StartCoroutine(MakeScreenBlurryForBotInstruction(0.001f));
    }
    IEnumerator MakeScreenBlurryForBotInstruction(float delay)
    {
        for (int i = 1; i < 20; i++)
        {
            focalLength.value = i;
            yield return new WaitForSeconds(delay);
        }
    }


}
