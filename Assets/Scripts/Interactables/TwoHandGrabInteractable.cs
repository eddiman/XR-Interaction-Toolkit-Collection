using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    [SerializeField]
    public XRBaseInteractor _selectingInteractor;
    public XRBaseInteractor _secondInteractor;
    private Quaternion _attachInitialRotation;
    private Quaternion _attachSecondHandInitialRotation;
    public enum TwoHandRotationType {None, First, Second}

    public TwoHandRotationType twoHandRotationType;
    public bool snapToSecondHand = true;

    private Quaternion _initialRotationOffset;
    [SerializeField]
    private bool _isAlreadyGrabbed;

    // Start is called before the first frame update
    void Start()
    {

        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEnter.AddListener(OnSecondHandGrab);
            item.onSelectExit.AddListener(OnSecondHandRelease);
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        _selectingInteractor = selectingInteractor;
        if (_secondInteractor && selectingInteractor && !_secondInteractor.GetComponent<XRSocketInteractor>())
        {
            //Rotate two hand grabbable
            if (snapToSecondHand)
            {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            }
            else {
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * _initialRotationOffset;
            }
        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation()
    {

        Quaternion targetRotation = new Quaternion();
        switch (twoHandRotationType)
        {
            case TwoHandRotationType.None:
                targetRotation = Quaternion.
                    LookRotation(_secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
                break;

            case TwoHandRotationType.First:
                targetRotation = Quaternion.
                    LookRotation(_secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.transform.up);
                break;
            case TwoHandRotationType.Second:
                targetRotation = Quaternion.
                    LookRotation(_secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, _secondInteractor.attachTransform.up);

                break;
        }
        return targetRotation;
    }

    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        _secondInteractor = interactor;
            _initialRotationOffset =
                Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
            _attachSecondHandInitialRotation = interactor.attachTransform.localRotation;


    }
    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        _secondInteractor = null;
        interactor.attachTransform.localRotation = _attachSecondHandInitialRotation;

    }

    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {

        base.OnSelectEnter(interactor);
        _attachInitialRotation = interactor.attachTransform.localRotation;
    }

    protected override void OnSelectExit(XRBaseInteractor interactor)
    {
        base.OnSelectExit(interactor);
        _secondInteractor = null;
        interactor.attachTransform.localRotation = _attachInitialRotation;


    }
/*
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        _isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor) && !selectingInteractor.GetComponent<XRSocketInteractor>();
        return base.IsSelectableBy(interactor) && !_isAlreadyGrabbed;
    }*/
}
