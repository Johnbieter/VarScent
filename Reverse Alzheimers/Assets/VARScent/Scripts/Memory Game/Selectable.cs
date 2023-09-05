using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------
 Script for selectable objects in the scene.
 --------------------------------------------------------*/

public class Selectable : MonoBehaviour
{
    public bool correct;
    public bool selected = false;
    public bool tutSelected = false;

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
        currentTime += Time.deltaTime; //Timer

        if (selected == true && logTime == false) //If the primary button was pressed on object
        {
            //Then mark time and prevent from marking again
            timeSelected = currentTime;
            logTime = true;
        }

        if (onHover)
        {
            timeLookedAt += Time.deltaTime; //Record how long they took when looking at object
        }


    }

    public void TestStart()
    {
        currentTime = 0;
    }
   

}
