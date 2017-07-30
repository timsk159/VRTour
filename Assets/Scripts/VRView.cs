using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField]
    Material sphereMat;


    private void Start()
    {
        IntroScreen.OnLoadingFinished += IntroScreen_OnLoadingFinished;
    }

    private void IntroScreen_OnLoadingFinished(VRViewAssets obj)
    {
        sphereMat.SetTexture("_MainTex", obj.TopTex);
        sphereMat.SetTexture("_SecondTex", obj.BotTex);
        
    }
}
