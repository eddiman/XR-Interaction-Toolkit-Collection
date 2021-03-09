using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapTurnController : MonoBehaviour
{
    public DeviceBasedSnapTurnProvider snapTurnProvider;

    public bool SnapTurnIsOn = true;
    [Tooltip("The higher this is the faster it rotates")]
    public float newTurnAmount = 2.75f;

    [SerializeField]
    private float _originalTurnAmount = 45f;

    [SerializeField]
    private float _originalActivationTimeout = .2f;



    void Start()
    {
        SetSnapTurnActivation(SnapTurnIsOn);
    }
    public bool SetSnapTurnActivation(bool snap)
    {
        SnapTurnIsOn = snap;
        if (SnapTurnIsOn)
        {
            snapTurnProvider.turnAmount = _originalTurnAmount;
            snapTurnProvider.debounceTime = _originalActivationTimeout;

        }
        else
        {
            snapTurnProvider.turnAmount = newTurnAmount;
            snapTurnProvider.debounceTime = 0;
        }

        return SnapTurnIsOn;
    }
}
