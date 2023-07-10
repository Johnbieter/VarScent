using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
//using System;

/*---------------------------------------------------------------------------------
The game manager for the Diagnostic Test Scene. This script will tell what script
to execute and where during runtime. 
It'll first configure the settings set in the UI start screen,
then set up the scene. 
It'll spawn objects in the taken positions -- three incorrect
and one correct correlating to the activated smell -- then start the break time.
Once break timer is up, then start the scent timer where the player finds the object
relating to the scent. Data is also being recorded by this point. 
Once timer is up,and test coordinator presses the compile data button, it'll run
the CompileData function in RaycastingLogInfo script.
 ----------------------------------------------------------------------------------*/

public class DiagnosticTestController : MonoBehaviour
{
    //References
    public RaycastingLogInfo logInfo;
    public CSVWriter writeInfo;
    public Arduino_Setting_Polling_Read_Write atomizerControl;

    //These are where the objects will go in the scene.
    public Transform[] positions;

    //Has the position been taken?
    public List<Transform> positionsTaken;

    public GameObject[] potentialCorrect;

    //This is the correct object for each test.
    public GameObject correctObject;
    //This is just a list of random incorrect objects.
    public GameObject[] incorrectObjects;
    private GameObject[] incorrectObjectsToSpawn;

    public List<GameObject> instantiatedObjects = new List<GameObject>(0);

    Camera MainCamera;
    RaycastingLogInfo logCompiler;

    private int index;

    bool testPrep = false;
    bool testComplete = true;

    [Header("Timer Display")]

    [SerializeField] TMP_Text scentTimerDisplay, breakTimerDisplay;
    [Header("Setup Screen")]
    [SerializeField] TMP_InputField documentName;
    [SerializeField] TMP_InputField setScentTimer;
    [SerializeField] TMP_InputField setBreakTimer;
    //[SerializeField] TMP_InputField setComPort;
    [SerializeField] TMP_Dropdown setComPort;
    [SerializeField] TMP_Dropdown[] atomizerContents;
    public GameObject startTestBtn;
    public GameObject compileDataBtn;


    [SerializeField] SerialController serial;
    private void Start()
    {
        index = 0;
        MainCamera = Camera.main;
        logCompiler = MainCamera.GetComponent<RaycastingLogInfo>();
    }

    public void ConfigureSettings()
    {
        logCompiler.myWriter.filename = documentName.text; //Sets file name to the UI text element
        //Assigns timers to the UI text elements
        atomizerControl.scentTime = int.Parse(setScentTimer.text);
        atomizerControl.breakTime = int.Parse(setBreakTimer.text);

        atomizerControl.breakTimer = atomizerControl.breakTime;

        //serial.portName = setComPort.; //Assigns Arduino port to the UI element
        switch (setComPort.value)
        {
            case 0:
                serial.enabled = false;
                serial.portName = "COM3";
                serial.enabled = true;
                //serial.RePort(); //<----------IF ERROR THIS COULD BE BREAKING THE SCRIPT------------
                break;
            case 1:
                serial.enabled = false;
                serial.portName = "COM4";
                serial.enabled = true;
                //serial.RePort(); //<----------IF ERROR THIS COULD BE BREAKING THE SCRIPT------------
                break;
            case 2:
                serial.enabled = false;
                serial.portName = "COM5";
                serial.enabled = true;
                //serial.RePort(); //<----------IF ERROR THIS COULD BE BREAKING THE SCRIPT------------
                break;
        }

        Debug.Log(serial.portName);

        //Sets the enums in the atomizerControl to the selected UI elements
        for (var i = 0; i < 4; i++)
        {
            var value = atomizerContents[i].value;

            if (value == 0)
            {
                if (i == 0)
                    atomizerControl.AtomizerOneContents = Arduino_Setting_Polling_Read_Write.Scents.Pine;
                if (i == 1)
                    atomizerControl.AtomizerTwoContents = Arduino_Setting_Polling_Read_Write.Scents.Pine;
                if (i == 2)
                    atomizerControl.AtomizerThreeContnets = Arduino_Setting_Polling_Read_Write.Scents.Pine;
                if (i == 3)
                    atomizerControl.AtomizerFourContents = Arduino_Setting_Polling_Read_Write.Scents.Pine;
            }
            if (value == 1)
            {
                if (i == 0)
                    atomizerControl.AtomizerOneContents = Arduino_Setting_Polling_Read_Write.Scents.Citrus;
                if (i == 1)
                    atomizerControl.AtomizerTwoContents = Arduino_Setting_Polling_Read_Write.Scents.Citrus;
                if (i == 2)
                    atomizerControl.AtomizerThreeContnets = Arduino_Setting_Polling_Read_Write.Scents.Citrus;
                if (i == 3)
                    atomizerControl.AtomizerFourContents = Arduino_Setting_Polling_Read_Write.Scents.Citrus;
            }
            if (value == 2)
            {
                if (i == 0)
                    atomizerControl.AtomizerOneContents = Arduino_Setting_Polling_Read_Write.Scents.Rose;
                if (i == 1)
                    atomizerControl.AtomizerTwoContents = Arduino_Setting_Polling_Read_Write.Scents.Rose;
                if (i == 2)
                    atomizerControl.AtomizerThreeContnets = Arduino_Setting_Polling_Read_Write.Scents.Rose;
                if (i == 3)
                    atomizerControl.AtomizerFourContents = Arduino_Setting_Polling_Read_Write.Scents.Rose;
            }
            if (value == 3)
            {
                if (i == 0)
                    atomizerControl.AtomizerOneContents = Arduino_Setting_Polling_Read_Write.Scents.Cinnamon;
                if (i == 1)
                    atomizerControl.AtomizerTwoContents = Arduino_Setting_Polling_Read_Write.Scents.Cinnamon;
                if (i == 2)
                    atomizerControl.AtomizerThreeContnets = Arduino_Setting_Polling_Read_Write.Scents.Cinnamon;
                if (i == 3)
                    atomizerControl.AtomizerFourContents = Arduino_Setting_Polling_Read_Write.Scents.Cinnamon;
            }

        }
        atomizerControl.SetAtomizerContents();

    }


