using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRInteractableConfigurator))]
public class XRGrabNoPlayerCollideInteractable : XRGrabInteractable
{
    public bool allowRotationOffset;
    public bool allowPositionOffset;
    private XRInteractableConfigurator _xrConfig;
    private XRInteractionManager manager;
    private LayerMask _originalLayer;
    private string noColliderLayerName = "NoPlayerCollide";
    private XRBaseInteractor controller;

    private Quaternion _interactorRotation = Quaternion.identity;
    private Vector3 _interactorPosition = Vector3.zero;
    protected override void Awake()
    {
        _xrConfig = GetComponent<XRInteractableConfigurator>();
        allowRotationOffset = _xrConfig.allowOffsetGrabRotation;
        allowPositionOffset = _xrConfig.allowOffsetGrabPosition;
        noColliderLayerName = _xrConfig.noColliderLayerName;
        manager = _xrConfig.manager;
        controller = _xrConfig.controller;

        base.Awake();
    }

    private void Update()
    {
        //manager.GetValidTargets(controller, manager.)
    }

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        _originalLayer = gameObject.layer;
        if (LayerMask.NameToLayer(noColliderLayerName) > -1)
        {
            gameObject.layer = LayerMask.NameToLayer(noColliderLayerName);
            foreach (var child in colliders)
            {
                child.gameObject.layer = LayerMask.NameToLayer(noColliderLayerName);
            }
        }

        if (allowRotationOffset || allowPositionOffset)
        {
            Storelnteractor(args.interactor);
            MatchAttachmentPoints(args.interactor);
        }

    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        Debug.Log(args.interactable.gameObject.layer);
        Debug.Log(args.interactable);
        Debug.Log("args.interactable.gameObject.name");
        base.OnHoverEntered(args);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);

        gameObject.layer = _originalLayer;
        foreach (var child in colliders)
        {
            child.gameObject.layer = _originalLayer;
        }

        /*Due to a weird grabbing bug, where the interactable object doesnt correctly dergister when SelectExiting is performed,
        usually with a high magnitude on the interactable (e.g. throwing we unregister and register the interactable from the interaction manager's list of valig targets.)

        Unregistering also removes it from the hand interactors list of current target which is the base of the problem. It still "there" wven when you drop the item in high speeds.

        Not sure if this is a bug in the XR Interaction Toolkit or not.
        */
        args.interactor.interactionManager.UnregisterInteractable(args.interactable);

        Debug.Log("resetting offset");
        ResetAttachmentPoint(args.interactor);
        Clearinteractor(args.interactor);
        args.interactor.interactionManager.RegisterInteractable(args.interactable);

    }

    private void Storelnteractor(XRBaseInteractor interactor)
    {
        if (allowRotationOffset && !allowPositionOffset)
        {
            _interactorRotation = interactor.attachTransform.localRotation;
        }
        else if (allowPositionOffset && !allowPositionOffset)
        {
            _interactorPosition = interactor.attachTransform.localPosition;
        }
        else if (allowRotationOffset && allowPositionOffset)
        {
            _interactorPosition = interactor.attachTransform.localPosition;
            _interactorRotation = interactor.attachTransform.localRotation;
        }
    }

    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        bool hasAttach = attachTransform != null;
        Debug.Log(hasAttach);
        if (allowRotationOffset && !allowPositionOffset)
        {
            interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
        }
        else if (allowPositionOffset && !allowRotationOffset)
        {
            interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        }
        else if (allowRotationOffset && allowPositionOffset)
        {
            interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
            interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;

        }

    }

    private void ResetAttachmentPoint(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localRotation = _interactorRotation;
        interactor.attachTransform.localPosition = _interactorPosition;
        if (allowRotationOffset & !allowPositionOffset)
        {
            interactor.attachTransform.localRotation = _interactorRotation;
        }
        else if (allowPositionOffset && !allowRotationOffset)
        {
            interactor.attachTransform.localPosition = _interactorPosition;
        }
        else if (allowRotationOffset && allowPositionOffset)
        {
            interactor.attachTransform.localRotation = _interactorRotation;
            interactor.attachTransform.localPosition = _interactorPosition;

        }

    }

    private void Clearinteractor(XRBaseInteractor interactor)
    {
        _interactorRotation = Quaternion.identity;
        _interactorPosition = Vector3.zero;

        if (allowRotationOffset && !allowPositionOffset)
        {
            _interactorRotation = Quaternion.identity;
        }
        else if (allowPositionOffset && !allowRotationOffset)
        {
            _interactorPosition = Vector3.zero;
        }
        else if (allowRotationOffset && allowPositionOffset)
        {
            _interactorRotation = Quaternion.identity;;
            _interactorPosition = Vector3.zero;

        }
    }
}
