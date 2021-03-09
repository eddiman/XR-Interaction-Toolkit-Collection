using UnityEngine;

// Makes objects float up & down while gently spinning.
namespace Assets.Scripts.Game_Specific
{
    public class Floater : MonoBehaviour {
        // User Inputs
        public float degreesPerSecond = 15.0f;
        public float amplitude = 0.5f;
        public float frequency = 1f;

        // Position Storage Variables
        private Vector3 _posOffset;
        private Vector3 _tempPos;

        // Use this for initialization
        void Start () {
            // Store the starting position & rotation of the object
            _posOffset = transform.position;
        }

        // Update is called once per frame
        void Update () {
            // Spin object around Y-Axis
            //transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

            // Float up/down with a Sin()
            _tempPos = _posOffset;
            _tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.position = _tempPos;
        }
    }
}
