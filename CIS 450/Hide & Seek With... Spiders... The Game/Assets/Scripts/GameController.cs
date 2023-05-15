/* Caleb Kahn
 * GameConroller
 * Assignment 6 (Hard)
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
    public GameObject[] doors;
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
            Destroy(enemy);
        }
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in items)
        {
            Destroy(item);
        }
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject spawner in spawners)
        {
            spawner.GetComponent<Spawner>().RestartSpawner();
        }
        spawners[Random.Range(0, spawners.Length)].GetComponent<Spawner>().offsetSpawn = true;
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
        GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().hasKey =false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().key.SetActive(false);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().money = 0;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().attacking = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().bulletCooldown = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().knockback = false;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().invincible = false;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health = 6;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthText.text = "Health: " + GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health;
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().moneyText.text = "Money: $0";
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
