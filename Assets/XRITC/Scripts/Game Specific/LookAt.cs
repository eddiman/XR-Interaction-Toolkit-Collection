using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    public Transform LookAtObject;

    public bool doLookAt = true;

    // Update is called once per frame
    void Update()
    {
        if(!LookAtObject) return;
            transform.LookAt(LookAtObject);
    }
}