    //Removes all spawned objects in the scene
    public void DestoryObjects()
    {
        if (instantiatedObjects == null) return;
        for (var i = 0; i < instantiatedObjects.Count; i++)
        {
            Destroy(instantiatedObjects[i]);
        }
    }


    public void StartPrep()
    {
        testPrep = true;
        instantiatedObjects = new List<GameObject>(0);
    }


    public void ConfigureTest()
    {

        index += 1;


        if (index >= atomizerControl.atomizerContents.Length)
        {
            //Dont cause null error. Return on finish.
            return;

        }

        //Delete objects from before



        //Reset Incorrect Objects to Spawn
        incorrectObjectsToSpawn = incorrectObjects;

        //Resetup positionsTaken;
        positionsTaken = new List<Transform>(0);
        for (var y = 0; y < positions.Length; y++)
        {
            positionsTaken.Add(positions[y]);
        }


        //Set correct object
        atomizerControl.currentScent = index;




        if (atomizerControl.atomizerContents[index] == Arduino_Setting_Polling_Read_Write.Scents.Pine)
        {
            Debug.Log("Pine is selected");
            //Correct object = Pine
            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Pine")
                {
                    correctObject = potentialCorrect[i];
                }
            }
        }

        if (atomizerControl.atomizerContents[index] == Arduino_Setting_Polling_Read_Write.Scents.Citrus)
        {
            Debug.Log("Citrus is selected");
            // correctObject = citrus;

            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Citrus")
                {
                    correctObject = potentialCorrect[i];
                }
            }
        }
        if (atomizerControl.atomizerContents[index] == Arduino_Setting_Polling_Read_Write.Scents.Cinnamon)
        {
            Debug.Log("Cinnamon is selected");
            // correctObject = Cinnamon;

            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Cinnamon")
                {
                    correctObject = potentialCorrect[i];
                }
            }

        }
        if (atomizerControl.atomizerContents[index] == Arduino_Setting_Polling_Read_Write.Scents.Rose)
        {
            Debug.Log("Rose is selected");
            // correctObject = Lavendar;
            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Rose")
                {
                    correctObject = potentialCorrect[i];
                }
            }
        }

        //---- Placing Objects ----


        //Place correct object
        int randPos = Random.Range(0, 4);

        GameObject obj = Instantiate(correctObject, positionsTaken[randPos].position, positionsTaken[randPos].rotation);

        instantiatedObjects.Add(obj);
        positionsTaken.Remove(positionsTaken[randPos]);
        SendDataToCompiler(obj, index - 1);

        //Place random incorrect objects
        for (var i = 0; i < positionsTaken.Count; i++)
        {
            int randObject = Random.Range(0, incorrectObjectsToSpawn.Length);
            GameObject objIncorrect = Instantiate(incorrectObjectsToSpawn[randObject], positionsTaken[i].position, positionsTaken[i].rotation);
            instantiatedObjects.Add(objIncorrect);

            var incorrectObjectsToSpawnList = incorrectObjectsToSpawn.ToList();
            incorrectObjectsToSpawnList.RemoveAt(randObject);
            incorrectObjectsToSpawn = incorrectObjectsToSpawnList.ToArray();
        }

        //---- Begin Test ----

        testComplete = false;
    }

    public void Update()
    {
        if (testPrep)
        {
            //Break timer in the scene
            startTestBtn.SetActive(false);
            compileDataBtn.SetActive(false);
            atomizerControl.breakTimer -= Time.deltaTime;
            breakTimerDisplay.text = "Break: " + Mathf.Round(atomizerControl.breakTimer).ToString();
            scentTimerDisplay.enabled = false;
            breakTimerDisplay.enabled = true;

            //When break time is up, set up the scene and run the test
            if (atomizerControl.breakTimer <= 0)
            {
                ConfigureTest();
                logCompiler.hasPlayed = false; //resets the UI sound
                atomizerControl.scentTimer = 1;
                atomizerControl.RunTest();
                testPrep = false;
            }
        }

        //While test is going record the info in the logCompiler component
        if (atomizerControl.scentTimer > 0)
        {
            logCompiler.RecordTestInfo(instantiatedObjects[0], instantiatedObjects);
        }

        if (testComplete == false)
        {
            //Scent Timer in the scene
            atomizerControl.RunTimer();

            scentTimerDisplay.text = "Scent Time: " + Mathf.Round(atomizerControl.scentTimer).ToString();

            breakTimerDisplay.enabled = false;
            scentTimerDisplay.enabled = true;

            //While scent timer is done, turn off atomizers, compile data, and reset scene
            if (atomizerControl.scentTimer <= 0)
            {
                atomizerControl.scentTimer = 0;
                scentTimerDisplay.text = "Scent Time: 0";
                atomizerControl.ToggleAllOff();
                CompileData();
                testComplete = true;
                DestoryObjects();
                startTestBtn.SetActive(true);
                compileDataBtn.SetActive(true);

            }

        }
    }

    // ---- Compile all the data gathered ----

    public void CompileData()
    {
        logCompiler.myWriter.CompileData();
    }

    void SendDataToCompiler(GameObject obj, int index)
    {
        logCompiler.ResetForNextTest();
        logCompiler.objectsToLog.Add(obj);
        logCompiler.correctObject = index;
        logCompiler.objectSpotted.Add(false);
    }
}
