/*
 * Caleb Kahn
 * Assignment 2
 * Adds score once player enters trigger zone
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnterTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerZone")) {
            ScoreManager.score++;
        }
    }
}
