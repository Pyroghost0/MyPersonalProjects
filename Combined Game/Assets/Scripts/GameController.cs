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
    public GameObject startScreen;
    public GameObject endScreen;
    public TextMeshProUGUI resultDescriptionText;
    public TextMeshProUGUI resultText;

    public GameObject pauseScreen;
    private bool timeScale0;
    public Image loadingImage;
    public static bool paused = false;
    public static bool fromMainMenu = false;
    public static bool fromDungeon = false;
    //public static bool doneLoading = false;

    //On start set timescale
    void Start()
    {
        StartCoroutine(FadeLoad());
    }

    IEnumerator FadeLoad()
    {
        if (fromMainMenu)
        {
            yield return new WaitUntil(() => MainMenuManager.doneLoading);
            loadingImage.gameObject.SetActive(true);
            if (Data.Exists())
            {
                TerrainGenerator.instance.Load();
            }
            else
            {
                TerrainGenerator.instance.GenerateTerrain(50);
            }
            AsyncOperation ao = SceneManager.UnloadSceneAsync("Main Menu");
            yield return new WaitUntil(() => ao.isDone);
        }
        else if (fromDungeon)
        {
            TerrainGenerator.instance.Load();
            loadingImage.gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(18.5f, -23.5f, 0f);
            fromDungeon = false;
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                TerrainGenerator.instance.Load();
            }
            //yield return new WaitUntil(() => doneLoading);
            loadingImage.gameObject.SetActive(true);
            //AsyncOperation ao = SceneManager.UnloadSceneAsync("House");
            //yield return new WaitUntil(() => ao.isDone);
        }
        float timer = 0f;
        while (timer < 1f)
        {
            loadingImage.color = new Color(0f, 0f, 0f, 1f - timer);
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        Resume();
        loadingImage.gameObject.SetActive(false);
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(AppearLoad(scene));
    }

    IEnumerator AppearLoad(string scene)
    {
        //doneLoading = false;
        Time.timeScale = 0f;
        loadingImage.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < 1f)
        {
            loadingImage.color = new Color(0f, 0f, 0f, timer);
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        loadingImage.color = Color.black;
        SceneManager.LoadScene(scene);
        //doneLoading = true;
        //yield return new WaitUntil(() => ao.isDone);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !loadingImage.gameObject.activeSelf)
        {
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        paused = true;
        pauseScreen.SetActive(true);
        timeScale0 = Time.timeScale == 0f;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        paused = false;
        pauseScreen.SetActive(false);
        if (!timeScale0)
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
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
        StartCoroutine(AppearLoad("Main Menu"));
    }

    //Quit game
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    //Respawns idol after item picked up
    /*public void DelayRecover(GameObject item, GameObject idol)
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
    }*/
}
