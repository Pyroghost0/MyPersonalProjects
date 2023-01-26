/* Caleb Kahn
 * Wall
 * Assignment 2 (Hard)
 * Destroys bullets that come in contact with it
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //Destroys bullet when bullet is out of sight
    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
