/*Caleb Kahn
 * Assignment 5A
 * Displays correct score and winning text
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndEndManager : MonoBehaviour
{

    public int score = 0;
    private GameObject player;
    private bool finished = false;

    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished) {
            scoreText.text = "Score: " + score;
        }
    }

    void OnTriggerEnter2D(Collider2D theCollider)
    {
        if (theCollider.CompareTag("Player") && score == 10) 
        {
            finished = true;
            scoreText.text = "You win!";
        }
    }
}
