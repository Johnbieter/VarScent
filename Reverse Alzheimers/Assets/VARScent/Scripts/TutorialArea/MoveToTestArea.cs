using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTestArea : MonoBehaviour
{
    public bool playerInsideTutorial = true;
    public Transform TestPos;

    private void Update()
    {
        if (playerInsideTutorial)
        {

        }  
    }

    public void MoveToTest()
    {
        gameObject.transform.position = TestPos.position;
    }
}
