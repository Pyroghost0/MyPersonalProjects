/* Caleb Kahn
 * GameConroller
 * Assignment 6 (Hard)
 * Controls menus and gamefunctions
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static bool openMainMenu = true;
    public GameObject startScreen;
    public GameObject endScreen;
    //public int score = 0;
    //public float timer = 60f;
    //public TextMeshProUGUI timerText;
    //public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultDescriptionText;

    //On start set timescale
    void Start()
    {
        if (openMainMenu)
        {
            Time.timeScale = 0f;
            startScreen.SetActive(true);
            /*if (GameObject.FindGameObjectsWithTag("BGM").Length == 1)
            {
                DontDestroyOnLoad(sceneBGM);
            }
            else
            {
                sceneBGM.SetActive(false);
            }*/
        }
        else
        {
            Time.timeScale = 1f;
            //sceneBGM.SetActive(false);
        }
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
        //endScreen.SetActive(false);
        //gameScreen.SetActive(true);
        //GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthText.text = "Health: " + GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().moneyText.text = "Money: $0";*/
        Time.timeScale = 1f;
        /*timer = 60f;
        scoreText.text = "Killed: None";*/
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

    public void Retry()
    {
        openMainMenu = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Sets up end game screen
    public void EndGame(bool won)
    {
        if (won)
        {
            resultText.text = "You Win";
            //resultDescriptionText.text = "Congratulations, Thanks For Playing My Game.";
        }
        else
        {
            resultText.text = "You Lose";
            //resultDescriptionText.text = "Try Again, I Won't Bite";
        }
        //finalScoreText.text = "Score: " + score + " Kills\nTime Remaining: " + (int)timer + " Seconds";
        StopAllCoroutines();
        Time.timeScale = 0f;
        endScreen.SetActive(true);
        //finalScoreText.text = "Score: " + score + " Kills\nTime Remaining: " + (int)timer + " Seconds";
    }

    //Switches to main menu screen
    public void MainMenu()
    {
        openMainMenu = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //endScreen.SetActive(false);
        //startScreen.SetActive(true);
    }

    //Quit game
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //Respawns idol after item picked up
    public void DelayRecover(GameObject item, GameObject idol)
    {
        StartCoroutine(DelayRecoverCoroutine(item, idol));
    }

    //Respawns idol after item picked up
    IEnumerator DelayRecoverCoroutine(GameObject item, GameObject idol)
    {
        idol.SetActive(false);
        yield return new WaitUntil(() => item == null);
        yield return new WaitForSeconds(5f);
        idol.SetActive(true);
    }
}
