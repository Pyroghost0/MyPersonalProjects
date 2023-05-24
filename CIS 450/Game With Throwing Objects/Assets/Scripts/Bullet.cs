/* Caleb Kahn
 * Bullet
 * Assignment 7 (Hard)
 * Destroys bullet on contact with walls
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Destroyed from contact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door") || collision.gameObject.CompareTag("Cracked Wall"))
        {
            Destroy(gameObject);
        }
    }
}