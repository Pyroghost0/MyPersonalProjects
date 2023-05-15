/* Caleb Kahn
 * GameConroller
 * Assignment 3 (Hard)
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
    //public GameObject gameScreen;
    public GameObject endScreen;
    //public int score = 0;
    //public float timer = 60f;
    //public TextMeshProUGUI timerText;
    //public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI resultText;

    //On start set timescale
    void Start()
    {
        Time.timeScale = 0f;
    }

    //Updates timer values
    /*void Update()
    {
        if (Time.timeScale == 1f)
        {
            timer -= Time.deltaTime;
            //timerText.text = "Time Remaining: " + ((int)timer) + " Seconds";
            if (timer <= 0f)
            {
                resultText.text = "You Lose!";
                EndGame();
            }
        }
    }*/

    //Sets up the game
    public void StartGame()
    {
        startScreen.SetActive(false);
        endScreen.SetActive(false);
        //gameScreen.SetActive(true);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            //Destroy(enemies[i]);
            enemy.GetComponent<Enemy>().Reset();
        }
        GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().tileValue = 10f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().failed = false;
        Time.timeScale = 1f;
        /*timer = 60f;
        score = 0;
        scoreText.text = "Killed: None";*/
        //StartCoroutine(SwitchGun());
    }

    //Add to score
    /*public void UpdateScore()
    {
        score++;
        scoreText.text = "Score: " + score;
        if (score >= 25)
        {
            resultText.text = "You Win!";
            EndGame();
        }
    }*/

    //Sets up end game screen
    public void EndGame()
    {
        StopAllCoroutines();
        Time.timeScale = 0f;
        //gameScreen.SetActive(false);
        endScreen.SetActive(true);
        //finalScoreText.text = "Score: " + score + " Kills\nTime Remaining: " + (int)timer + " Seconds";
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
