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

    public float selectTimeOut = 0.5f;
    public bool canSelect = true;

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
        Debug.Log("Button is pressed!");
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
                        //Turn on selected material
                        hit.transform.gameObject.GetComponent<MeshRenderer>().material = onSelectMaterial;
                        hit.transform.gameObject.tag = "Unselectable";
                    }
                    else
                    {
                        //Turn off selected material
                        hit.transform.gameObject.GetComponent<MeshRenderer>().material = hitSelect.originalMaterial;
                        hit.transform.gameObject.tag = "Selectable";
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
