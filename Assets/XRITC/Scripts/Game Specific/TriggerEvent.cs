using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent  : MonoBehaviour
{

    [Serializable] public class TriggerColliderEvent : UnityEvent <Collider> {}

    public GameObject colliderRuleObject;
    public string colliderTagRuleString;
    public bool disableMeshRendererAtRuntime = true;

    public TriggerColliderEvent triggerEnter;
    public TriggerColliderEvent triggerStay;
    public TriggerColliderEvent triggerExit;

    private void Start()
    {
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = !disableMeshRendererAtRuntime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if ( colliderTagRuleString != "" && other.gameObject.CompareTag(colliderTagRuleString))
        {
            triggerEnter?.Invoke(other);

        }else
        if (colliderRuleObject == other.gameObject && colliderTagRuleString == "")
        {
            triggerEnter?.Invoke(other);
        } else
            //If there is no object set to collide with, then execute order
        if (colliderRuleObject == null && colliderTagRuleString == "")
        {
            triggerEnter?.Invoke(other);
        }
        //If there is an object set to collide with, then execute order when they collide

    }

    private void OnTriggerStay(Collider other)
    {
        if ( colliderTagRuleString != "" && other.gameObject.CompareTag(colliderTagRuleString))
        {
            triggerEnter?.Invoke(other);

        }else
        if (colliderRuleObject == other.gameObject && colliderTagRuleString == "")
        {
            triggerEnter?.Invoke(other);

        } else
            //If there is no object set to collide with, then execute order
        if (colliderRuleObject == null && colliderTagRuleString == "")
        {
            triggerEnter?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ( colliderTagRuleString != "" && other.gameObject.CompareTag(colliderTagRuleString))
        {
            triggerEnter?.Invoke(other);

        }else
        if (colliderRuleObject == other.gameObject && colliderTagRuleString == "")
        {
            triggerEnter?.Invoke(other);

        } else
            //If there is no object set to collide with, then execute order
        if (colliderRuleObject == null && colliderTagRuleString == "")
        {
            triggerEnter?.Invoke(other);
        }
    }
}
