using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IntroScreen : MonoBehaviour
{
    public static event Action<VRViewAssets> OnLoadingFinished;

    [SerializeField]
    List<ImageAudioButton> imageAudioButs;

    [SerializeField]
    CanvasGroup gridGroup;

    private void Start()
    {
        foreach(var but in imageAudioButs)
        {
            but.OnTapped += HandleImageAudioClicked;
        }
    }

    private void HandleImageAudioClicked(ImageAudioButton obj)
    {
        //Display loading.

        iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", 0.0f, "time", 0.4f, "onupdate", "FadeGroupUpdate"));

        StartCoroutine(obj.LoadRoutine(LoadingFinished));
    }

    private void LoadingFinished(VRViewAssets assets)
    {
        //Hide Loading.

        if(OnLoadingFinished != null)
            OnLoadingFinished(assets);
    }

    private void FadeGroupUpdate(float alpha)
    {
        gridGroup.alpha = alpha;
    }
}

