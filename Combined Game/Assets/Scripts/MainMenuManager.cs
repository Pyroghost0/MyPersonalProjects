using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject canvas;
    public GameObject eventSystem;
    public Button continueButton;
    public Image loadingImage;
    public AudioMixer musicMixer;
    public AudioMixer effsctsMixer;
    public static bool doneLoading = false;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Player.select = 0;
        if (Data.Exists())
        {
            musicMixer.SetFloat("Volume", (Mathf.Log10(GameController.musicValue + 1f) * 50f) - 80f);
            effsctsMixer.SetFloat("Volume", (Mathf.Log10(GameController.effectsValue + 1f) * 50f) - 80f);
            continueButton.interactable = true;
        }
        StartCoroutine(FadeLoad());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Potion");
        }
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
        Player.floatTime = Player.inventoryProgress[16] % 4 == 0 ? 240f : Player.inventoryProgress[16] % 4 == 1 ? 1200f : 0f;
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
        if (Data.Exists())
        {
            Data.ClearData();
            ushort volume = Player.inventoryProgress[23];
            Player.inventoryProgress = new ushort[24];
            Player.inventoryProgress[23] = volume;
        }
        else
        {
            Player.inventoryProgress = new ushort[24];
            Player.inventoryProgress[23] = 5100;
        }
        TerrainGenerator.resourceTypes = null;
        StartCoroutine(AppearLoad());
    }

    public void Continue()
    {
        Player.inventoryProgress = Data.Progress();
        Player.floatTime = Player.inventoryProgress[16] / 4 == 0 ? 240f : Player.inventoryProgress[16] / 4 == 1 ? 1200f : 0f;
        //Has Pickaxe
        //Apply efficiency upgrades

        TerrainGenerator.regenerate = true;
        StartCoroutine(AppearLoad());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
