using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraFading;
public class CameraFadeManager : MonoBehaviour
{
    public void FadeOut()
    {
        Debug.Log("Fade out");
        CameraFade.Out();
    }

    public void FadeIn()
    {
        Debug.Log("Fade in");
        CameraFade.In();
    }
}
