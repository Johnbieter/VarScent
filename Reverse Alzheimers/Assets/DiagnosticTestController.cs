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

    public GameObject[] potentialCorrect;

    //This is the correct object for each test.
    public GameObject correctObject;
    //This is just a list of random incorrect objects.
    public GameObject[] incorrectObjects;



    public void ConfigureTest()
    {
        //Set correct object
        int rand = Random.Range(0, 3);
        atomizerControl.currentScent = rand;


        if (atomizerControl.atomizerContents[rand] == Arduino_Setting_Polling_Read_Write.Scents.Pine)
        {
            //Correct object = Pine
            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Pine")
                {
                    correctObject = potentialCorrect[i];
                }
            }
        }

        if (atomizerControl.atomizerContents[rand] == Arduino_Setting_Polling_Read_Write.Scents.Citrus)
        {
            // correctObject = citrus;

            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Citrus")
                {
                    correctObject = potentialCorrect[i];
                }
            }
        }
        if (atomizerControl.atomizerContents[rand] == Arduino_Setting_Polling_Read_Write.Scents.Peanut_Butter)
        {
            // correctObject = peanut_butter;

            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Peanut_Butter")
                {
                    correctObject = potentialCorrect[i];
                }
            }

        }
        if (atomizerControl.atomizerContents[rand] == Arduino_Setting_Polling_Read_Write.Scents.Lavender)
        {
            // correctObject = Lavendar;
            for (var i = 0; i < potentialCorrect.Length; i++)
            {
                if (potentialCorrect[i].name == "Lavendar")
                {
                    correctObject = potentialCorrect[i];
                }
            }
        }




        //Fire Scent
        atomizerControl.RunTest();
        //Begin Test
        
    }
    public void BeginTest()
    { 
        
    }
}
