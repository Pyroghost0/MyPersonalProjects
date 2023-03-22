/* Caleb Kahn
 * Idol
 * Assignment 7 (Hard)
 * Spawns items when destroyed
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idol : MonoBehaviour
{
    public GameObject item;

    //On destruction, it gives item
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Explosion"))
        {
            Destroy(collision.gameObject);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().DelayRecover(Instantiate(item, transform.position, transform.rotation), gameObject);
        }
    }
}
