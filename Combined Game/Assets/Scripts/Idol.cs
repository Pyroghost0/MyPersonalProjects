/* Caleb Kahn
 * Idol
 * Assignment 6 (Hard)
 * Spawns items when destroyed
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idol : MonoBehaviour
{
    public ItemType itemType;
    public GameObject item;

    void Start()
    {
        if (itemType != ItemType.Free)
        {
            itemType = (ItemType)Random.Range(2, 16);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") || collision.CompareTag("Explosion"))
        {
            Instantiate(item, transform.position, transform.rotation).GetComponent<Item>().itemType = itemType;
            if (collision.CompareTag("Bullet"))
            {
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
            //GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().DelayRecover(Instantiate(item, transform.position, transform.rotation), gameObject);
        }
    }
}