using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{
    [Header("Remember to set Collider from the child in 'Mesh' to Attach Transform/Colliders")]
    private Vector3 _initialAttachLocalPos;
    private Quaternion _initialAttachLocalRot;
    private void Start()
    {
        //Create attach point if not exists
        if (!attachTransform)
        {
            GameObject grab = new GameObject("Grab Pivot");
            grab.transform.SetParent(transform,false);
            attachTransform = grab.transform;
        }

        _initialAttachLocalPos = attachTransform.localPosition;
        _initialAttachLocalRot = attachTransform.localRotation;
    }
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        if (interactor is XRDirectInteractor)
        {
            attachTransform.position = interactor.transform.position;
            attachTransform.rotation = interactor.transform.rotation;
        }
        else
        {
            attachTransform.localPosition = _initialAttachLocalPos;
            attachTransform.localRotation = _initialAttachLocalRot;
        }
        base.OnSelectEntered(interactor);
    }
}
