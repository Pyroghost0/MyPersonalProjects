/* Caleb Kahn
 * Gacha
 * Assignment 5 (Hard)
 * Spawns random item for money
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gacha : MonoBehaviour
{
    public int cost = 1;
    public KeyType keyType = KeyType.Yellow;
    public Transform spawnPlace;
    public GameObject buyPrompt;
    public bool inRadius = false;

    //Spawns item when E is pressed
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inRadius)
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player.money >= cost)
            {
                player.money -= cost;
                player.moneyText.text = "Money: $" + player.money;
                GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(true, spawnPlace.position, 0, keyType);
            }
        }
    }

    //Show prompt when in circle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRadius = true;
            buyPrompt.SetActive(true);
        }
    }

    //Hide prompt when outside circle
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRadius = false;
            buyPrompt.SetActive(false);
        }
    }
}
