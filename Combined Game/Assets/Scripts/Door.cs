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
    public bool canOpen = true;

    //Turns off door
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger && canOpen)
        {
            if (dungeonDoor)
            {
                //if (loadScene == "Dungeon")
                if (loadScene != "Game")
                {
                    if (collision.GetComponent<Player>().hasKey)
                    {
                        //collision.GetComponent<Player>().key.SetActive(false);
                        //collision.GetComponent<Player>().hasKey = false;
                    }
                    else
                    {
                        return;
                    }
                }
                /*else if (loadScene == "Boss")
                {
                    if (collision.GetComponent<Player>().hasKey)
                    {
                        collision.GetComponent<Player>().key.SetActive(false);
                        collision.GetComponent<Player>().hasKey = false;
                    }
                    else
                    {
                        return;
                    }
                }*/
                else
                {
                    if (collision.GetComponent<Player>().hasKey)
                    {
                        GameController.fromDungeon = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else if (Player.floatTime == 1200f)
            {
                Player.healed = false;
                Player.floatTime = 0f;
                Player.inventoryProgress[21] = Player.inventoryProgress[21] / 4 % 4 == 2 ? Player.inventoryProgress[21] : (ushort)(Player.inventoryProgress[21] + 4);
                Player.inventoryProgress[16]++;
                Data.Save(TerrainGenerator.resourceTypes, Player.inventoryProgress);
            }
            else if (Player.floatTime >= 1140f && loadScene == "House")
            {
                Player.healed = false;
                Player.floatTime = 1200f;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Tired(false);
            }
            BGM.instance.GetComponent<BGM>().Destroy(loadScene == "Game" ? MusicType.Overworld : loadScene == "House" ? MusicType.MainMenuHouse : loadScene == "Dungeon" ? MusicType.Dungeon : MusicType.Boss);
            Player.instance.doorSound.Play();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LoadScene(loadScene);
        }
    }
}
