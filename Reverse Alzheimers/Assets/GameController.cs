using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float currentTime = 10;

    [SerializeField] private TMP_Text timer;

    [SerializeField] private GameObject player;

    [SerializeField] Transform spawnpoint;

    private void FixedUpdate()
    {
        if (currentTime <= 0)
        {
            player.transform.position = spawnpoint.position;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }

        timer.text = currentTime.ToString();
    }
}
