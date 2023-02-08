/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class Arduino_Setting_Polling_Read_Write : MonoBehaviour
{
    public SerialController serialController;
    public enum Scents { Pine, Citrus, Peanut_Butter, Lavender };


    public Scents AtomizerOneContents;
    public Scents AtomizerTwoContents;
    public Scents AtomizerThreeContnets;
    public Scents AtomizerFourContents;

    public Scents[] atomizerContents = new Scents[4];

    //Timer
    public float scentTime = 60.0f;
    public float breakTime = 5.0f;

    public int currentScent = 1;
    private bool firstAtomizerOn = false;
    private bool secondAtomizerOn = false;
    private bool thirdAtomizerOn = false;
    private bool fourthAtomizerOn = false;

    private float scentTimer;
    private float breakTimer;
    private int[] atomizerOrder = new int[4]; //Array with the lenght of 4 because there are only 4 atomoizers
    private int currentTest = 0;                //Current test running, Starting at zero beacuase it is used to index the atomizerOrder array
    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
        scentTimer = scentTime;
        breakTimer = breakTime;

        atomizerContents[0] = AtomizerOneContents;
        atomizerContents[1] = AtomizerTwoContents;
        atomizerContents[2] = AtomizerThreeContnets;
        atomizerContents[3] = AtomizerFourContents;
    }

    // Executed each frame
    void Update()
    {
        if (currentTest < 4) // Becase there are only 4 scents and we dont need the timer to keep running afer that
        {
            RunTest();
            scentTimer -= Time.deltaTime;
            if (scentTimer < 0) breakTimer -= Time.deltaTime; //run break timer when scent timer is up
        }
        //---------------------------------------------------------------------
        // Receive data
        //---------------------------------------------------------------------

        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            Debug.Log("Message arrived: " + message);
    }

    public void RunTest()
    {
        if (currentScent == 1)
        {
            if (scentTimer > 0) ToggleFirstAtomizer(true);
            else
            {
                ToggleFirstAtomizer(false);
                if (breakTimer < 0) ResetTimes();  //Wait for break to be up before continuing onto next scent
            }
        }
        else if (currentScent == 2)
        {
            if (scentTimer > 0) ToggleSecondAtomizer(true);
            else
            {
                ToggleSecondAtomizer(false);
                if (breakTimer < 0) ResetTimes();   //Wait for break to be up before continuing onto next scent
            }
        }
        else if (currentScent == 3)
        {
            if (scentTimer > 0) ToggleThirdAtomizer(true);
            else
            {
                ToggleThirdAtomizer(false);
                if (breakTimer < 0) ResetTimes();   //Wait for break to be up before continuing onto next scent
            }
        }
        else if (currentScent == 4)
        {
            if (scentTimer > 0) ToggleFourthAtomizer(true);
            else
            {
                ToggleFourthAtomizer(false);
                if (breakTimer < 0) ResetTimes();   //Wait for break to be up before continuing onto next scent
            }
        }



    }

    private void ToggleFirstAtomizer(bool status)
    {
        if (status == firstAtomizerOn) return;
        firstAtomizerOn = status;

        if (status == true) serialController.SendSerialMessage("Q"); //Turn on the first atomizer
        if (status == false) serialController.SendSerialMessage("A"); //Turn off the first atomizer

        if (status == true) Debug.Log("First Atomizer On");
        else Debug.Log("First Atomizer off");
    }

    private void ToggleSecondAtomizer(bool status)
    {
        if (status == secondAtomizerOn) return;
        secondAtomizerOn = status;

        if (status == true) serialController.SendSerialMessage("W"); //Turn on the Second atomizer
        if (status == false) serialController.SendSerialMessage("S"); //Turn off the Second atomizer

        if (status == true) Debug.Log("Second Atomizer On");
        else Debug.Log("Second Atomizer off");
    }

    private void ToggleThirdAtomizer(bool status)
    {
        if (status == thirdAtomizerOn) return;
        thirdAtomizerOn = status;

        if (status == true) serialController.SendSerialMessage("E"); //Turn on the Third atomizer
        if (status == false) serialController.SendSerialMessage("D"); //Turn off the Third atomizer

        if (status == true) Debug.Log("Third Atomizer On");
        else Debug.Log("Third Atomizer off");
    }

    private void ToggleFourthAtomizer(bool status)
    {
        if (status == fourthAtomizerOn) return;
        fourthAtomizerOn = status;

        if (status == true) serialController.SendSerialMessage("R"); //Turn on the Fourth atomizer
        if (status == false) serialController.SendSerialMessage("F"); //Turn off the Fourth atomizer

        if (status == true) Debug.Log("Fourth Atomizer On");
        else Debug.Log("Fourth Atomizer off");
    }

    private void ResetTimes()
    {
        scentTimer = scentTime;
        breakTimer = breakTime;

    }
}
