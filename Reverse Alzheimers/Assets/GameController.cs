using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private float timeToRemember;

    [SerializeField] private GameObject player;

    [SerializeField] Transform spawnpoint;

    [SerializeField] private float timeToTeleport;

    [SerializeField] private float timeToTest;

    private bool testStarted;
    private bool testFinished;
    private bool timerFinished = false;
    public bool startRemember = false;
    

    public UnityEvent onTimerFinished;
    public UnityEvent onTestStart;
    public UnityEvent onTestFinshed;

    public List<GameObject> correctSelected;
    public List<GameObject> incorrectSelected;
    public List<GameObject> objectList;

    public InputField rememberInput;
    public InputField testInput;
    public InputField fileNameInput;
    public Toggle useAtomizer;
    //public InputField portInput;

    [SerializeField] CompileMemoryTestData compileMemoryTest;
    [SerializeField] Arduino_Setting_Polling_Read_Write arduinoSettings;

    [SerializeField] SerialController serialController;

    private void FixedUpdate()
    {

        if (!startRemember) return;
        if (!timerFinished)
        {
            if (timeToRemember <= 0)
            {
                onTimerFinished.Invoke();
                Invoke("MoveToTest", timeToTeleport);
                timerFinished = true;
            }
            else
            {
                timeToRemember -= Time.deltaTime;
            }
        }
        if (!testFinished)
        {
            if (testStarted)
            {
                if (timeToTest <= 0)
                {
                    onTestFinshed.Invoke();
                    testFinished = true;
                }
                else
                {
                    timeToTest -= Time.deltaTime;
                }
            }
        }
    }

    public void MoveToTest()
    {
        player.transform.position = spawnpoint.position;
        onTestStart.Invoke();
        testStarted = true;
        for (var i = 0; i < objectList.Count; i++)
        {
            objectList[i].GetComponent<Selectable>().TestStart();
        }
    }
    public void StartRemember()
    {

        //serialController.RePort();

        startRemember = true;
        timeToRemember = float.Parse(rememberInput.text, CultureInfo.InvariantCulture.NumberFormat);
        timeToTest = float.Parse(testInput.text, CultureInfo.InvariantCulture.NumberFormat);
        compileMemoryTest = GetComponent<CompileMemoryTestData>();
        compileMemoryTest.filename = fileNameInput.text;
        //serialController.portName = portInput.text;
       

       
        arduinoSettings.useAtomizer = this.useAtomizer.isOn;

        Debug.Log(useAtomizer.isOn);
        if (useAtomizer.isOn)
        {
            arduinoSettings.RunTest();
        }

        
        
    }

}
