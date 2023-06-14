/* Caleb Kahn
 * CrackedWall
 * Assignment 6 (Hard)
 * Wall destroyed on explosion
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedWall : MonoBehaviour
{
    public ushort num = 0;

    void Start()
    {
        if ((Player.inventoryProgress[18] / num) % 2 == 1)
        {
            Destroy(gameObject);
        }
    }

    //Turns off wall
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            Player.inventoryProgress[18] += num;
            gameObject.SetActive(false);
        }
    }
}
