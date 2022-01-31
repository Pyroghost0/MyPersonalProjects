/*
 * Caleb Kahn
 * Assignment 2
 * Increases score when player hits trigger zone
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseScore : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            ScoreWinLose.score++;
        }
    }
}
