/* Caleb Kahn
 * House
 * Assignment 9 (Hard)
 * Slowly adds resources to the house to build it
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    public int spriteNum = 0;

    public void AddPhase()
    {
        if (spriteNum != sprites.Length-1)
        {
            spriteNum++;
            spriteRenderer.sprite = sprites[spriteNum];
            if (spriteNum == sprites.Length-1)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame(true);
            }
        }
    }
}
