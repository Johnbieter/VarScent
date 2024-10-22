using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HighlightObject : MonoBehaviour
{
    private InputDevice targetDevice;

    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] Material onSelectMaterial;
    GameController gC;

    float count = 0;
    public float selectTimeOut = 0.5f;
    public bool canSelect = true;

    public bool rightHand;
    // Start is called before the first frame update
    void Start()
    {

        gC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rayInteractor = GetComponent<XRRayInteractor>();
        if (!rightHand)
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
        else
        {
            List<InputDevice> devices = new List<InputDevice>();
            var desiredCharacteristics = InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
            InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, devices);
            if (devices.Count > 0)
            {
                targetDevice = devices[0];
                Debug.Log(devices[0]);
            }
        }

            
        
        
    }

    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);

        
        if (primaryButtonValue == true)
        {
           if (canSelect == true)
           {
                canSelect = false;
                Invoke("CanSelectAgain", selectTimeOut);

                if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
                {

                    if (hit.transform.gameObject.GetComponent<Selectable>())
                    {
                        Selectable hitSelect = hit.transform.gameObject.GetComponent<Selectable>();
                        hitSelect.selected = !hitSelect.selected;
                        if (hitSelect.selected)
                        {
                            hit.transform.gameObject.GetComponent<MeshRenderer>().material = onSelectMaterial;
                            hit.transform.gameObject.tag = "Unselectable";
                        }
                        else
                        {
                            hit.transform.gameObject.GetComponent<MeshRenderer>().material = hitSelect.originalMaterial;
                            hit.transform.gameObject.tag = "Selectable";
                        }
                      
                    }

                    /*
                        if (hit.transform.gameObject.tag == "Selectable")
                        {
                            Debug.Log("Object was a selectable");
                            if (hit.transform.gameObject.GetComponent<Selectable>().correct)
                            {
                                Debug.Log("add to mf list");
                                hit.transform.gameObject.GetComponent<Selectable>().selected = true;
                                //gC.correctSelected.Add(hit.transform.gameObject);
                                hit.transform.gameObject.GetComponent<MeshRenderer>().material = onSelectMaterial;
                                hit.transform.gameObject.tag = "Unselectable";
                            }
                            else
                            {
                                //gC.incorrectSelected.Add(hit.transform.gameObject);
                                hit.transform.gameObject.GetComponent<MeshRenderer>().material = onSelectMaterial;
                                hit.transform.gameObject.tag = "Unselectable";
                            }


                        }
                        
                        else if (hit.transform.gameObject.tag == "Unselectable")
                        {
                            if (hit.transform.gameObject.GetComponent<Selectable>().correct)
                            {
                                gC.correctSelected.Remove(hit.transform.gameObject);
                                hit.transform.gameObject.GetComponent<MeshRenderer>().material = hit.transform.gameObject.GetComponent<Selectable>().originalMaterial;
                                hit.transform.tag = "Selectable";
                            }
                            else
                            {
                                gC.incorrectSelected.Remove(hit.transform.gameObject);
                                hit.transform.gameObject.GetComponent<MeshRenderer>().material = hit.transform.gameObject.GetComponent<Selectable>().originalMaterial;
                                hit.transform.tag = "Selectable";
                            }
                        }
                    */

                }
           }
        }
    }

    public void CanSelectAgain()
    {
        canSelect = true;
    }
}
