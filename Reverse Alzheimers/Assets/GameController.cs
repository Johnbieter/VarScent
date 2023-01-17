using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class GameController : MonoBehaviour
{
    [SerializeField] private float currentTime = 10;

    [SerializeField] private TMP_Text timer;

    [SerializeField] private GameObject player;

    [SerializeField] Transform spawnpoint;

    [SerializeField] private float timeToTeleport;


    private bool timerFinished = false;

    public UnityEvent onTimerFinished;
    public UnityEvent onTestStart;

    public List<GameObject> correctSelected;
    public List<GameObject> incorrectSelected;
    private void FixedUpdate()
    {
        if (!timerFinished)
        {
            if (currentTime <= 0)
            {
                onTimerFinished.Invoke();
                Invoke("MoveToTest", timeToTeleport);
                timerFinished = true;
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
        timer.text = currentTime.ToString();
    }

    public void MoveToTest()
    {
        player.transform.position = spawnpoint.position;
        onTestStart.Invoke();
    }

}
