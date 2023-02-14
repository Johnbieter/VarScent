using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    
    private int index = -1;

    private void Start()
    {
       
    }


    public void ConfigureTest()
    {

        index += 1;

        //Delete objects from before
        
            for (var i = 0; i < instantiatedObjects.Count; i++)
            {
                Destroy(instantiatedObjects[i]);
            }
        instantiatedObjects = new List<GameObject>(0);

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
        if (atomizerControl.atomizerContents[index] == Arduino_Setting_Polling_Read_Write.Scents.Peanut_Butter)
        {
            Debug.Log("Peanut butter is selected");
            // correctObject = peanut_butter;

            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Peanut_Butter")
                {
                    correctObject = potentialCorrect[i];
                }
            }

        }
        if (atomizerControl.atomizerContents[index] == Arduino_Setting_Polling_Read_Write.Scents.Lavender)
        {
            Debug.Log("Lavender  is selected");
            // correctObject = Lavendar;
            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Lavendar")
                {
                    correctObject = potentialCorrect[i];
                }
            }
        }

        //Place Objects

        
        //Place correct object
        int randPos = Random.Range(0, 4);

        GameObject obj = Instantiate(correctObject, positionsTaken[randPos].position, Quaternion.identity);
        instantiatedObjects.Add(obj);
        positionsTaken.Remove(positionsTaken[randPos]);
      
        //Place random incorrect objects
            for (var i = 0; i < positionsTaken.Count; i++)
            {
             int randObject = Random.Range(0, incorrectObjects.Length);
            GameObject objIncorrect = Instantiate(incorrectObjects[randObject], positionsTaken[i].position, Quaternion.identity);
            instantiatedObjects.Add(objIncorrect);
        }


        //Fire Scent
        atomizerControl.RunTest();
        //Begin Test
        
    }
    public void BeginTest()
    { 
        
    }
}
