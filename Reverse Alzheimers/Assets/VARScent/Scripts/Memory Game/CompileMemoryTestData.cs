using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/*-------------------------------------------------------------------------------------
 This will save all the data collected during scene runtime to a CSV file.
 --------------------------------------------------------------------------------------*/

public class CompileMemoryTestData : MonoBehaviour
{
    //Class for what data the file will store
    [System.Serializable]
    public class MemoryData
    {
        public string objectName;
        public bool correct;
        public bool wasSelected;
        public float timeLookedAt;
        public float timeSelected;

    }

    [System.Serializable]
    public class MemoryDataList
    {
        public List<MemoryData> data;
    }



    [System.Serializable]
    public class ExtraData
    {
        public string objectName;
        public bool correct;
        public bool wasSelected;
        public float timeLookedAt;
    }

    [System.Serializable]
    public class ExtraDataList
    {
        public List<ExtraData> data;
    }



    public MemoryDataList memoryDataList = new MemoryDataList();

    public ExtraDataList extraDataList = new ExtraDataList();

    //References
    private GameController gameController;
    public string filename;

    private void Start()
    {
        gameController = GetComponent<GameController>();
    }

    public void TestEndCompile()
    {

        for(var x = 0; x < gameController.objectList.Count; x++)
        {
            //Creates a new instance of the MemoryData class to store values in. This will be saved in the CSV file.
            MemoryData newMemoryData = new MemoryData();
            newMemoryData.objectName = gameController.objectList[x].name;
            newMemoryData.correct = gameController.objectList[x].GetComponent<Selectable>().correct;
            newMemoryData.wasSelected = gameController.objectList[x].GetComponent<Selectable>().selected;
            newMemoryData.timeLookedAt = gameController.objectList[x].GetComponent<Selectable>().timeLookedAt;
            newMemoryData.timeSelected = gameController.objectList[x].GetComponent<Selectable>().timeSelected;
            memoryDataList.data.Add(newMemoryData);
        }

        /*
        for (var x = 0; x < gameController.incorrectSelected.Count; x++)
        {
            MemoryData newMemoryData = new MemoryData();
            newMemoryData.objectName = gameController.incorrectSelected[x].name;
            newMemoryData.correct = false;
            newMemoryData.timeLookedAt = gameController.incorrectSelected[x].GetComponent<Selectable>().timeLookedAt;
            memoryDataList.data.Add(newMemoryData);
        }
        */

        PrintData();
    }

    //Saves data to a CSV file
    public void PrintData()
    {
        TextWriter tw = new StreamWriter(Application.dataPath + "/" + filename + ".csv", false);
        tw.WriteLine("Object Selected, Correct, Was Selected, Time Hovered, Time Selected");
        tw.Close();

        if (memoryDataList.data.Count > 0)
        {
            tw = new StreamWriter(Application.dataPath + "/" + filename + ".csv", true); //Might put this in a Data Dump folder - C

            for (int i = 0; i < memoryDataList.data.Count; i++)
            {
                tw.WriteLine(memoryDataList.data[i].objectName + "," + memoryDataList.data[i].correct + "," + memoryDataList.data[i].wasSelected + "," + memoryDataList.data[i].timeLookedAt + "," + memoryDataList.data[i].timeSelected);

            }

            tw.Close();
        }
        //ExtraDataCompile();
    }

    /*
    public void ExtraDataCompile()
    {

        for (var x = 0; x < gameController.objectList.Count; x++)
        {
            ExtraData newExtraData = new ExtraData();
            newExtraData.objectName = gameController.objectList[x].name;
            newExtraData.correct = gameController.objectList[x].GetComponent<Selectable>().correct;
            newExtraData.wasSelected = gameController.objectList[x].GetComponent<Selectable>().selected;
            newExtraData.timeLookedAt = gameController.objectList[x].GetComponent<Selectable>().timeLookedAt;
            extraDataList.data.Add(newExtraData);
        }

        PrintExtraData();
    }

    public void PrintExtraData()
    {
        TextWriter tw = new StreamWriter(Application.dataPath + "/" + filename + "AllData" + ".csv", false);
        tw.WriteLine("Object List, Correct, Time Hovered");
        tw.Close();

        if (extraDataList.data.Count > 0)
        {
            tw = new StreamWriter(Application.dataPath + "/" + filename + ".csv", true);

            for (int i = 0; i < extraDataList.data.Count; i++)
            {
                tw.WriteLine(extraDataList.data[i].objectName + "," + extraDataList.data[i].correct + "," + extraDataList.data[i].wasSelected + extraDataList.data[i].timeLookedAt);

            }

            tw.Close();
        }
    }
    */
}
