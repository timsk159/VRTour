using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PhotoSphereAssets
{
    public Texture2D TopTex { get; private set; }
    public Texture2D BotTex { get; private set; }
    public AudioClip Audio { get; private set; }

    public PhotoSphereAssets(Texture2D topTex, Texture2D botTex, AudioClip audio)
    {
        this.TopTex = topTex; this.BotTex = botTex; this.Audio = audio;
    }

}

public class PhotoSphereView : MonoBehaviour
{
    public static event Action OnFinished;

    [SerializeField]
    Material sphereMat;

    [SerializeField]
    CanvasGroup menuGroup;

    [SerializeField]
    Button backButton;

    bool viewing;

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
        iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", 0.0f, "time", 0.6f, "onupdate", "FadeGroupUpdate"));
        iTween.ValueTo(gameObject, iTween.Hash("from", 1.0f, "to", 0.0f, "time", 0.6f, "onupdate", "FadeSphereUpdate"));
        if (OnFinished != null)
            OnFinished();
        viewing = false;
    }

    private void IntroScreen_OnLoadingFinished(PhotoSphereAssets obj)
    {
        viewing = true;
        sphereMat.SetTexture("_MainTex", obj.TopTex);
        sphereMat.SetTexture("_SecondTex", obj.BotTex);

        menuGroup.blocksRaycasts = true;
        iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f, "to", 1.0f, "time", 0.6f, "onupdate", "FadeGroupUpdate"));
        iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f, "to", 1.0f, "time", 0.6f, "onupdate", "FadeSphereUpdate"));
    }

    void FadeGroupUpdate(float alpha)
    {
        menuGroup.alpha = alpha;
    }

    void FadeSphereUpdate(float alpha)
    {
        var col = sphereMat.GetColor("_Color");
        col.a = alpha;
        sphereMat.SetColor("_Color", col);
    }

    void Update()
    {
        if (viewing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                HandleBackButtonPressed();
            }
        }
    }
}
