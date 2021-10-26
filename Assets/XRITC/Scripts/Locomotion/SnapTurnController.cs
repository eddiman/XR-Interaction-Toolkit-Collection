using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace XRITC.Scripts.Locomotion
{
    public class SnapTurnController : MonoBehaviour
    {
        public DeviceBasedSnapTurnProvider snapTurnProvider;
        public DeviceBasedContinuousTurnProvider continuousTurnProvider;

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
                snapTurnProvider.enabled = true;
                continuousTurnProvider.enabled = false;

            }
            else
            {
                snapTurnProvider.enabled = false;
                continuousTurnProvider.enabled = true;
            }

            return SnapTurnIsOn;
        }
    }
}
