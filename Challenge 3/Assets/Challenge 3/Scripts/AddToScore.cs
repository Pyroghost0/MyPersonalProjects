/* Caleb Kahn
 * Assignment 4
 * Adds 1 to score when hitting the money
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToScore : MonoBehaviour
{

    private UIManager uIManager;
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        uIManager = GameObject.FindObjectOfType<UIManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        uIManager.score++;
    }
}