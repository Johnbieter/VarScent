using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RaycastController : MonoBehaviour
{

    private InputDevice targetDevice;

    public Transform leftHandControllerPos;

    [SerializeField] Material onSelectMaterial;
    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log(devices[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        
        if (primaryButtonValue == true)
        {
            Debug.Log("Trigger pressed");
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(leftHandControllerPos.position, leftHandControllerPos.transform.forward, out hit, Mathf.Infinity))
            {
                // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

                if (hit.transform.tag == "Selectable")
                {
                    //Debug.Log("Select material");
                    //hit.transform.gameObject.GetComponent<MeshRenderer>().material = onSelectMaterial;
                }
                else
                {
                    Debug.Log("Object wasn't changed");
                }
            }
        }
    }
}
