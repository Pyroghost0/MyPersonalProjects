/* Caleb Kahn
 * Assignment 6
 * Manages the shield on the enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : TextElements
{
    public EnemyMovement enemy;
    private TutorialManager tm;

    void Start()
    {
        tm = FindObjectOfType<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tm != null) {
            if (tm.canKill && letter != string.Empty && Input.GetKeyDown(letter) && enemy.shielded)
                {
                text.text = string.Empty;
                enemy.shielded = false;
                enemy.protect.text.text = enemy.protect.letter.ToUpper();
                if (enemy.speed != null)
                {
                    enemy.speed.text.text = enemy.speed.letter.ToUpper();
                }
                if (enemy.invisable != null)
                {
                    enemy.invisable.text.text = enemy.invisable.letter.ToUpper();
                }
                this.gameObject.SetActive(false);
            }
        }
        else if (letter != string.Empty && Input.GetKeyDown(letter) && enemy.shielded)
        {
            text.text = string.Empty;
            enemy.shielded = false;
            enemy.protect.text.text = enemy.protect.letter.ToUpper();
            if (enemy.speed != null)
            {
                enemy.speed.text.text = enemy.speed.letter.ToUpper();
            }
            if (enemy.invisable != null)
            {
                enemy.invisable.text.text = enemy.invisable.letter.ToUpper();
            }
            this.gameObject.SetActive(false);
        }
    }

}
