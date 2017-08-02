using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    private static AudioController instance;

    public static AudioController Instance { get { return instance; } }

    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    AudioSource oneShotSource;

    [SerializeField]
    AudioSource engineSource;

    private void Awake()
    {
        instance = this;
    }

    public void PlayEngine()
    {
        engineSource.time = 0;
        engineSource.Play();
        iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f, "to", 0.5f, "time", 0.7f, "onupdate", "OnEngineTweenUpdate"));
    }

    public void StopEngine()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", engineSource.volume, "to", 0.0f, "time", 0.7f, "onupdate", "OnEngineTweenUpdate", "oncomplete", "OnEngineTweenFinish"));
    }

    void OnEngineTweenUpdate(float newVol)
    {
        engineSource.volume = newVol;
    }

    void OnEngineTweenFinish()
    {
        engineSource.Stop();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.time = 0;
        musicSource.Play();
        iTween.ValueTo(gameObject, iTween.Hash("from", 0.0f, "to", 1.0f, "time", 0.7f, "onupdate", "OnMusicTweenUpdate"));
    }

    public void StopMusic()
    {
        iTween.ValueTo(gameObject, iTween.Hash("from", musicSource.volume, "to", 0.0f, "time", 0.7f, "onupdate", "OnMusicTweenUpdate", "oncomplete", "OnMusicTweenFinish"));
    }

    void OnMusicTweenUpdate(float newVol)
    {
        musicSource.volume = newVol;
    }

    void OnMusicTweenFinish()
    {
        musicSource.Stop();
    }

    public void PlayOneShot(AudioClip clip)
    {
        oneShotSource.PlayOneShot(clip);
    }

}
