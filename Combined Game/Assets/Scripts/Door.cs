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
    public string loadScene;
    public bool dungeonDoor;

    //Turns off door
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (dungeonDoor && loadScene == "Dungeon")
            {
                if (!dungeonDoor || collision.GetComponent<Player>().hasKey)
                {
                    collision.GetComponent<Player>().key.SetActive(false);
                    collision.GetComponent<Player>().hasKey = false;
                }
                else
                {
                    return;
                }
            }
            else if (dungeonDoor)
            {
                GameController.fromDungeon = true;
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LoadScene(loadScene);
        }
    }
}
