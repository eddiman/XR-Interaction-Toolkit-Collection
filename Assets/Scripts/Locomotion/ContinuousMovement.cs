using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public float speed = 1;
    public float gravity = -9.81f;
    public XRNode InputSource;
    public LayerMask groundLayer;
    public float additionalHeight = 0.2f; //20cm
    public bool enableContinuousMovement = true;

        public float FOVRestrictorThreshold = 0.1f; //How much of the stick before the fov restrictor is activated


    public UnityEvent StartedMoving;
    public UnityEvent StoppedMoving;

    private bool _isGrounded = true;
    private bool _isMoving = false;


    private Vector2 _inputAxis;
    private float _fallingSpeed;
    private XRRig _rig;

    //Layer on this object has to be "Player"
    private CharacterController _character;
    // Start is called before the first frame update
    void Start()
    {
        _character = GetComponent<CharacterController>();
        _rig = GetComponent<XRRig>();
        _isGrounded = CheckIfGrounded();
    }

    // Update is called once per framemenu
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(InputSource);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis);
    }

    void FixedUpdate()
    {
        CapsuleFollowHeadSet();
        Quaternion headYaw = Quaternion.Euler(0, _rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(_inputAxis.x, 0, _inputAxis.y);
        if (enableContinuousMovement)
        {
            _character.Move(direction * Time.deltaTime * speed);
            Debug.Log("X " + _inputAxis.x);
            Debug.Log("Y " + _inputAxis.y);
            //TODO: Fix floating comparison, works on Oculus Touch, but other controller might be more floaty with its input values, just remember to check for positive AND negative values
            if(Math.Abs(_inputAxis.y) > FOVRestrictorThreshold && _isMoving == false)
            {
                _isMoving = true;
                StartedMoving.Invoke();
            } else if(Math.Abs(_inputAxis.x) > FOVRestrictorThreshold && _isMoving == false)
            {
                _isMoving = true;
                StartedMoving.Invoke();
            } else if (Math.Abs(_inputAxis.x) < FOVRestrictorThreshold
                && Math.Abs(_inputAxis.y) < FOVRestrictorThreshold
                && _isMoving)
            {
                _isMoving = false;
                StoppedMoving.Invoke();
            }
        }

        //Gravity
        _isGrounded = CheckIfGrounded();
        if (_isGrounded)
        {
            _fallingSpeed = 0;
        }
        else
        {

            _fallingSpeed += gravity * Time.fixedDeltaTime;
        }

        _character.Move(Vector3.up * _fallingSpeed * Time.fixedDeltaTime);

    }
    public bool SetContinuousMovementActivation(bool value)
    {
        enableContinuousMovement = value;
        return enableContinuousMovement;
    }

    public bool ToggleContinuousMovement()
    {
        enableContinuousMovement = !enableContinuousMovement;
        return enableContinuousMovement;
    }

    private void CapsuleFollowHeadSet()
    {
        _character.height = _rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(_rig.cameraGameObject.transform.position);
        _character.center = new Vector3(capsuleCenter.x, _character.height/2 + _character.skinWidth, capsuleCenter.z);
    }
    private bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(_character.center);
        float rayLength = _character.center.y + 0.01f;

        bool hasHit = Physics.SphereCast(rayStart, _character.radius, Vector3.down, out RaycastHit hitInfo, rayLength,
            groundLayer);
        return hasHit;
    }
}
