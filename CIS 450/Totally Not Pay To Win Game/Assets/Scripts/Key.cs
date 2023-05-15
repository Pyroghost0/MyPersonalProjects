/* Caleb Kahn
 * Key
 * Assignment 5 (Hard)
 * Key to unlock door
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{
    None = 0,
    Yellow = 1,
    Blue = 2,
    Red = 3
}

public class Key : Item
{
    public KeyType keyType;

    //Collects key
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !doublePickUp)
        {
            doublePickUp = true;
            collision.GetComponent<Player>().keyType = keyType;
            StartCoroutine(CollectionAnimation());
            //Destroy(gameObject);
        }
    }
}
