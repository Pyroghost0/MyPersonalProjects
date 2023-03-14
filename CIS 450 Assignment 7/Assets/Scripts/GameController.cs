/* Caleb Kahn
 * GameConroller
 * Assignment 7 (Hard)
 * Controls menus and gamefunctions
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject sceneBGM;
    public GameObject startScreen;
    public GameObject endScreen;
    public GameObject[] doors;
    public static bool openMainMenu = true;
    public TextMeshProUGUI resultText;

    //On start set timescale
    void Start()
    {
        if (openMainMenu)
        {
            Time.timeScale = 0f;
            startScreen.SetActive(true);
            DontDestroyOnLoad(sceneBGM);
        }
        else
        {
            sceneBGM.SetActive(false);
        }
    }

    //Sets up the game
    public void StartGame()
    {
        Time.timeScale = 1f;
        startScreen.SetActive(false);
    }

    public void Retry()
    {
        openMainMenu = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //StartCoroutine(ReloadSceneRetry());
    }

    /*IEnumerator ReloadSceneRetry()
    {
        //loading.SetActive(true);
        DontDestroyOnLoad(gameObject);
        AsyncOperation ao = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        yield return new WaitUntil(() => ao.isDone);
        GameObject[] controllers = GameObject.FindGameObjectsWithTag("GameController");
        GameController otherController = controllers[0] == gameObject ? controllers[1].GetComponent<GameController>() : controllers[0].GetComponent<GameController>();
        otherController.openMainMenu = false;
        otherController.StartGame();
        Destroy(gameObject);
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
        openMainMenu = true;
        Time.timeScale = 1f;
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
}
