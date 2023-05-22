using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------------------------------------------------------------------------------
 Enables and disables the hoverObject game object on attached object prefab. This
is feedback to show that the player is looking at current object.
 ---------------------------------------------------------------------------------*/

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
