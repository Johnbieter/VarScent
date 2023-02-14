using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CompileMemoryTestData : MonoBehaviour
{
    // Start is called before the first frame update

    public string filename;

    [System.Serializable]
    public class MemoryData
    {
        public string objectName;
        public bool correct;
        public float timeLookedAt;
    }


    [System.Serializable]
    public class MemoryDataList
    {
        public List<MemoryData> data;
    }


    public MemoryDataList memoryDataList = new MemoryDataList();
    //References
    private GameController gameController;

    private void Start()
    {
        gameController = GetComponent<GameController>();
    }

    public void TestEndCompile()
    {

        for(var x = 0; x < gameController.correctSelected.Count; x++)
        {
            MemoryData newMemoryData = new MemoryData();
            newMemoryData.objectName = gameController.correctSelected[x].name;
            newMemoryData.correct = true;
            newMemoryData.timeLookedAt = gameController.correctSelected[x].GetComponent<Selectable>().timeLookedAt;
            memoryDataList.data.Add(newMemoryData);
        }

        for (var x = 0; x < gameController.incorrectSelected.Count; x++)
        {
            MemoryData newMemoryData = new MemoryData();
            newMemoryData.objectName = gameController.incorrectSelected[x].name;
            newMemoryData.correct = false;
            newMemoryData.timeLookedAt = gameController.incorrectSelected[x].GetComponent<Selectable>().timeLookedAt;
            memoryDataList.data.Add(newMemoryData);
        }

        PrintData();
    }

    public void PrintData()
    {

        TextWriter tw = new StreamWriter(Application.dataPath + "/" + filename + ".csv", false);
        tw.WriteLine("Object Selected, Correct, Time Hovered");
        tw.Close();

        if (memoryDataList.data.Count > 0)
        {
            tw = new StreamWriter(Application.dataPath + "/" + filename + ".csv", true);

            for (int i = 0; i < memoryDataList.data.Count; i++)
            {
                tw.WriteLine(memoryDataList.data[i].objectName + "," + memoryDataList.data[i].correct + "," + memoryDataList.data[i].timeLookedAt);

            }

            tw.Close();
        }
    }
}
