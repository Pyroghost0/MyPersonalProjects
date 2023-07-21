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
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI resultDescriptionText;
    public AudioSource buttonSound;
    public AudioSource settingSound;

    public static bool easyMode = false;
    public static bool openMainMenu = true;
    public static bool hideCursor = false;
    public static bool skipSeenTexts = true;
    public static bool seenEText = false;
    public static bool seenHealth = false;
    public static bool seenHands = false;

    public bool canPause = true;
    public bool isPaused = false;
    public GameObject pauseScreen;
    public Toggle mainMenuSkipToggle;
    public Toggle mainMenuHideToggle;
    public Toggle pauseSkipToggle;
    public Toggle pauseHideToggle;

    //On start set timescale
    void Start()
    {
        if (openMainMenu)
        {
            Cursor.visible = true;
            Time.timeScale = 0f;
            startScreen.SetActive(true);
            seenEText = false;
            seenHealth = false;
            seenHands = false;
            if (GameObject.FindGameObjectsWithTag("BGM").Length == 1)
            {
                DontDestroyOnLoad(sceneBGM);
            }
            else {
                sceneBGM.SetActive(false);
            }
        }
        else
        {
            buttonSound.Play();
            Time.timeScale = 1f;
            Cursor.visible = hideCursor ? false : true;
            sceneBGM.SetActive(false);
        }
        pauseScreen.SetActive(true);
        EnableSkip(skipSeenTexts);
        EnableHideCursor(hideCursor);
        pauseScreen.SetActive(false);
        //mainMenuSkipToggle.isOn = skipSeenTexts;
        //mainMenuHideToggle.isOn = hideCursor;
        //pauseSkipToggle.isOn = skipSeenTexts;
        //pauseHideToggle.isOn = hideCursor;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)) && (isPaused || Time.timeScale != 0f) && canPause) {
            isPaused = !isPaused;
            pauseScreen.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
            Cursor.visible = hideCursor ? isPaused : true;
        }
    }

    public void Resume()
    {
        buttonSound.Play();
        isPaused = !isPaused;
        pauseScreen.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.visible = hideCursor ? isPaused : true;
    }

    //Sets up the game
    public void StartGame(bool easy)
    {
        buttonSound.Play();
        easyMode = easy;
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (easy)
        {
            player.hearts = player.easyHearts;
        }
        player.hearts[0].transform.parent.gameObject.SetActive(true);
        player.health = player.hearts.Length;
        Cursor.visible = hideCursor ? false : true;
        Time.timeScale = 1f;
        startScreen.SetActive(false);
    }

    public void Retry()
    {
        openMainMenu = false;
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
    public void EndGame(bool won)
    {
        if (won)
        {
            resultText.text = "You Win";
            resultDescriptionText.text = easyMode ? "Now You're Ready For Hard Mode" : "Congratulations, Thanks For\nPlaying My Game.";
        }
        else
        {
            resultText.text = "You Lose";
            resultDescriptionText.text = "Quit Now, And Show Your Cowardice To Trashy";
        }
        StopAllCoroutines();
        Cursor.visible = true;
        Time.timeScale = 0f;
        endScreen.SetActive(true);
        //finalScoreText.text = "Score: " + score + " Kills\nTime Remaining: " + (int)timer + " Seconds";
    }

    public void EnableSkip(bool enabled)
    {
        if (!pauseScreen.activeSelf || isPaused)
        {
            settingSound.Play();
        }
        skipSeenTexts = enabled;
        mainMenuSkipToggle.isOn = enabled;
        pauseSkipToggle.isOn = enabled;
        //Debug.Log(enabled);
    }

    public void EnableHideCursor(bool enabled)
    {
        if (!pauseScreen.activeSelf || isPaused)
        {
            settingSound.Play();
        }
        hideCursor = enabled;
        mainMenuHideToggle.isOn = enabled;
        pauseHideToggle.isOn = enabled;
        //Debug.Log(enabled);
    }

    //Switches to main menu screen
    public void MainMenu()
    {
        settingSound.Play();
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
}
