using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    // Start is called before the first frame update
    public bool showController = false;
    public InputDeviceCharacteristics DeviceCharacteristics;
    public List<GameObject> ControllerPrefabs;
    public GameObject HandModelPrefab;

    private GameObject _spawnedController;
    private GameObject _spawnedHandModel;
    private List<InputDevice> _devices;
    private InputDevice _targetDevice;
    private Animator _handAnimator;

    /*Is run only in Unity Editor. Responds only to changes in public values in the editor.
     So it allows you to toggle on and off the hands.*/
    void OnValidate()
    {
        try
        {
            //ShowControllerActivated(showController);
        }
        catch (Exception e)
        {
            Console.WriteLine(e + " Hand Model prefab is not set");
            throw;
        }

    }
    void Start()
    {
        _devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(DeviceCharacteristics, _devices);
        TryInitializeControllers();

    }

    void Update()
    {
        if (!showController && _devices.Count > 0)
        {
            UpdateHandAnimation();
        }
    }

    /*
     * If there exists any XR device, add the first device in list to set as controller,
     * if no device in list, listen to when a device is connected and add the newly connected device
     */
    private void TryInitializeControllers()
    {

        if (_devices.Count > 0)
        {
            Debug.Log(_devices.Count + " controllers has been seen setting devices...");
            AddDevice(_devices[0]);
        }
        else
        {
            Debug.Log("No devices has been seen. Adding listener");
            InputDevices.deviceConnected += AddDevice;
        }


    }
    /*Method for showing and hiding controlers and hands. */
    public void ShowControllerActivated(bool show)
    {
        try
        {
            showController = show;
            if (showController)
            {
                _spawnedController.SetActive(true);
                _spawnedHandModel.SetActive(false);
            }
            else
            {
                _spawnedController.SetActive(false);
                _spawnedHandModel.SetActive(true);
            }
        }
        catch (Exception e)
        {
            Debug.Log( e + " - Hand model prefab is not set");
        }

    }
/* Method for setting animation based on input values from trigger and grip.
 The hand animator is cached in Start(), and animates based on a 0 to 1 float value from the trigger/grip*/
    private void UpdateHandAnimation()
    {
        if (_targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {

            _handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            _handAnimator.SetFloat("Trigger", 0);
        }

        if (_targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            _handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            _handAnimator.SetFloat("Grip", 0);
        }
    }
/* Receives an InputDevice, searches for the exact name of Input Device with the ControllerPrefabs List.
 InputDevice obj is the connected device, but is not used in this context, is required by delegate signature
 */
    private void AddDevice(InputDevice obj)
    {
        Debug.Log(_devices.Count);
        try
        {
            //Refresh the device list once more due, for when the delegate InputDevice.deviceConnected fires
            InputDevices.GetDevicesWithCharacteristics(DeviceCharacteristics, _devices);
            if (_devices.Count <= 0) return;
            _targetDevice = _devices[0];
            GameObject prefab = ControllerPrefabs.Find(controller => controller.name == _targetDevice.name);

            if (prefab)
            {
                _spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.Log("Did not find controller");
                _spawnedController = Instantiate(ControllerPrefabs[0], transform);
            }

            _spawnedHandModel = Instantiate(HandModelPrefab, transform);

            _handAnimator = _spawnedHandModel.GetComponent<Animator>();
            ShowControllerActivated(showController);
            InputDevices.deviceConnected -= AddDevice;
        }
        catch (Exception e)
        {
            //TODO: This under
            Debug.LogWarning("TODO: Find out this Index Out of Bounds is, doesnt affect any fuinctionalty tho");
            throw;
        }
    }

    private void storeLastPosition()
    {
        var newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);


    }


}
