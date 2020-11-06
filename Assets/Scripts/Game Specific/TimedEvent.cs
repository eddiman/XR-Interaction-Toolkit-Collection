using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedEvent : MonoBehaviour
{
    public float waitSeconds;
    public UnityEvent onTimerStart;
    public UnityEvent onTimerFinished;

    public void StartTimer()
    {

        onTimerStart.Invoke();
        StartCoroutine(Timer(waitSeconds));
    }

    private IEnumerator Timer(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        onTimerFinished.Invoke();
    }
}
