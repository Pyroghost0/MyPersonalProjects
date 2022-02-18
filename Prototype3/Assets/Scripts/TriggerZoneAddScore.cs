/* Caleb Kahn
 * Assignment 4
 * Adds 1 to score when jumping over an obsticle
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneAddScore : MonoBehaviour
{

    private UIManager uIManager;
    private bool triggered = false;
    
    // Start is called before the first frame update
    void Start()
    {
        uIManager = GameObject.FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered) {
            triggered = true;
            uIManager.score++;
        }
    }
}
