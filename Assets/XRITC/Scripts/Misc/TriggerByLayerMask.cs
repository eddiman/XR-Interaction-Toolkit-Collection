using System;
using UnityEngine;
using UnityEngine.Events;

namespace XRITC.Scripts.Misc
{
    public class TriggerByLayerMask : MonoBehaviour
    {
        public LayerMask CollidingLayer;
        public UnityEvent onTriggerEnter;
        public UnityEvent onTriggerExit;
        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("trigger enter " + other.transform.name);
            if (!IsInLayerMask(other.gameObject, CollidingLayer)) return;

            Debug.Log("fade layer");
            onTriggerEnter.Invoke();
        }
        private void OnTriggerExit(Collider other)
        {
            if (!IsInLayerMask(other.gameObject, CollidingLayer)) return;
            onTriggerExit.Invoke();

        }
        public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
        {
            return ((layerMask.value & (1 << obj.layer)) > 0);
        }
    }
}
