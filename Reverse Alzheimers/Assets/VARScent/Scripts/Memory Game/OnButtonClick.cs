using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Checks for button input on an input action
/// </summary>
public class OnButtonClick : MonoBehaviour
{
    [Tooltip("Actions to check")] //Path to which button to look for when pressed (Done in the Unity Editor)
    public InputAction action = null;

    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] Material onSelectMaterial;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    public float selectTimeOut = 0.5f;
    public bool canSelect = true;

    private Material objectsMaterial;

    // When the button is pressed
    public UnityEvent OnPress = new UnityEvent();

    // When the button is released
    public UnityEvent OnRelease = new UnityEvent();

    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    private void Awake()
    {
        action.started += Pressed;
        action.canceled += Released;
    }

    private void OnDestroy()
    {
        action.started -= Pressed;
        action.canceled -= Released;
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Pressed(InputAction.CallbackContext context)
    {
        OnPress.Invoke();
    }

    private void Released(InputAction.CallbackContext context)
    {
        OnRelease.Invoke();
    }

    //Function to call when the desired button is pressed. This will enable and disable the selection of objects and change thier material.
    public void HighlightObject()
    {
        //Debug.Log("Button is pressed!");
        if (canSelect == true) //If the selection cooldown is done
        {
            canSelect = false;
            Invoke("CanSelectAgain", selectTimeOut); //Reselect cool down

            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)) //if raycast is hitting something
            {

                if (hit.transform.gameObject.GetComponent<Selectable>()) //And if the something has a Selectable component
                {
                    Selectable hitSelect = hit.transform.gameObject.GetComponent<Selectable>();
                    hitSelect.selected = !hitSelect.selected;
                    if (hitSelect.selected)
                    {
                        if (hitSelect.originalMaterial != null)
                        {
                            objectsMaterial = hit.transform.gameObject.GetComponent<MeshRenderer>().material; //TEST

                            //Turn on selected material
                            hit.transform.gameObject.GetComponent<MeshRenderer>().material = onSelectMaterial;
                            hit.transform.gameObject.tag = "Unselectable";

                            source.pitch = 1.3f;
                            source.PlayOneShot(clip, 1f);
                        }
                        else //TEST--
                        {
                            objectsMaterial = hit.transform.gameObject.GetComponentInChildren<MeshRenderer>().material;

                            hit.transform.gameObject.GetComponentInChildren<MeshRenderer>().material = onSelectMaterial;
                            hit.transform.gameObject.tag = "Unselectable";

                            source.pitch = 1.3f;
                            source.PlayOneShot(clip, 1f);
                        }//--TEST
                    }
                    else
                    {
                        if (hitSelect.originalMaterial != null)
                        {
                            //Turn off selected material
                            hit.transform.gameObject.GetComponent<MeshRenderer>().material = hitSelect.originalMaterial;
                            hit.transform.gameObject.tag = "Selectable";

                            source.pitch = 0.7f;
                            source.PlayOneShot(clip, 1f);
                        }
                        else //TEST--
                        {
                            hit.transform.gameObject.GetComponentInChildren<MeshRenderer>().material = hitSelect.originalMaterial;
                            hit.transform.gameObject.tag = "Selectable";

                            source.pitch = 0.7f;
                            source.PlayOneShot(clip, 1f);
                        }//--TEST
                    }

                }
            }
        }
    }
    public void CanSelectAgain()
    {
        canSelect = true;
    }
}
