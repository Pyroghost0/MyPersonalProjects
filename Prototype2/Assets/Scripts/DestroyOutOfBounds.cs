/*
 * Caleb Kahn
 * Assignment 3
 * Destroys object when out of bounds and updates health if enemy goes out of bounds
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    public float topBound = 20;
    public float bottomBound = -10;

    private HealthSystem healthSystemScrypt;

    void Start()
    {
        healthSystemScrypt = GameObject.FindGameObjectWithTag("HealthSystem").GetComponent<HealthSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > topBound) {
            Destroy(gameObject);
        }
        else if(transform.position.z < bottomBound) {
            //Debug.Log("Game Over!");
            //GameObject.FindGameObjectWithTag("HealthSystem").GetComponent<HealthSystem>().TakeDamage();
            healthSystemScrypt.TakeDamage();
            Destroy(gameObject);
        }
    }
}
