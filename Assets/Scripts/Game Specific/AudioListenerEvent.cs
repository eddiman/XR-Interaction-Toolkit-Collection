using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioListenerEvent : MonoBehaviour
{
    public float delay;
    public bool runOnStart;
    public AudioScaler AudioScalerForSoundEnd;
    public UnityEvent onSoundStartEvent;
    public UnityEvent onSoundEndEvent;

    void Start()
    {
        if (!runOnStart) return;
        onSoundStart();
        StartCoroutine(Timer(delay));
    }
    public void onSoundStart()
    {
        onSoundStartEvent.Invoke();
        StartCoroutine(Timer(delay));
    }
    public void onSoundEnd()
    {
        if (AudioScalerForSoundEnd)
        {
            AudioScalerForSoundEnd.SoundEnded.AddListener(onSoundStart);
        }
        StartCoroutine(Timer(delay));
    }

    private IEnumerator Timer(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        AudioScalerForSoundEnd.SoundEnded.RemoveListener(onSoundStart);
        onSoundEndEvent.Invoke();
    }
}
