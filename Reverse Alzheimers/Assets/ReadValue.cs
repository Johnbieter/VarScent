using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class ReadValue : MonoBehaviour
{
    public Transform objectA;

    // Drag & Drop the other object
    public Transform objectB;

    public float log;
    private int intLog;

    SerialPort port = new SerialPort("COM4", 9600);

    private void Start()
    {
        port.Open();
    }

    // Drag & Drop the gameobject, child of a Canvas holding a Text component

    // Use LateUpdate to compute the distance **after** the player / object has moved
    private void LateUpdate()
    {
        log = Vector3.Distance(objectA.position, objectB.position);

        //Debug.Log(log);

        if (log <= 5)
        {
            Debug.Log("Writing");
            port.Write("1");
            //log = 5000;
            
        }
        else
        {
            port.Write("0");
        }
    }
}