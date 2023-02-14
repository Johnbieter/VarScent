using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool correct;
    public bool selected = false;
    public float timeLookedAt;
    public Material originalMaterial;

    bool onHover;
    public void SetBool(bool onhover)
    {
        onHover = onhover;
    }

    private void Update()
    {
        if (onHover)
        {
            timeLookedAt += Time.deltaTime;
        }
    }
   

}
