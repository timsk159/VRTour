using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IntroScreen : MonoBehaviour
{
    public static event Action<PhotoSphereAssets> OnLoadingFinished;

    [SerializeField]
    VRModeSwitcher vrModeSwitcher;

    [SerializeField]
    List<ImageAudioButton> imageAudioButs;

    [SerializeField]
    CanvasGroup gridGroup;

    [SerializeField]
    AudioClip bellClip;

    private void Start()
    {
        foreach(var but in imageAudioButs)
        {
            but.OnTapped += HandleImageAudioClicked;
        }
        PhotoSphereView.OnFinished += VRView_OnFinished;
        AudioController.Instance.PlayEngine();
    }

    private void VRView_OnFinished()
    {
        AudioController.Instance.StopMusic();
        AudioController.Instance.PlayEngine();
        gridGroup.blocksRaycasts = true;
        iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f, "to", 1.0f, "time", 0.6f, "onupdate", "FadeGroupUpdate"));
    }

    private void HandleImageAudioClicked(ImageAudioButton obj)
    {
        //Display loading.
        StartCoroutine(FinishIntroRoutine(obj));

    }

    private void LoadingFinished(PhotoSphereAssets assets)
    {
        //Hide Loading.

        AudioController.Instance.PlayMusic(assets.Audio);

        if (OnLoadingFinished != null)
            OnLoadingFinished(assets);
    }

    IEnumerator FinishIntroRoutine(ImageAudioButton obj)
    {
        gridGroup.blocksRaycasts = false;
        iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", 0.0f, "time", 0.6f, "onupdate", "FadeGroupUpdate"));
        yield return StartCoroutine(AudioController.Instance.PlaySequenceRoutine(bellClip, obj.clickClip));
        AudioController.Instance.StopEngine();

        StartCoroutine(obj.LoadRoutine(LoadingFinished));
    }

    private void FadeGroupUpdate(float alpha)
    {
        gridGroup.alpha = alpha;
    }

    public void ToggleSoundPressed()
    {
        AudioController.Instance.ToggleMute();
    }

    public void VRModePressed()
    {
        vrModeSwitcher.MagicWindowButtonPressed();
    }

    public void WebsitePressed()
    {
        Application.OpenURL("www.google.com");
    }

    public void MapsPressed()
    {

    }
}

