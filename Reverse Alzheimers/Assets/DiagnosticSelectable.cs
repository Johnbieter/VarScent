using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnosticSelectable : MonoBehaviour
{
    public GameObject hoverObject;

    public bool isSelected;

    private void Update()
    {
        if (isSelected)
            hoverObject.SetActive(true);
        else
            hoverObject.SetActive(false);
    }
}
