using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class GameController : MonoBehaviour
{
    [SerializeField] private float timeToRemember;

    [SerializeField] private TMP_Text timer;

    [SerializeField] private GameObject player;

    [SerializeField] Transform spawnpoint;

    [SerializeField] private float timeToTeleport;

    [SerializeField] private float timeToTest;

    private bool testStarted;
    private bool testFinished;
    private bool timerFinished = false;
    

    public UnityEvent onTimerFinished;
    public UnityEvent onTestStart;
    public UnityEvent onTestFinshed;

    public List<GameObject> correctSelected;
    public List<GameObject> incorrectSelected;
    private void FixedUpdate()
    {
        if (!timerFinished)
        {
            if (timeToRemember <= 0)
            {
                onTimerFinished.Invoke();
                Invoke("MoveToTest", timeToTeleport);
                timerFinished = true;
            }
            else
            {
                timeToRemember -= Time.deltaTime;
            }
        }
        timer.text = timeToRemember.ToString();
        if (!testFinished)
        {
            if (testStarted)
            {
                if (timeToTest <= 0)
                {
                    onTestFinshed.Invoke();
                    testFinished = true;
                }
                else
                {
                    timeToTest -= Time.deltaTime;
                }
            }
        }
    }

    public void MoveToTest()
    {
        player.transform.position = spawnpoint.position;
        onTestStart.Invoke();
        testStarted = true;
    }

}
