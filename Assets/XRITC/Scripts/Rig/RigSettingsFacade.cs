using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using XRITC.Scripts.Locomotion;

namespace Assets.Scripts
{
    public class RigSettingsFacade : MonoBehaviour
    {
        private ContinuousMovement _continuousMovement;
        private TeleportationController _teleportationController;
        private Climber _climber;
        private SnapTurnController _snapTurnController;
        [ReadOnly]
        [Tooltip("This script is supposed to give you all the functions for using with events. " +
                 "Also, it allows you to control certain functions of the VR Rig. " +
                 "Use the buttons for disabling and enabling key functions")]
        public string HoverThisToSeeEntireTextUnderneath = "";
        [Header("This script is supposed to give you all the functions for using with events. " +
                "Also, it allows you to control certain functions of the VR Rig. " +
                "Use the buttons for disabling and enabling key functions")]

        [Tooltip("Insert Left/Ray Interactors here, if missing")]
        public List<XRRayInteractor> XRRayInteractors;
        [Tooltip("Use this for setting the mask of the Left/Right Ray Interactors")]
        public LayerMask XRRayInteractorMask;
        [HideInInspector]
        [SerializeField]
        private bool _canMoveContinuously;
        [HideInInspector]
        [SerializeField]
        private bool _canClimb;
        [HideInInspector]
        [SerializeField]
        private bool _canTeleport;
        [HideInInspector]
        [SerializeField]
        private bool _enableRayForUiOnly;
        [HideInInspector]
        [SerializeField]
        private bool _canSnapTurn;

        void Awake()
        {
            _continuousMovement = GetComponent<ContinuousMovement>();
            _teleportationController = GetComponent<TeleportationController>();
            _climber = GetComponent<Climber>();
            _snapTurnController = GetComponent<SnapTurnController>();

            _continuousMovement.enableContinuousMovement = _canMoveContinuously;
            _climber.EnableClimbing = _canClimb;
            _teleportationController.EnableTeleport = _canTeleport;
            EnableRayForUiOnly = _enableRayForUiOnly;
            _snapTurnController.SnapTurnIsOn = _canSnapTurn;



        }

        public bool CanMoveContinuously
        {

            get { return _canMoveContinuously; }
            set
            {
                _canMoveContinuously = value;
                try
                {
                    _continuousMovement.SetContinuousMovementActivation(_canMoveContinuously);

                }
                catch
                {
                    Debug.Log("Oak's words echoed... 'Remember, this does not affect the value in the editor, only runtime'");
                }
            }
        }
        public bool CanClimb
        {
            get { return _canClimb; }
            set
            {
                _canClimb = value;
                try
                {
                    _climber.SetClimbingActivation(_canClimb);
                }
                catch
                {
                    Debug.Log("Oak's words echoed... 'Remember, this does not affect the value in the editor, only runtime'");

                }
            }
        }
        public bool CanTeleport
        {
            get { return _canTeleport; }
            set
            {
                _canTeleport = value;
                try
                {
                    _teleportationController.SetTeleportActivation(_canTeleport);
                }
                catch
                {
                    Debug.Log("Oak's words echoed... 'Remember, this does not affect the value in the editor, only runtime'");

                }
            }
        }
        public bool EnableRayForUiOnly
        {
            get { return _enableRayForUiOnly; }
            set
            {
                _enableRayForUiOnly = value;
                try
                {
                    foreach (var item in XRRayInteractors)
                    {
                        LayerMask layerMask = new LayerMask();

                        if (_enableRayForUiOnly)
                        {
                            layerMask |= (1 << LayerMask.NameToLayer("UI"));
                            item.raycastMask = layerMask;
                        }
                        else
                        {
                            item.raycastMask = XRRayInteractorMask;
                        }
                    }
                }
                catch
                {
                    Debug.Log("Oak's words echoed... 'Remember, this does not affect the value in the editor, only runtime'");

                }
            }
        }

        public bool CanSnapTurn
        {
            get { return _canSnapTurn; }
            set
            {
                _canSnapTurn = value;
                try
                {
                    _snapTurnController.SetSnapTurnActivation(_canSnapTurn);
                }
                catch
                {
                    Debug.Log("Oak's words echoed... 'Remember, this does not affect the value in the editor, only runtime'");

                }
            }
        }


    }

#if UNITY_EDITOR
    [CustomEditor(typeof(RigSettingsFacade))]
    public class RigSettingsFacadeEditor : Editor
    {
        public RigSettingsFacade script;

        public void OnEnable()
        {
            script = (RigSettingsFacade)target;
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool contMovementTarget = !script.CanMoveContinuously;
            GUI.backgroundColor = getBtnColor(contMovementTarget);
            if(GUILayout.Button(btnText("Continuous movement", script.CanMoveContinuously, contMovementTarget)))
            {
                script.CanMoveContinuously = contMovementTarget;
            }

            bool canClimbTarget = !script.CanClimb;
            GUI.backgroundColor = (canClimbTarget) ? Color.red : Color.green;
            if(GUILayout.Button(btnText("Climbing ", script.CanClimb, canClimbTarget)))
            {
                script.CanClimb = canClimbTarget;
            }

            bool canTeleportTarget = !script.CanTeleport;
            GUI.backgroundColor = (canTeleportTarget) ? Color.red : Color.green;
            if(GUILayout.Button(btnText("Teleporting ", script.CanTeleport, canTeleportTarget)))
            {
                script.CanTeleport = canTeleportTarget;
            }

            bool enableRayForUiOnlyTarget = !script.EnableRayForUiOnly;
            GUI.backgroundColor = (enableRayForUiOnlyTarget) ? Color.red : Color.green;
            if(GUILayout.Button(btnText("Interactor for only UI ", script.EnableRayForUiOnly, enableRayForUiOnlyTarget)))
            {
                script.EnableRayForUiOnly = enableRayForUiOnlyTarget;
            }
            bool snapTurnRotateTarget = !script.CanSnapTurn;
            GUI.backgroundColor = (snapTurnRotateTarget) ? Color.red : Color.green;
            if(GUILayout.Button(btnText("Snap turn ", script.CanSnapTurn, snapTurnRotateTarget)))
            {
                script.CanSnapTurn = snapTurnRotateTarget;
            }

            EditorUtility.SetDirty(script);
            GUI.backgroundColor = Color.white;


        }

        public Color getBtnColor(bool cond)
        {
            return GUI.backgroundColor = (cond) ? Color.red : Color.green;
        }

        public string btnText(string ActionText, bool valueBool, bool targetBool)
        {
            return ActionText + " is " + (valueBool ? "activated": "deactivated") + " (Click to " + (targetBool ? "activate": "deactivate") + ")";
        }


    }

#endif
}
