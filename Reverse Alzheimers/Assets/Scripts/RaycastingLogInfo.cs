using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingLogInfo : MonoBehaviour
{

    public List<GameObject> objectsToLog;
    public List<float> timeLookedOnObject;

    public int correctObject;
    public float timeToCorrectObject;
    public bool correctObjectFound = false;

    private void Start()
    {

        for (var i = 0; i < objectsToLog.Count; i++)
        {
            timeLookedOnObject.Add(0f);
        }
        
    }

    void Update()
    {
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
                    timeLookedOnObject[i] += Time.deltaTime;

                    if (!correctObjectFound)
                    {
                        if (i == correctObject)
                        {
                            timeToCorrectObject = Time.time;
                            correctObjectFound = true;
                        }
                    }
                }
            }
            //Debug.Log("Object: " + hit.transform.gameObject.name + "Time:" + Time.time);
        }
    }


    
}
