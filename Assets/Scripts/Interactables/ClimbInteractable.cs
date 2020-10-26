using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractable : XRBaseInteractable
{
    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {
        base.OnSelectEnter(interactor);

        if (interactor is XRDirectInteractor)
        {
            Climber.ClimbingHand = interactor.GetComponent<XRController>();

        }
    }

    protected override void OnSelectExit(XRBaseInteractor interactor)
    {
        base.OnSelectExit(interactor);
        if (interactor is XRDirectInteractor)
        {
            if (Climber.ClimbingHand && Climber.ClimbingHand.name == interactor.name)
            {
                Climber.ClimbingHand = null;
            }
        }
    }

}
