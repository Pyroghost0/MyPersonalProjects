/* Caleb Kahn
 * GameConroller
 * Assignment 7 (Hard)
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
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultDescriptionText;

    //On start set timescale
    void Start()
    {
        if (openMainMenu)
        {
            Time.timeScale = 0f;
            startScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    //Sets up the game
    public void StartGame()
    {
        startScreen.SetActive(false);
        //endScreen.SetActive(false);
        //gameScreen.SetActive(true);
        /*GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().moneyText.text = "Money: $0";*/
        Time.timeScale = 1f;
    }

    public void Retry()
    {
        openMainMenu = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

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
