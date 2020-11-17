using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScaler : MonoBehaviour
{
    public AudioSource audio;
    public float _volume = 2;

    [SerializeField] private bool _isClipPlaying;

    private float _refValue = .1f;
    private float _rmsValue;
    private float _dbValue;
    private int _qSamples = 1024;

    private Vector3 _initScale;
    private Vector3 _newScale;
    private Vector3 _oldScale;

    private float[] _samples;
    // Start is called before the first frame update
    void Start()
    {
        _samples = new float[_qSamples];
        _initScale = transform.localScale;
        _isClipPlaying = true;
    }

    private void GetVolume()
    {
        audio.GetOutputData(_samples, 0); // fill array with samples
        float sum = 0f;
        for (int i=0; i < _qSamples; i++){
            sum += _samples[i]*_samples[i]; // sum squared samples
        }
        _rmsValue = Mathf.Sqrt(sum/_qSamples); // rms = square root of average
        _dbValue = 20*Mathf.Log10(_rmsValue/_refValue); // calculate dB
        if (_dbValue < -160) _dbValue = -160; // clamp it to -160dB min
        Debug.Log(_dbValue);
    }
    // Update is called once per frame
    void Update()
    {
        if (audio.isPlaying)
        {
            _isClipPlaying = true;
            GetVolume();


            transform.localScale = Vector3.Lerp(_initScale, _newScale, Time.deltaTime * 2f );
            _newScale = new Vector3((_volume * _rmsValue), (_volume * _rmsValue), _volume * _rmsValue );
        }

    }
}
