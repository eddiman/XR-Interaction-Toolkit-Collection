using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRITC.Scripts.Interactables
{
    public class XROffsetGrabInteractable : XRGrabInteractable
    {
        private Vector3 _interactorPosition = Vector3.zero;
        private Quaternion _interactorRotation = Quaternion.identity;
        private LayerMask _originalLayer;
        private string noColliderLayerName = "NoPlayerCollide";
        
        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            var interactor = args.interactor;
            //Remember to set "NoPlayerCollide" layer to no collide in physics matrix Project Settings > Physics
            //Checks if layer exists, and then sets GO and colliders in XRGrabInteractable to not collide with player
            _originalLayer = gameObject.layer;
            if (LayerMask.NameToLayer(noColliderLayerName) > -1)
            {
                gameObject.layer = LayerMask.NameToLayer(noColliderLayerName);
                foreach (var child in colliders)
                {
                    child.gameObject.layer = LayerMask.NameToLayer(noColliderLayerName);
                }
            }

            base.OnSelectEntered(args);
            StoreInteractor(interactor);
            MatchAttachmentPoints(interactor);
        }

        private void StoreInteractor(XRBaseInteractor interactor)
        {
            _interactorPosition = interactor.attachTransform.localPosition;
            _interactorRotation = interactor.attachTransform.localRotation;
        }

        private void MatchAttachmentPoints(XRBaseInteractor interactor)
        {
  
            bool hasAttach = attachTransform != null;
            interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
            interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
        }
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            var interactor = args.interactor;
            gameObject.layer = _originalLayer;
            foreach (var child in colliders)
            {
                child.gameObject.layer = _originalLayer;
            }

            base.OnSelectExited(args);
            ClearInteractor();
            ResetAttachmentPoint(interactor);
        }

        private void ResetAttachmentPoint(XRBaseInteractor interactor)
        {
            interactor.attachTransform.transform.localPosition = _interactorPosition;
            interactor.attachTransform.transform.localRotation = _interactorRotation;
        }

        private void ClearInteractor()
        {
            _interactorPosition = Vector3.zero;
            _interactorRotation = Quaternion.identity;
        }

    }
}