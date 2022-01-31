/*
 * Caleb Kahn
 * Assignment 2
 * Player loses when out of bounds
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -50 || transform.position.y > 80) {
            ScoreWinLose.gameOver = true;
        }
    }
}
