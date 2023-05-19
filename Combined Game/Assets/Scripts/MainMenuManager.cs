using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject eventSystem;
    public Button continueButton;
    public Image loadingImage;
    public static bool doneLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Data.Exists())
        {
            continueButton.interactable = true;
        }
        StartCoroutine(FadeLoad());
    }

    IEnumerator FadeLoad()
    {
        //if (SceneManager.sceneCount > 1)
        if (doneLoading)
        {
            //yield return new WaitUntil(() => GameController.doneLoading);
            //loadingImage.gameObject.SetActive(true);
            //canvas.SetActive(true);
            //AsyncOperation ao = SceneManager.UnloadSceneAsync("Game");
            //yield return new WaitUntil(() => ao.isDone);
        }
        else
        {
            Time.timeScale = 0f;
        }
        loadingImage.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < 1f)
        {
            loadingImage.color = new Color(0f, 0f, 0f, 1f - timer);
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        loadingImage.gameObject.SetActive(false);
    }

    IEnumerator AppearLoad()
    {
        eventSystem.SetActive(false);
        doneLoading = false;
        GameController.fromMainMenu = true;
        AsyncOperation ao = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        loadingImage.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < 1f)
        {
            loadingImage.color = new Color(0f, 0f, 0f, timer);
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        loadingImage.color = Color.black;
        MainMenuManager.doneLoading = true;
        yield return new WaitUntil(() => ao.isDone);
        canvas.SetActive(false);
    }

    public void NewGame()
    {
        Data.ClearData();
        Player.inventoryProgress = null;
        TerrainGenerator.resourceTypes = null;
        StartCoroutine(AppearLoad());
    }

    public void Continue()
    {
        Player.inventoryProgress = Data.Progress();
        TerrainGenerator.regenerate = true;
        StartCoroutine(AppearLoad());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
