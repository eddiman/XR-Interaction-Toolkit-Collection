using UnityEngine;
using UnityEngine.Serialization;

namespace XRITC.Scripts.Misc
{
    public class FollowObject : MonoBehaviour
    {
        [FormerlySerializedAs("Target")] public Transform target;
        public Transform rotationTarget;
        private Vector3 relativePos;
        public bool rotate;

        public Vector3 Offset;
        // Start is called before the first frame update
        void Start()
        {
            Offset = transform.position - target.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, target.position + Offset, Time.deltaTime * 10);
            if (rotate != true) return;
            transform.rotation = rotationTarget.rotation;
        }
    }
}
