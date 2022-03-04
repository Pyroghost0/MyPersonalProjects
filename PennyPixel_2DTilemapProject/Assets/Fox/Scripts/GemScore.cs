/*Caleb Kahn
 * Assignment 5A
 * Updates score when a gem is collected
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScore : MonoBehaviour
{

    private ScoreAndEndManager sm;

    // Start is called before the first frame update
    void Start()
    {
        sm = GameObject.FindGameObjectWithTag("ScoreAndEndManager").GetComponent<ScoreAndEndManager>();
    }

    void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player")) {
            sm.score++;
        }
    }
}
