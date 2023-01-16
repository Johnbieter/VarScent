using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingLogInfo : MonoBehaviour
{

    public List<GameObject> objectsToLog;
    public List<bool> objectSpotted;
    

    [Header("Set Correct Object")]
    public int correctObject;

    [Header("Interim Data")]
    public float timeToCorrectObject;
    public float timeOnCorrectObject;
    public float timeOnWrong;
    public bool correctObjectFound = false;
    public float time;
    void Start()
    {

        for (var i = 0; i < objectsToLog.Count; i++)
        {
            objectSpotted.Add(false);
        }
       
    }
    

    void Update()
    {
        time += Time.deltaTime;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            GameObject lookObject = hit.transform.gameObject;

            for (var i = 0; i < objectsToLog.Count; i++)
            {
                if (lookObject.name == objectsToLog[i].name)
                {
                    Debug.Log("Object found");

                    if (i == correctObject)
                    {
                        if (!correctObjectFound)
                        {
                            timeToCorrectObject = time;
                            correctObjectFound = true;
                        }
                        timeOnCorrectObject += Time.deltaTime;

                    }
                    else 
                    {
                        if (!correctObjectFound)
                        {
                            objectSpotted[i] = true;
                        }
                        timeOnWrong += Time.deltaTime;
                    }
                }
            }
            //Debug.Log("Object: " + hit.transform.gameObject.name + "Time:" + Time.time);
        }
    }


    
}
