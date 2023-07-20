using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

/*----------------------------------------------------------------------------------------------------
This script takes all the data from DiagnosticSelectable.cs and creates a CSV file to study scent age
-----------------------------------------------------------------------------------------------------*/

public class CSVWriter : MonoBehaviour
{
    public string filename = ""; //This will be filled via UI input from DiagnosticTestController.cs

    [System.Serializable]
    public class Data
    {
        public string scent;
        public float timeToObject;
        //public int tries;
        //public int correctFirst;
        //public int streak;
        // public float concentration;
        //public float scentTime;
        public float timeOnWrong;
        public float timeOnCorrect;
        public float average;
        // public int age;
    }

    [System.Serializable]
    public class DataList
    {
        public List<Data> data;
    }

    public DataList myDataList = new DataList();
    public RaycastingLogInfo info; //Might be able to make this private
    public TMP_Text scoreTxt;
    //private int tries;
    private void Start()
    {
        //filename = Application.dataPath + "/test.csv";
        info = GetComponent<RaycastingLogInfo>();

        //If data dump folder does not exist create one
        if(!Directory.Exists(Application.dataPath + "/VARScentData"))
        {
            Directory.CreateDirectory(Application.dataPath + "/VARScentData");
        }
    }


    public void CompileData()
    {
        Data newObjectData = new Data();

        //Populates data from RaycastingLogInfo.cs in the new instance of Data class
        newObjectData.scent = info.objectsToLog[info.correctObject].name; //Gets the scent that was chosen in test
        newObjectData.timeToObject = info.timeToCorrectObject; 
        newObjectData.timeOnWrong = info.timeOnWrong;
        newObjectData.timeOnCorrect = info.timeOnCorrectObject;

        myDataList.data.Add(newObjectData);
        /*
        for (var x = 0; x < info.objectSpotted.Count; x++)
        {
            if (info.objectSpotted[x] == true)
            {
                tries += 1;
            }
        }
        */

        // newObjectData.tries = tries;

        /*
        if (tries <= 0)
        {
            newObjectData.correctFirst = 1;
        }
        else
        {
            newObjectData.correctFirst = 0;
        }
        */

        // newObjectData.streak = Random.Range(0, 4);
        // newObjectData.concentration = 0.5f;
        // newObjectData.scentTime = 10;
        //newObjectData.age = 22;
    }

    //Creates the CSV file
    public void WriteCSV()
    {
        TextWriter tw = new StreamWriter(Application.dataPath + "/VARScentData/" + filename + ".csv", false);
        tw.WriteLine("Scent, Time To Object, Time On Wrong, Time On Correct"); //Creates columns
        tw.Close();

        float score;
        float TTOAvg;
        float TOWSum = 0;
        float TOCSum = 0;
        float TTOSum = 0;

        if (myDataList.data.Count > 0)
        {
            tw = new StreamWriter(Application.dataPath + "/VARScentData/" + filename + ".csv", true); 

            for (int i = 0; i < myDataList.data.Count; i++)
            {
                TTOSum += myDataList.data[i].timeToObject;
                TOWSum += myDataList.data[i].timeOnWrong;
                TOCSum += myDataList.data[i].timeOnCorrect;
                tw.WriteLine(myDataList.data[i].scent + ","
                    + myDataList.data[i].timeToObject + ","
                    //+ myDataList.data[i].tries + ","
                    //+ myDataList.data[i].correctFirst + ","
                    // + myDataList.data[i].streak + ","
                    //+ myDataList.data[i].concentration + ","
                    //+ myDataList.data[i].scentTime + ","
                    + myDataList.data[i].timeOnWrong + ","
                    + myDataList.data[i].timeOnCorrect);
            }
            TTOAvg = TTOSum / myDataList.data.Count;

            score = 100 * (TOCSum / 60) - (TOWSum) + (15 - TTOAvg);
            scoreTxt.text = "Score: " + score.ToString("N0");
            tw.WriteLine();
            tw.WriteLine("Score:");
            tw.Write(score);
            tw.Close();
        }
    }
}
