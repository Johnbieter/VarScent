using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    public string filename = "";

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
       // public int age;
    }

    [System.Serializable]
    public class DataList
    {
        public List<Data> data;
    }

    public DataList myDataList = new DataList();
    public RaycastingLogInfo info;
    private int tries;
    private void Start()
    {
        //filename = Application.dataPath + "/test.csv";
        info = GetComponent<RaycastingLogInfo>();
    }

    
    public void CompileData()
    {

       
            Data newObjectData = new Data();
            newObjectData.scent = info.objectsToLog[info.correctObject].name;
            newObjectData.timeToObject = info.timeToCorrectObject;

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
        newObjectData.timeOnWrong = info.timeOnWrong;
            newObjectData.timeOnCorrect = info.timeOnCorrectObject;
          
            myDataList.data.Add(newObjectData);

        
    }
    public void WriteCSV()
    {

        TextWriter tw = new StreamWriter(Application.dataPath + "/" + filename + ".csv", false);
        tw.WriteLine("Scent, timeToObject, timeonwrong, timeoncorrect");
        tw.Close();

        if (myDataList.data.Count > 0)
        {
            tw = new StreamWriter(Application.dataPath + "/" + filename + ".csv", true);

            for (int i = 0; i < myDataList.data.Count; i++)
            {
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

            tw.Close();
        }
    }
}
