using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class VRMovementController : MonoBehaviour
{
    private InputDevice targetDevice;
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDeviceCharacteristics inputDeviceCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, devices);

        if(devices.Count > 0)
        {
            targetDevice = devices[0];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
         if(primaryButtonValue)
        {
            Debug.Log("Backward");
            transform.Translate(Vector3.back * Time.deltaTime);
        }
        targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
        if (secondaryButtonValue)
        {
            Debug.Log("Forward");
            transform.Translate(Vector3.forward * Time.deltaTime);
        }

    }
}
