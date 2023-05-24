using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------
 Records how long the player has looked at any selectable object
 ----------------------------------------------------------------*/

public class MemoryEyeInformation : MonoBehaviour
{
    public List<Selectable> objectsToLog;

    [SerializeField] Camera cam;
    [Header("Interim Data")]
    public float timeOnObject;

    public bool start = false;


    private void Start()
    {
        GameObject[] selectableGameobjects = GameObject.FindGameObjectsWithTag("Selectable");

        for (var i = 0; i < selectableGameobjects.Length; i++)
        {
            objectsToLog.Add(selectableGameobjects[i].GetComponent<Selectable>()); //Add all the selectable objects into a list
        }
    }

    public void Update()
    {
        if (start)
        {
            //Debug.Log("This is an update function");
            RaycastHit hit;

            if (Physics.SphereCast(cam.transform.position, 0.75f, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
            {
                //Debug.Log("We are sphere casting");
                //Gizmos.DrawSphere(cam.transform.position, 1000f);

                GameObject lookObject = hit.transform.gameObject;

                if (hit.transform.gameObject.GetComponent<Selectable>()) //If the object we are looking at has a selectable component
                {
                    //Debug.Log("We hit an object");
                    hit.transform.gameObject.GetComponent<Selectable>().timeLookedAt += Time.deltaTime; //Record how long player is looking at object
                }



            }
        }
    }

    public void TestStart()
    {
        start = true;
    }

}
