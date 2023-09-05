using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*------------------------------------------------------------------------------
Game manager scipt for the Memory Test Scene. This script will tell what script
to execute and where during runtime.
It'll take the UI input and compile the settings into the scene.
Once test starts, player will look around to try and remember scene. Once timer 
is up,player is transported to a different area and try and find all the objects 
that are different.
When memory timer is up, test coordinator compiles data to CSV file.
 -------------------------------------------------------------------------------*/

public class GameController : MonoBehaviour
{

    [SerializeField] private float timeToRemember;

    [SerializeField] private GameObject player;

    [SerializeField] Transform rememberSpawnpoint;
    [SerializeField] Transform testSpawnpoint;

    [SerializeField] private float timeToTeleport;

    [SerializeField] private float timeToTest;

    private bool testStarted;
    private bool testFinished;
    private bool timerFinished = false;
    public bool startRemember = false;
    private bool isTeleporting = false;

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

    private void Update()
    {

        if (!startRemember) return;
        if (!timerFinished)
        {
            //When Remember timer hits 0, run onTimerFinished events and move the player to the test area.
            if (timeToRemember <= 0)
            {
                onTimerFinished.Invoke();
                //Invoke("MoveToTest", timeToTeleport);
                if (!isTeleporting)
                {
                    StartCoroutine(MoveToTestCoroutine());
                    testStarted = true;
                    for (var i = 0; i < objectList.Count; i++)
                    {
                        objectList[i].GetComponent<Selectable>().TestStart();
                    }
                }
                timerFinished = true;
            }
            else
            {
                timeToRemember -= Time.deltaTime;
            }
        }

        //When Testing timer hits 0, invoke onTestFinished events
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

    private IEnumerator MoveToRememberCoroutine()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(timeToTeleport);
        onTestStart.Invoke();
        player.transform.position = rememberSpawnpoint.position;
        isTeleporting = false;
    }
    private IEnumerator MoveToTestCoroutine()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(timeToTeleport);
        player.transform.position = testSpawnpoint.position;
        onTestStart.Invoke();
        isTeleporting = false;
    }

/*    public void MoveToRemember()
    {
        if(!isTeleporting)
        {
            StartCoroutine(MoveToRememberCoroutine());
        }
    }
*/
    //Sends player to test area and run onTestStart events
    //Also puts all the selectable objects into the objectList
/*    public void MoveToTest()
    {
        if (!isTeleporting)
        {
            StartCoroutine(MoveToTestCoroutine());
            testStarted = true;
            for (var i = 0; i < objectList.Count; i++)
            {
                objectList[i].GetComponent<Selectable>().TestStart();
            }
        }
    }
*/
    //Takes UI input and assigns it to variables as well as populates the file name in the compileMemoryTest component.
    public void StartRemember()
    {
        //serialController.RePort();
        onTimerFinished.Invoke();
        //Invoke("MoveToRemember", timeToTeleport);
        if (!isTeleporting)
        {
            StartCoroutine(MoveToRememberCoroutine());
        }
        startRemember = true;
        timeToRemember = float.Parse(rememberInput.text, CultureInfo.InvariantCulture.NumberFormat);
        timeToTest = float.Parse(testInput.text, CultureInfo.InvariantCulture.NumberFormat);
        compileMemoryTest = GetComponent<CompileMemoryTestData>();
        compileMemoryTest.filename = fileNameInput.text;
        //serialController.portName = portInput.text;
       

       //Determinds if atomizer is in use for this test
        arduinoSettings.useAtomizer = this.useAtomizer.isOn;

        Debug.Log(useAtomizer.isOn);
        if (useAtomizer.isOn)
        {
            arduinoSettings.RunTest();
        }
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("TherapyTestScene");
    }
}
