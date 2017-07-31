using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class VRViewAssets
{
    public Texture2D TopTex { get; private set; }
    public Texture2D BotTex { get; private set; }
    public AudioClip Audio { get; private set; }

    public VRViewAssets(Texture2D topTex, Texture2D botTex, AudioClip audio)
    {
        this.TopTex = topTex; this.BotTex = botTex; this.Audio = audio;
    }

}

public class VRView : MonoBehaviour
{
    public static event Action OnFinished;

    [SerializeField]
    Material sphereMat;

    [SerializeField]
    CanvasGroup menuGroup;

    [SerializeField]
    Button backButton;

    private void Start()
    {
        menuGroup.blocksRaycasts = false;
        menuGroup.alpha = 0;
        IntroScreen.OnLoadingFinished += IntroScreen_OnLoadingFinished;
        backButton.onClick.AddListener(HandleBackButtonPressed);
    }

    private void HandleBackButtonPressed()
    {
        menuGroup.blocksRaycasts = false;
        iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", 0.0f, "time", 0.4f, "onupdate", "FadeGroupUpdate"));
        if (OnFinished != null)
            OnFinished();
    }

    private void IntroScreen_OnLoadingFinished(VRViewAssets obj)
    {
        sphereMat.SetTexture("_MainTex", obj.TopTex);
        sphereMat.SetTexture("_SecondTex", obj.BotTex);

        menuGroup.blocksRaycasts = true;
        iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f, "to", 1.0f, "time", 0.4f, "onupdate", "FadeGroupUpdate"));
    }

    void FadeGroupUpdate(float alpha)
    {
        menuGroup.alpha = alpha;
    }
}
