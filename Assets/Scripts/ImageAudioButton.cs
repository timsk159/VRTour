using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageAudioButton : MonoBehaviour
{
    [Header("Resources")]
    [SerializeField]
    string img_b;
    [SerializeField]
    string img_d;
    [SerializeField]
    string img_f;
    [SerializeField]
    string img_l;
    [SerializeField]
    string img_r;
    [SerializeField]
    string img_u;

    [SerializeField]
    string audioClipName;

    [Space()]

    [Header("Assets")]

    [SerializeField]
    public AudioClip clickClip;

    [Space()]

    [Header("GameObjects")]
    [SerializeField]
    Button button;
    [SerializeField]
    Image thumbnailImage;
    
    public Action OnHoverStart;
    public Action OnHoverEnd;
    public Action<ImageAudioButton> OnTapped;

    public void Start()
    {
        button.onClick.AddListener(OnButtonClicked);
        var hoverStart = new UnityEngine.EventSystems.EventTrigger.Entry() { eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter };
        var hoverEnd = new UnityEngine.EventSystems.EventTrigger.Entry() { eventID = UnityEngine.EventSystems.EventTriggerType.PointerExit };
        hoverStart.callback.AddListener(FireHoverStart);
        hoverEnd.callback.AddListener(FireHoverEnd);
        var trig = button.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
        trig.triggers.Add(hoverStart);
        trig.triggers.Add(hoverEnd);
    }

    private void FireHoverEnd(BaseEventData arg0)
    {
        if (OnHoverEnd != null)
            OnHoverEnd();
    }

    private void FireHoverStart(BaseEventData arg0)
    {
        if (OnHoverStart != null)
            OnHoverStart();
    }

    private void OnButtonClicked()
    {
        if (OnTapped != null)
            OnTapped(this);
    }

    public void LoadAsync(MonoBehaviour host, Action<PhotoSphereAssets> onComplete)
    {
        host.StartCoroutine(LoadRoutine(onComplete));
    }

    public IEnumerator LoadRoutine(Action<PhotoSphereAssets> onComplete)
    {
        var loadReq1 = Resources.LoadAsync<Texture2D>(img_b);
        var loadReq2 = Resources.LoadAsync<Texture2D>(img_d);
        var loadReq3 = Resources.LoadAsync<Texture2D>(img_f);
        var loadReq4 = Resources.LoadAsync<Texture2D>(img_l);
        var loadReq5 = Resources.LoadAsync<Texture2D>(img_r);
        var loadReq6 = Resources.LoadAsync<Texture2D>(img_u);
        var loadReqAudio = Resources.LoadAsync<AudioClip>(audioClipName);

        while (!loadReq1.isDone && !loadReq2.isDone && !loadReq3.isDone
            && !loadReq4.isDone && !loadReq5.isDone && !loadReq6.isDone 
            && !loadReqAudio.isDone)
        {
            yield return null;
        }

        var assets = new PhotoSphereAssets((Texture2D)loadReq1.asset, (Texture2D)loadReq2.asset,
            (Texture2D)loadReq3.asset, (Texture2D)loadReq4.asset,
            (Texture2D)loadReq5.asset, (Texture2D)loadReq6.asset, 
            (AudioClip)loadReqAudio.asset);
        onComplete(assets);
    }
}
