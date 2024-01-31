using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mem_Tutorial_Controller : MonoBehaviour
{
    bool playerInsideTutorial = true;

    [SerializeField] List<GameObject> objs = new List<GameObject>();

    public TMP_Text TutText;
    private int TutCounter;

    private void Update()
    {
        if (playerInsideTutorial)
        {
            foreach (GameObject obj in objs)
            {
                if (obj.GetComponent<Selectable>().selected == true && obj.GetComponent<Selectable>().tutSelected == false)
                {
                    TutCounter++;
                    obj.GetComponent<Selectable>().tutSelected = true;
                }
            }
            TutText.text = TutCounter.ToString() + "/7 objects found.";
        }

        if (TutCounter == 7)
        {
            TutText.text = "All objects found!";
        }
        else
        {
            TutText.text = TutCounter.ToString() + "/7 objects found.";
        }
    }

    public void MovedPlayer()
    {
        playerInsideTutorial = false;
    }
}
