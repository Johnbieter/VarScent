using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DiagnosticTestController : MonoBehaviour
{
    public RaycastingLogInfo logInfo;
    public CSVWriter writeInfo;
    public Arduino_Setting_Polling_Read_Write atomizerControl;

    //These are where the objects will go.
    public Transform[] positions;

    //Has the position been taken?
    public List<Transform> positionsTaken;

    public GameObject[] potentialCorrect;

    //This is the correct object for each test.
    public GameObject correctObject;
    //This is just a list of random incorrect objects.
    public GameObject[] incorrectObjects;

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
    [SerializeField] TMP_InputField setComPort;
    [SerializeField] TMP_Dropdown[] atomizerContents;


    [SerializeField] SerialController serial;
    private void Start()
    {
        index = 0;
        MainCamera = Camera.main;
        logCompiler = MainCamera.GetComponent<RaycastingLogInfo>();
    }

    public void ConfigureSettings()
    {
        logCompiler.myWriter.filename = documentName.text;
        atomizerControl.scentTime = int.Parse(setScentTimer.text);
        atomizerControl.breakTime = int.Parse(setBreakTimer.text);

        atomizerControl.breakTimer = atomizerControl.breakTime;

        serial.portName = setComPort.text;

        for (var i = 0; i < 4; i++)
        {
            var value = atomizerContents[i].value;

            if (value == 0)
            {
                if (i == 0)
                    atomizerControl.AtomizerOneContents = Arduino_Setting_Polling_Read_Write.Scents.Pine;
                if(i == 1)
                    atomizerControl.AtomizerTwoContents = Arduino_Setting_Polling_Read_Write.Scents.Pine;
                if (i == 2)
                    atomizerControl.AtomizerThreeContnets = Arduino_Setting_Polling_Read_Write.Scents.Pine;
                if(i == 3)
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


    public void StartPrep()
    {
        testPrep = true;
        for (var i = 0; i < instantiatedObjects.Count; i++)
        {
            Destroy(instantiatedObjects[i]);
        }
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
            Debug.Log("Peanut butter is selected");
            // correctObject = peanut_butter;

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
            Debug.Log("Lavender  is selected");
            // correctObject = Lavendar;
            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Rose")
                {
                    correctObject = potentialCorrect[i];
                }
            }
        }

        //Place Objects


        //Place correct object
        int randPos = Random.Range(0, 4);

        GameObject obj = Instantiate(correctObject, positionsTaken[randPos].position, positionsTaken[randPos].rotation);
        instantiatedObjects.Add(obj);
        positionsTaken.Remove(positionsTaken[randPos]);
        SendDataToCompiler(obj, index - 1);

        //Place random incorrect objects
        for (var i = 0; i < positionsTaken.Count; i++)
        {
            int randObject = Random.Range(0, incorrectObjects.Length);
            GameObject objIncorrect = Instantiate(incorrectObjects[randObject], positionsTaken[i].position, positionsTaken[i].rotation);
            instantiatedObjects.Add(objIncorrect);
        }

        //Begin Test

        testComplete = false;
    }

    public void Update()
    {
        if (testPrep)
        {
            atomizerControl.breakTimer -= Time.deltaTime;
            breakTimerDisplay.text = "Break: " + atomizerControl.breakTimer.ToString();
            scentTimerDisplay.enabled = false;
            breakTimerDisplay.enabled = true;

            if (atomizerControl.breakTimer <= 0)
            {
                ConfigureTest();
                atomizerControl.RunTest();
                testPrep = false;
            }
        }

        if (atomizerControl.scentTimer > 0)
        {
            
            logCompiler.RecordTestInfo(instantiatedObjects[0], instantiatedObjects);
        }

        if (testComplete == false)
        {
            atomizerControl.RunTimer();

            scentTimerDisplay.text = "Scent Time: " + atomizerControl.scentTimer.ToString();
            breakTimerDisplay.enabled = false;
            scentTimerDisplay.enabled = true;
            if (atomizerControl.scentTimer <= 0)
            {
                
                    CompileData();
                    testComplete = true;
                
            }

        }
        
        
    }


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
