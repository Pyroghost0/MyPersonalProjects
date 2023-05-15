/* Caleb Kahn
 * Door
 * Assignment 6 (Hard)
 * Unlocks if given correct key
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Turns off door
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<Player>().hasKey)
        {
            collision.GetComponent<Player>().key.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
