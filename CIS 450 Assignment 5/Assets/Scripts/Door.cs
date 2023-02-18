/* Caleb Kahn
 * Door
 * Assignment 5 (Hard)
 * Unlocks if given correct key
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public KeyType keyType;

    //Turns off door
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<Player>().keyType == keyType)
        {
            gameObject.SetActive(false);
        }
    }
}
