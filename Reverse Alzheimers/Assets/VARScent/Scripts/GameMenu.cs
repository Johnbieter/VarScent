using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public Dropdown selectProgram;
    public InputField inputTime;
    public Dropdown selectObject;
    private int time;

    public void Play()
    {
        time = int.Parse(inputTime.text);
        if (selectProgram.value == 1)
        {
            
            if(inputTime != null && time < 0)
            {
                SceneManager.LoadScene("DiagnosticTestScene");
            }
        }
        else if(selectProgram.value == 2)
        {
            SceneManager.LoadScene("TherapyTestScene");
        }
    }
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
        //
    }
}
