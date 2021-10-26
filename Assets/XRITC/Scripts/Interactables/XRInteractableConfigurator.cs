using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * A class for configuring XRGrabNoPlayerCollideInteractable
 */
public class XRInteractableConfigurator : MonoBehaviour
{
  public bool allowOffsetGrabRotation;
  public bool allowOffsetGrabPosition;
  public string noColliderLayerName = "NoPlayerCollide";

      public XRInteractionManager manager;
    public XRBaseInteractor controller;
}
