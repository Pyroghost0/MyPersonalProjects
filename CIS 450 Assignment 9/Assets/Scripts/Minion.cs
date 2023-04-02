/* Caleb Kahn
 * Minion
 * Assignment 9 (Hard)
 * Minion that gathers resources to build a house
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    public MinionState currentState;
    public MinionState collectingState;
    public MinionState returningState;
    public bool hasResource = false;
    public float speed = 1.5f;

    void Start()
    {
        collectingState = gameObject.AddComponent<Collecting>();
        returningState = gameObject.AddComponent<Returning>();
        Collect();
    }

    public void Return() {
        currentState = returningState;
        currentState.Return(); 
    }

    public void Collect()
    {
        currentState = collectingState;
        currentState.StartCollecting();
    }

    //Killed by colllision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().spawner.EnemyDeath();
            Instantiate(collision.GetComponent<Enemy>().smoke, collision.transform.position, transform.rotation);
            Destroy(collision.gameObject);
            if (GameObject.FindGameObjectsWithTag("Minion").Length == 1)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            if (GameObject.FindGameObjectsWithTag("Minion").Length == 1)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
