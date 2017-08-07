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
    string imageTopName;
    [SerializeField]
    string imageBotName;
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
        var loadReqTop = Resources.LoadAsync<Texture2D>(imageTopName);
        var loadReqBot = Resources.LoadAsync<Texture2D>(imageBotName);
        var loadReqAudio = Resources.LoadAsync<AudioClip>(audioClipName);

        while (!loadReqTop.isDone && !loadReqBot.isDone && !loadReqAudio.isDone)
        {
            yield return null;
        }

        var assets = new PhotoSphereAssets((Texture2D)loadReqTop.asset, (Texture2D)loadReqBot.asset, (AudioClip)loadReqAudio.asset);
        onComplete(assets);
    }
}
