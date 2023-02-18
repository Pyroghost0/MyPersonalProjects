/* Caleb Kahn
 * Common
 * Assignment 5 (Hard)
 * Commons do nothing
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common : Item
{
    //Collects nothing
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !doublePickUp)
        {
            doublePickUp = true;
            StartCoroutine(CollectionAnimation());
            //Destroy(gameObject);
        }
    }
}
