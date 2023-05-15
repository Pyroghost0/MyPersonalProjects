/*
 * Caleb Kahn
 * Assignment 5B
 * Adds health to an object and allows it and something else to be destroyed at 0
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public float health = 50f;
    public GameObject targetDestroy;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        if (targetDestroy != null)
        {
            Destroy(targetDestroy);
        }
    }
}
