using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*----------------------------------------------------------
 Controller to the tutorial area in the Diagnostic Level. It
will detect objects in the area if they were found. It then
records the score on the UI.
 -----------------------------------------------------------*/

public class TutorialController : MonoBehaviour
{
    public bool playerInsideTutorial = true;

    [SerializeField] List<GameObject> objs = new List<GameObject>();

    public Transform TestPos;
    public GameObject player;
    [SerializeField] RaycastingLogInfo logCompiler;

    
    [SerializeField] TMP_Text score;
    [SerializeField] TMP_Text TutText;
    [SerializeField] TMP_Text scoreText;

    private void Update()
    {
        if (playerInsideTutorial)
        {
            logCompiler.RecordTestInfo(objs);
            score.text = logCompiler.tutScore + "/5";
        }

        if(logCompiler.tutScore == 5)
        {
            TutText.enabled = false;
            scoreText.text = "All objects found!";
        }
    }

    public void MoveToTest()
    {
        player.transform.position = TestPos.position;
        playerInsideTutorial = false;
    }
}
