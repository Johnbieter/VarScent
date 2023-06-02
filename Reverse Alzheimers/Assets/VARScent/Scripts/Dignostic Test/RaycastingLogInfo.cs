using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-----------------------------------------------------------------------------
 Sends a raycast where player is looking. If raycast hits a selectable object,
it checks if object is the correct object or not. If correct mark how long it
took them and how long they look at the correct object. If incorrect, record
how long they look at it.
 ------------------------------------------------------------------------------*/

public class RaycastingLogInfo : MonoBehaviour
{

    public List<GameObject> objectsToLog = new List<GameObject>();
    public List<bool> objectSpotted;


    [Header("Set Correct Object")]
    public int correctObject;

    [Header("Interim Data")]
    public float timeToCorrectObject; //How long it takes individual to look at correct object
    public float timeOnCorrectObject; //How long they stay on the correct object
    public float timeOnWrong; //Time they spend on wrong object
    public bool correctObjectFound = false;
    public float time;

    [SerializeField] private string selectableTag = "Selectable";
    private Transform _selection;

    public int tutScore;

    public CSVWriter myWriter; //Might be able to make this private -- already initilized in Start()

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
                    timeToCorrectObject = time; //Marks how long it took individual to find correct object
                    correctObjectFound = true;
                }
                if (correctObjectFound)
                {
                    timeOnCorrectObject += Time.deltaTime; //records how long player is looking at correct object
                }


            }
            else
            {
                for (var i = 1; i < incorrectObj.Count; i++)
                {
                    if (lookObject.name == incorrectObj[i].name)
                    {
                        timeOnWrong += Time.deltaTime; //records how long player is looking incorrect object
                    }
                }
            }

            //If player looks away from object, deactivate selection material
            if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<DiagnosticSelectable>();
                selectionRenderer.isSelected = false;
                //selectionRenderer.material = defaultMaterial;
                _selection = null;
            }

            //If player looks at a selectable object, enable selection material
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<DiagnosticSelectable>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.isSelected = true;
                }
                _selection = selection;
            }

        }
    }
    
    public void RecordTestInfo(List<GameObject> objectList) //For the tutorial in the Diagnotic level
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            //If player looks away from object, deactivate selection material
            /*if (_selection != null)
            {
                var selectionRenderer = _selection.GetComponent<DiagnosticSelectable>();
                selectionRenderer.isSelected = false;
                //selectionRenderer.material = defaultMaterial;
                _selection = null;
            }*/

            //If player looks at a selectable object, enable selection material
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<DiagnosticSelectable>();
                if (selectionRenderer != null && selectionRenderer.isSelected == false)
                {
                    selectionRenderer.isSelected = true;
                    tutScore++;
                }
                _selection = selection;
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

    private void Update()
    {
      
       
    }


}
