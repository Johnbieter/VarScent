using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingLogInfo : MonoBehaviour
{

    public List<GameObject> objectsToLog = new List<GameObject>();
    public List<bool> objectSpotted;


    [Header("Set Correct Object")]
    public int correctObject;

    [Header("Interim Data")]
    public float timeToCorrectObject;
    public float timeOnCorrectObject;
    public float timeOnWrong;
    public bool correctObjectFound = false;
    public float time;


    public CSVWriter myWriter;

    void Start()
    {
        myWriter = GetComponent<CSVWriter>();
    }



    public void RecordTestInfo(GameObject correctObj, List<GameObject> incorrectObj)
    {
        time += Time.deltaTime;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            GameObject lookObject = hit.transform.gameObject;

            if (lookObject.name == correctObj.name)
            {
                Debug.Log("Object found");

                if (!correctObjectFound)
                {
                    timeToCorrectObject = time;
                    correctObjectFound = true;
                }
                if (correctObjectFound)
                {
                    timeOnCorrectObject += Time.deltaTime;
                }


            }
            else
            {
                for (var i = 1; i < incorrectObj.Count; i++)
                {
                    if (lookObject.name == incorrectObj[i].name)
                    {
                        timeOnWrong += Time.deltaTime;
                    }
                }
            }
        }
    }
    public void ResetForNextTest()
    {

        //Resetting data
        timeToCorrectObject = 0;

        timeOnWrong = 0;
        correctObjectFound = false;
        time = 0;
        timeOnCorrectObject = 0;


    }
    public void StoreData()
    {
        myWriter.CompileData();
    }


}
