using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class TeleportationController : MonoBehaviour
{
    public XRController LeftTeleportRayXRController, RightTeleportRayXRController;
    public InputHelpers.Button TeleportActivationButton;
    public float ActivationThreshold = 0.001f;

    public XRRayInteractor leftInteractorRay;
    public XRRayInteractor rightInteractorRay;

    public bool EnableTeleport = true;
    public bool EnableTeleportLeftHand = true;
    public bool EnableTeleportRightHand = true;

    //These are used in the XR Direct Interactor in "Show Interactor Events" of the Right/Left hand
    //They are used to deactivate the teleporter when something is grabbed
    public bool EnableLeftTeleport { get; set; } = true;
    public bool EnableRightTeleport { get; set; } = true;

    private XRRayInteractor _leftTeleportRayInteractor;
    private XRRayInteractor _rightTeleportRayInteractor;
    private void Start()
    {
        if (LeftTeleportRayXRController)
        {
            _leftTeleportRayInteractor = LeftTeleportRayXRController.gameObject.GetComponent<XRRayInteractor>();
        }

        if (RightTeleportRayXRController)
        {

            _rightTeleportRayInteractor = RightTeleportRayXRController.gameObject.GetComponent<XRRayInteractor>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;
        if (LeftTeleportRayXRController)
        {
            bool isLeftInteractorRayHovering =
                leftInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);

            _leftTeleportRayInteractor.allowSelect = CheckIfActivated(LeftTeleportRayXRController) && !isLeftInteractorRayHovering && EnableTeleport && EnableTeleportLeftHand;
            LeftTeleportRayXRController.gameObject.SetActive(EnableLeftTeleport && CheckIfActivated(LeftTeleportRayXRController) && !isLeftInteractorRayHovering && EnableTeleport && EnableTeleportLeftHand);
        }
        if (RightTeleportRayXRController)
        {
            bool isRightInteractorRayHovering =
                rightInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget);
            _rightTeleportRayInteractor.allowSelect = CheckIfActivated(RightTeleportRayXRController) && !isRightInteractorRayHovering && EnableTeleport && EnableTeleportRightHand;
            RightTeleportRayXRController.gameObject.SetActive(EnableRightTeleport && CheckIfActivated(RightTeleportRayXRController) && !isRightInteractorRayHovering && EnableTeleport && EnableTeleportRightHand);
        }

        if (!EnableTeleport && _leftTeleportRayInteractor.gameObject.activeSelf && _rightTeleportRayInteractor.gameObject.activeSelf)
        {
            SetTeleportActivation(false);
        }
    }

    /*
     * Use preferably as a one time calling, i.e. in event calls
     *
     */
    public bool SetTeleportActivation(bool value)
    {
        EnableTeleport = value;
        _leftTeleportRayInteractor.gameObject.SetActive(value);
        _rightTeleportRayInteractor.gameObject.SetActive(value);
        return EnableTeleport;
    }

    public bool ToggleTeleportActivation()
    {
        EnableTeleport = !EnableTeleport;
        _leftTeleportRayInteractor.gameObject.SetActive(EnableTeleport);
        _rightTeleportRayInteractor.gameObject.SetActive(EnableTeleport);
        return EnableTeleport;
    }
    private bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, TeleportActivationButton, out bool isActivated, ActivationThreshold);
        return isActivated;
    }
}
