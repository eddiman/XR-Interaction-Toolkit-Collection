using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletteColor : MonoBehaviour
{
    public Color paletteColor;
    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.gameObject;
        //Making it easy, could've been better tho

        //Checks if the colliding object has either of two components in them
        if (gameObject.GetComponent<XRGrab3DMarker>())
        {
            gameObject.GetComponent<XRGrab3DMarker>().ChangeColorDrawColorRPC(paletteColor);
        } else if (gameObject.GetComponent<XRGrabMarker>())
        {
            gameObject.GetComponent<XRGrabMarker>().ChangeColorDrawColorRPC(paletteColor);
        }
    }

}
