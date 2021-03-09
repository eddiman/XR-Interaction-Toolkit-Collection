using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvent : MonoBehaviour
{
    public float waitSeconds;
    public bool runOnStart;
    public AudioScaler AudioScalerForSoundEnd;
    public UnityEvent onTimerStart;
    public UnityEvent onTimerFinished;

    void Start()
    {
        if (!runOnStart) return;
        StartTimer();
        StartCoroutine(Timer(waitSeconds));
    }
    public void StartTimer()
    {
        onTimerStart.Invoke();
        if(AudioScalerForSoundEnd)
        StartCoroutine(Timer(waitSeconds));
    }

    private IEnumerator Timer(float waitSeconds)
    {
        float coroutineWaitSeconds = waitSeconds;
        yield return new WaitForSeconds(coroutineWaitSeconds);
        onTimerFinished.Invoke();
    }
}
