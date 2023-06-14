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
    public static bool wentToHouse = false;

    public AudioSource longButtonSound;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Player.select = 0;
        if (Data.Exists())
        {
            Player.inventoryProgress = Data.Progress();
            musicMixer.SetFloat("Volume", (Mathf.Log10(GameController.musicValue + 1f) * 50f) - 80f);
            effsctsMixer.SetFloat("Volume", (Mathf.Log10(GameController.effectsValue + 1f) * 50f) - 80f);
            continueButton.interactable = true;
        }
        else
        {
            continueButton.interactable = false;
        }
        StartCoroutine(FadeLoad());
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Potion");
        }
    }*/

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
        longButtonSound.Play();
        BGM.instance.GetComponent<BGM>().Destroy(MusicType.Overworld);
        UpdatePlayer();
        eventSystem.SetActive(false);
        doneLoading = false;
        loadingImage.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < 1f)
        {
            loadingImage.color = new Color(0f, 0f, 0f, timer);
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
        }
        loadingImage.color = Color.black;
        GameController.fromMainMenu = true;
        wentToHouse = Player.inventoryProgress[16] % 4 >= 1;
        if (Player.inventoryProgress[16] % 4 == 2)
        {
            Player.inventoryProgress[16]--;
            Player.inventoryProgress[21] -= 4;//Resets when you go outside
        }
        Debug.Log(wentToHouse);
        AsyncOperation ao = SceneManager.LoadSceneAsync(wentToHouse ? "House" : "Game", LoadSceneMode.Additive);
        yield return new WaitUntil(() => ao.isDone);
        doneLoading = true;
        canvas.SetActive(false);
    }

    public static void UpdatePlayer()
    {
        Player.maxHealth = 4 + (Player.inventoryProgress[20] % 2) + (Player.inventoryProgress[20] / 2 % 2) + (Player.inventoryProgress[20] / 4 % 2) + (Player.inventoryProgress[20] / 8 % 2);
        Player.health = Player.maxHealth;
        Player.healed = false;
        Player.hasPickaxe = Player.inventoryProgress[18] / 16 == 1;
        Player.bulletCooldownTime = (Player.inventoryProgress[19] / 1) % 2 == 1 ? .45f : .6f;
        Player.gatherEfficiency[1] = (Player.inventoryProgress[19] / 4) % 2 == 1 ? 2f : 1f;
        Player.gatherEfficiency[2] = (Player.inventoryProgress[19] / 8) % 2 == 1 ? 2f : 1f;
        Player.floatTime = Player.inventoryProgress[16] % 4 == 0 ? 240f : Player.inventoryProgress[16] % 4 == 1 ? 1200f : 0f;
        UpgradeStations.potionAmount = (short)((Player.inventoryProgress[19] / 2) % 2 == 1 ? 8 : 5);
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
        TerrainGenerator.regenerate = true;
        StartCoroutine(AppearLoad());
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
