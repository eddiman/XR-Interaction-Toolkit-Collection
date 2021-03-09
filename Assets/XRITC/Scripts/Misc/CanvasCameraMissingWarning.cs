using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Sends out warning in console if Camera of World Space is not set
 * To get this working again, set the "VR Camera" in the VR Rig to the "Event Camera", when Render mode is "World Space"
 */
public class CanvasCameraMissingWarning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!GetComponent<Canvas>().worldCamera)
        {
            Debug.LogWarning("'Event Camera in' " + transform.name + " is not set. UI interactions will not work.");
        }
    }
}
