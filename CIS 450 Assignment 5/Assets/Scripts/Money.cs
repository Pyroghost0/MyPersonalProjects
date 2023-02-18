/* Caleb Kahn
 * Money
 * Assignment 5 (Hard)
 * Money for gacha
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Item
{
    public int moneyAmount = 1;

    //Collects money
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !doublePickUp)
        {
            doublePickUp = true;
            collision.GetComponent<Player>().money += moneyAmount;
            collision.GetComponent<Player>().moneyText.text = "Money: $" + collision.GetComponent<Player>().money;
            StartCoroutine(CollectionAnimation());
            //Destroy(gameObject);
        }
    }
}
