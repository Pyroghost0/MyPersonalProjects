/* Caleb Kahn
 * CrackedWall
 * Assignment 7 (Hard)
 * Wall destroyed on explosion
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedWall : MonoBehaviour
{
    //Turns off wall
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion"))
        {
            gameObject.SetActive(false);
        }
    }
}
