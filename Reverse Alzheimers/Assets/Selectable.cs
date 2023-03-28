using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool correct;
    public bool selected = false;
    public float timeLookedAt;
    public Material originalMaterial;
    public float timeSelected;
    private float currentTime;
    bool logTime = false;
    bool onHover;
    public void SetBool(bool onhover)
    {
        onHover = onhover;
    }

    private void Update()
    {
        currentTime += Time.time;

        if (selected == true && logTime == false)
        {
            timeSelected = currentTime;
            logTime = true;
        }

        if (onHover)
        {
            timeLookedAt += Time.deltaTime;
        }


    }
   

}
