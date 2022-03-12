/*
 * Caleb Kahn
 * Assignment 5B
 * Shows wins text when collided with
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{

    public Text message;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            message.enabled = true;
        }
    }
}
