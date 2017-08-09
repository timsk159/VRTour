using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PhotoSphereAssets
{
    public Texture2D Tex_b { get; private set; }
    public Texture2D Tex_d { get; private set; }
    public Texture2D Tex_f { get; private set; }
    public Texture2D Tex_l { get; private set; }
    public Texture2D Tex_r { get; private set; }
    public Texture2D Tex_u { get; private set; }
    public AudioClip Audio { get; private set; }

    public PhotoSphereAssets(Texture2D tex_b, Texture2D tex_d, Texture2D tex_f
        , Texture2D tex_l, Texture2D tex_r, Texture2D tex_u, AudioClip audio)
    {
        this.Tex_b = tex_b; this.Tex_d = tex_d; this.Tex_f = tex_f;
        this.Tex_l = tex_l; this.Tex_r = tex_r; this.Tex_u = tex_u;
        this.Audio = audio;
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
        sphereMat.SetTexture("_FrontTex", obj.Tex_f);
        sphereMat.SetTexture("_BackTex", obj.Tex_b);
        sphereMat.SetTexture("_LeftTex", obj.Tex_l);
        sphereMat.SetTexture("_RightTex", obj.Tex_r);
        sphereMat.SetTexture("_UpTex", obj.Tex_u);
        sphereMat.SetTexture("_DownTex", obj.Tex_d);

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
        var col = sphereMat.GetColor("_Tint");
        col.r = alpha;
        col.g = alpha;
        col.b = alpha;
        col.a = alpha;
        sphereMat.SetColor("_Tint", col);
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
