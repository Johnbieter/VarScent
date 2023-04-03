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
    public enum Scents { Pine, Citrus, Cinnamon, Rose, Zero };


    public Scents AtomizerOneContents;
    public Scents AtomizerTwoContents;
    public Scents AtomizerThreeContnets;
    public Scents AtomizerFourContents;

    public Scents[] atomizerContents = new Scents[4];

    //Timer
    public float scentTime = 60.0f;
    public float breakTime = 10f;


    public int currentScent = 1;
    private bool firstAtomizerOn = false;
    private bool secondAtomizerOn = false;
    private bool thirdAtomizerOn = false;
    private bool fourthAtomizerOn = false;

    public float scentTimer;
    public float breakTimer;
    private int[] atomizerOrder = new int[4]; //Array with the lenght of 4 because there are only 4 atomoizers
    private int currentTest = 0;                //Current test running, Starting at zero beacuase it is used to index the atomizerOrder array


    public bool isMemoryTest;
    public bool useAtomizer;
    // Initialization
    void Start()
    {

        
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        
        //scentTimer = scentTime;
        breakTimer = breakTime;

        if (useAtomizer)
        {
            currentScent = 1;
            RunTest();
        }

        if (isMemoryTest) return;


        
    }

    public void SetAtomizerContents()
    {
        atomizerContents[1] = AtomizerOneContents;
        atomizerContents[2] = AtomizerTwoContents;
        atomizerContents[3] = AtomizerThreeContnets;
        atomizerContents[4] = AtomizerFourContents;
    }
    // Executed each frame
    void Update()
    {


        
            //RunTest();
       
        if (scentTimer < 0)
        {
            ToggleFirstAtomizer(false);
            ToggleSecondAtomizer(false);
            ToggleThirdAtomizer(false);
            ToggleFourthAtomizer(false);
            //RunTest();
            //run break timer when scent timer is up
            //breakTimer -= Time.deltaTime;
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
    public void RunTimer()
    {
        scentTimer -= Time.deltaTime;
    }

    public void StopTest()
    {
        ToggleFirstAtomizer(false);
    }
    public void RunTest()
    {
        scentTimer = scentTime;
        breakTimer = breakTime;

        if (isMemoryTest){ currentScent = 1;} 

        if (currentScent == 1)
        {
            if (scentTimer > 0) { ToggleFirstAtomizer(true); Debug.Log("Atomizer on!"); }
            else
            {
                Debug.Log("Atomizer off!");
                ToggleFirstAtomizer(false);
                if (breakTimer < 0) ResetTimes();  //Wait for break to be up before continuing onto next scent
            }
        }

        if (isMemoryTest) return;

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

        if (status == true) serialController.SendSerialMessage("1"); //Turn on the first atomizer
        if (status == false) serialController.SendSerialMessage("0"); //Turn off the first atomizer

        if (status == true) Debug.Log("First Atomizer On");
        else Debug.Log("First Atomizer off");
    }

    private void ToggleSecondAtomizer(bool status)
    {
        if (status == secondAtomizerOn) return;
        secondAtomizerOn = status;

        if (status == true) serialController.SendSerialMessage("2"); //Turn on the Second atomizer
        if (status == false) serialController.SendSerialMessage("0"); //Turn off the Second atomizer

        if (status == true) Debug.Log("Second Atomizer On");
        else Debug.Log("Second Atomizer off");
    }

    private void ToggleThirdAtomizer(bool status)
    {
        if (status == thirdAtomizerOn) return;
        thirdAtomizerOn = status;

        if (status == true) serialController.SendSerialMessage("3"); //Turn on the Third atomizer
        if (status == false) serialController.SendSerialMessage("0"); //Turn off the Third atomizer

        if (status == true) Debug.Log("Third Atomizer On");
        else Debug.Log("Third Atomizer off");
    }

    private void ToggleFourthAtomizer(bool status)
    {
        if (status == fourthAtomizerOn) return;
        fourthAtomizerOn = status;

        if (status == true) serialController.SendSerialMessage("4"); //Turn on the Fourth atomizer
        if (status == false) serialController.SendSerialMessage("0"); //Turn off the Fourth atomizer

        if (status == true) Debug.Log("Fourth Atomizer On");
        else Debug.Log("Fourth Atomizer off");
    }

    public void ResetTimes()
    {
        scentTimer = scentTime;
        breakTimer = breakTime;

    }
}
