using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    string filename = "";

    [System.Serializable]
    public class Data
    {
        public string scent;
        public float timeToObject;
        public int tries;
        public int correctFirst;
        public int streak;
        public float concentration;
        public float scentTime;
        public float timeOnWrong;
        public float timeOnCorrect;
        public int age;
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
        filename = Application.dataPath + "/test.csv";
        info = GetComponent<RaycastingLogInfo>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Data newObjectData = new Data();
            newObjectData.scent = info.objectsToLog[info.correctObject].name;
            newObjectData.timeToObject = info.timeToCorrectObject;

                for(var x = 0; x < info.objectSpotted.Count; x++)
                {
                    if (info.objectSpotted[x] == true)
                    {
                        tries += 1;
                    }
                }

                newObjectData.tries = tries;

            if (tries <= 0)
            {
                newObjectData.correctFirst = 1;
            }
            else
            {
                newObjectData.correctFirst = 0;
            }

            newObjectData.streak = Random.Range(0, 4);
            newObjectData.concentration = 0.5f;
            newObjectData.scentTime = 10;
            newObjectData.timeOnWrong = info.timeOnWrong;
            newObjectData.timeOnCorrect = info.timeOnCorrectObject;
            newObjectData.age = 22;
            myDataList.data.Add(newObjectData);


           
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //Resetting data
            info.timeToCorrectObject = 0;

            for (var y = 0; y < info.objectSpotted.Count; y++)
            {
                info.objectSpotted[y] = false;
            }
            info.timeOnWrong = 0;
            info.correctObjectFound = false;
            info.time = 0;
            info.timeOnCorrectObject = 0;

        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            WriteCSV();
        }
            
    }
    public void WriteCSV()
    {

        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Scent, timeToObject, tries, correctfirst, streak, concentration, scenttime, timeonwrong, timeoncorrect");
        tw.Close();

        if (myDataList.data.Count > 0)
        {
            tw = new StreamWriter(filename, true);

            for (int i = 0; i < myDataList.data.Count; i++)
            {
                tw.WriteLine(myDataList.data[i].scent + ","
                    + myDataList.data[i].timeToObject + ","
                    + myDataList.data[i].tries + ","
                    + myDataList.data[i].correctFirst + ","
                    + myDataList.data[i].streak + ","
                    + myDataList.data[i].concentration + ","
                    + myDataList.data[i].scentTime + ","
                    + myDataList.data[i].timeOnWrong + ","
                    + myDataList.data[i].timeOnCorrect);
            }

            tw.Close();
        }
    }
}
