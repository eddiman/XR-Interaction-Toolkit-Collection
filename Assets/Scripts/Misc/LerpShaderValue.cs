using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpShaderValue : MonoBehaviour
{
    public string ShaderValueName;
    public float lerpDuration = 3;
    public float lerpDelay = .5f;
    public float lerpInStartValue = 0;
    public float lerpInEndValue = 1;
    public float lerpOutStartValue = 1;
    public float lerpOutEndValue = 0;

    float valueToLerp;
    private Renderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<Renderer> ();
        meshRenderer.material.SetFloat(ShaderValueName, 1);
        LerpOut();
    }
  void OnEnable()
    {
        try
        {
            meshRenderer.material.SetFloat(ShaderValueName, 1);

        }
        catch
        {
            //TODO: Fix this
            Debug.Log(
                "Funky error, but works, TODO: Fix this");
        }
        LerpOut();
    }
    public void LerpIn()
    {
        StartCoroutine(Lerp(lerpDelay, lerpInStartValue, lerpInEndValue));
    }
    public void LerpOut()
    {
        StartCoroutine(Lerp(lerpDelay, lerpOutStartValue, lerpOutEndValue));
    }

    IEnumerator Lerp(float delay, float startValue, float endValue)
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

        valueToLerp = endValue;
    }

}
