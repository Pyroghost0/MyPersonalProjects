/* Caleb Kahn
 * GameConroller
 * Assignment 9 (Hard)
 * Controls menus and game functions
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

    public GameObject resource;

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
            StartCoroutine(SpawnResources());
        }
    }

    //Sets up the game
    public void StartGame()
    {
        startScreen.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(SpawnResources());
    }

    //Spawns resources
    IEnumerator SpawnResources()
    {
        for (float i = 30f; i > 0f; i -= Random.Range(1f, 3f))
        {
            Instantiate(resource, RanddomSpawnPosition(), transform.rotation);
        }
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(8f, 13f) / GameObject.FindGameObjectsWithTag("Minion").Length);
            Instantiate(resource, RanddomSpawnPosition(), transform.rotation);
        }
    }

    private Vector3 RanddomSpawnPosition()
    {
        return Random.value < .5 ? new Vector3(Random.Range(-20f, 20f), Random.Range(-15f, -5f), 0f) : Random.value < .5 ? new Vector3(Random.Range(-20f, -10f), Random.Range(-15f, 5f), 0f) : new Vector3(Random.Range(10f, 20f), Random.Range(-15f, 5f), 0f);
        /*LayerMask layerMask = LayerMask.GetMask("Spawn");
        Vector2 dir = new Vector2(Mathf.Pow(Random.Range(-1f, 1f), 2), Random.Range(-.5f, .5f)).normalized;
        RaycastHit2D[] rays = Physics2D.RaycastAll(Vector2.zero, dir, 50f, layerMask);
        return new Vector2(Random.Range(transform.position.x, rays[0].point.x), Random.Range(transform.position.y, rays[0].point.y));*/
    }

    //Restarts game
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
        StopAllCoroutines();
        Time.timeScale = 0f;
        endScreen.SetActive(true);
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
}
