/* Caleb Kahn
 * GameConroller
 * Assignment 4 (Hard)
 * Controls menus and gamefunctions
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject endScreen;
    public Transform firstCheckpoint;
    public TextMeshProUGUI resultText;
    public GameObject respawnButton;
    public GameObject restartButton;
    public GameObject[] enemies;

    //On start set timescale
    void Start()
    {
        Time.timeScale = 0f;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    //Sets up the game
    public void StartGame()
    {
        startScreen.SetActive(false);
        endScreen.SetActive(false);
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Enemy>().Reset();
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Reset();
        Time.timeScale = 1f;
    }

    //Restarts to begining of the game
    public void Restart()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().checkpoint = firstCheckpoint;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().attack = new AttackPattern(new Attack(), AttackType.HeavyAttack, true);
        StartGame();
    }

    //Sets up end game screen
    public void EndGame()
    {
        StopAllCoroutines();
        Time.timeScale = 0f;
        //gameScreen.SetActive(false);
        endScreen.SetActive(true);
        if (resultText.text == "You Win")
        {
            respawnButton.SetActive(false);
            restartButton.SetActive(true);
        }
        else
        {
            respawnButton.SetActive(true);
            restartButton.SetActive(false);
        }
    }

    //Switches to main menu screen
    public void MainMenu()
    {
        endScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    //Quit game
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
