using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class ReadValue : MonoBehaviour
{
    public bool redLedIsOn = false;
    public bool yellowLedIsOn = false;
    public bool blueLedIsOn = false;
    public bool greenLedIsOn = false;

    //public string portName = "COM5";
    //public int baudRate = 9600;
    SerialPort port = new SerialPort("COM5", 9600);


    // Start is called before the first frame update
    void Start()
    {
        port.Open();
    }

    private void Update()
    {
        if (redLedIsOn)
        {
            port.Write("R");
            //port.Write("5");
        }
        else
        {
            port.Write("0");
        }

        if (yellowLedIsOn == true)
        {
            port.Write("2");
        }
        else
        {
            port.Write("0");
        }

        if (blueLedIsOn == true)
        {
            port.Write("5");
        }
        else
        {
            port.Write("0");
        }

        if (greenLedIsOn == true)
        {
            port.Write("15");
        }
        else
        {
            port.Write("0");
        }
    }
}