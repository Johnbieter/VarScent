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
        public string ObjectName;
        public float timeLookedAt;
        public float timeToSpotCorrectObject;
    }

    [System.Serializable]
    public class DataList
    {
        public List<Data> data;
    }

    public DataList myDataList = new DataList();
    public RaycastingLogInfo info;
    private void Start()
    {
        filename = Application.dataPath + "/test.csv";
        info = GetComponent<RaycastingLogInfo>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (var i = 0; i < info.objectsToLog.Count; i++)
            {
                Data newObjectData = new Data();
                newObjectData.ObjectName = info.objectsToLog[i].name;
                newObjectData.timeLookedAt = info.timeLookedOnObject[i];
                newObjectData.timeToSpotCorrectObject = info.timeToCorrectObject;
                myDataList.data.Add(newObjectData);
            }
            WriteCSV();
        }
            
    }
    public void WriteCSV()
    { 
        if(myDataList.data.Count > 0)
        {
            TextWriter tw = new StreamWriter(filename, false);
            tw.WriteLine("ObjectName, timeLookedAt, correctObject, timeToSpotCorrectObject");
            tw.Close();

            tw = new StreamWriter(filename, true);

            for (int i = 0; i < myDataList.data.Count; i++)
            {
                tw.WriteLine(myDataList.data[i].ObjectName + "," 
                    + myDataList.data[i].timeLookedAt + "," 
                    + myDataList.data[i].timeToSpotCorrectObject);

            }

            tw.Close();
        }
    }
}
