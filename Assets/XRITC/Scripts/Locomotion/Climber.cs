using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Climber : MonoBehaviour
{

    private CharacterController _character;
    public static XRController ClimbingHand;
    public bool EnableClimbing = true;
    private ContinuousMovement _continuousMovement;

    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<CharacterController>();
        _continuousMovement = GetComponent<ContinuousMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ClimbingHand && EnableClimbing)
        {
            _continuousMovement.enabled = false;
            Climb();
        }
        else
        {
            _continuousMovement.enabled = true;
        }

    }

    public bool SetClimbingActivation(bool value)
    {
        EnableClimbing = value;
        return EnableClimbing;
    }

    public bool ToggleClimbing()
    {
        EnableClimbing = !EnableClimbing;
        return EnableClimbing;
    }

    private void Climb()
    {
        InputDevices.GetDeviceAtXRNode(ClimbingHand.controllerNode)
            .TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        //Multiply with rotation and minus velocity to take player rotation when rotating the
        //character controller with the snap rotation
        _character.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
    }
}
