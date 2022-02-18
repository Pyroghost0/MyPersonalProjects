/* Caleb Kahn
 * Assignment 4
 * Moves the object left and destroys obsticles off screen
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{

    public float speed = 30f;
    private float leftBound = -15;

    private PlayerControler playerControlerScript;

    private void Start()
    {
        playerControlerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!playerControlerScript.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
