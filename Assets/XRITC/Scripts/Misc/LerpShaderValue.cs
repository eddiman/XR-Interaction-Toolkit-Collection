using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LerpShaderValue : MonoBehaviour
{
    public string ShaderValueName;
    public float lerpDuration = 3;
    public float lerpDelay = .5f;
    public float StartValue = 0;
    public float lerpInStartValue = 0;
    public float lerpInEndValue = 1;
    public float lerpOutStartValue = 1;
    public float lerpOutEndValue = 0;
    public bool activateEvent;
    public bool activateOnEnable = true;

    public UnityEvent LerpEnd;

    float valueToLerp;
    private Renderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<Renderer> ();
        if (!activateOnEnable) return;
        meshRenderer.material.SetFloat(ShaderValueName, StartValue);
        LerpOut();
    }
    void OnEnable()
    {
        if (!activateOnEnable) return;
        try
        {
            meshRenderer.material.SetFloat(ShaderValueName, StartValue);

        }
        catch
        {
            //TODO: Fix this
            Debug.Log(
                "Funky error, but works, TODO: Fix this");
        }
        LerpOut();
    }

    public void SetShaderFloatValue(float value)
    {
        meshRenderer.material.SetFloat(ShaderValueName, value);
    }
    public void LerpIn()
    {
        if(!isActiveAndEnabled) return;
        StartCoroutine(Lerp(lerpDelay, lerpInStartValue, lerpInEndValue, activateEvent));
    }
    public void LerpOut()
    {
        if(!isActiveAndEnabled) return;
        StartCoroutine(Lerp(lerpDelay, lerpOutStartValue, lerpOutEndValue, activateEvent));
    }

    IEnumerator Lerp(float delay, float startValue, float endValue, bool activateEvent)
    {
        float timeElapsed = 0;
        yield return new WaitForSeconds(delay);

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            meshRenderer.material.SetFloat(ShaderValueName, valueToLerp);
            yield return null;
        }

        if (activateEvent)
        {
            LerpEnd.Invoke();
        }

        valueToLerp = endValue;
    }

}
