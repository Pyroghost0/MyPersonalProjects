/* Caleb Kahn
 * Assignment 6
 * Manages the tutorial by updating the scene
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public int stage = 0;
    public Text mainText;
    public Text continueText;
    public EnemyMovement enemy;
    public SpeedPowerup speed;
    public ProtectPowerup protect;
    public InvisablePowerup invisable;
    public bool speedDemenstration = false;
    public bool canKill = false;

    // Update is called once per frame
    void Update()
    {
        if (stage == 0)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                stage++;
                mainText.text = "Press Space To Pause";
            }
        }
        else if (stage == 1)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                stage++;
                mainText.text = "The Thing In The Middle Is An Enemy";
            }
        }
        else if (stage == 2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                stage++;
                mainText.text = "To Kill It Press The Indicated Button";
                continueText.text = "Kill It To Continue";
                enemy.letter = "a";
                enemy.text.text = enemy.letter.ToUpper();
            }
        }
        else if (stage == 3)
        {
            //Done On Enemy Side
        }
        else if (stage == 4)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                stage++;
                mainText.text = "The Stick Doubles The Enemy Speed";
                protect.gameObject.SetActive(false);
                invisable.gameObject.SetActive(false);
                enemy.movementSpeed = 7f;
                continueText.gameObject.SetActive(false);
            }
        }
        else if (stage == 5)
        {
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && speedDemenstration)
            {
                stage++;
                mainText.text = "The Rectangle Give The Enemy An Shield";
                enemy.gameObject.transform.position = new Vector3(0f, 5f, 0f);
                speed.gameObject.transform.position = new Vector3(0f, 5f, 0f);
                protect.gameObject.SetActive(true);
            }
        }
        else if (stage == 6)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                stage++;
                mainText.text = "The Circle Partly Changes The Text Invisable";
                invisable.gameObject.SetActive(true);
            }
        }
        else if (stage == 7)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                stage++;
                mainText.text = "Try Killing The Enemy";
                continueText.text = "Kill It To Continue";
                canKill = true;
            }
        }
        else if (stage == 8)
        {
            //Done In Enemy
        }
        else if (stage == 9)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                stage++;
                mainText.text = "If A Single Enemy Reaches The End, You Lose";
            }
        }
        else if (stage == 10)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                stage++;
                mainText.text = "Now Survive In Endless Mode As Long As You Can";
                continueText.text = "Press Any Arrow Key To Return To The Menu";
            }
        }
        else if (stage == 11)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
