/* Caleb Kahn
 * GameConroller
 * Assignment 6 (Hard)
 * Controls menus and gamefunctions
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
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
    public static bool fromPotion = false;
    public Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
    //public static bool doneLoading = false;

    public GameObject tutorialScreen;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI explainationText;
    public static int musicValue { get { return Player.inventoryProgress[23] % 101; } set { Player.inventoryProgress[23] += (ushort)(value - (Player.inventoryProgress[23] % 101)); } }
    public static int effectsValue { get { return Player.inventoryProgress[23] / 101; } set { Player.inventoryProgress[23] = (ushort)((value*101) + (Player.inventoryProgress[23] % 101)); } }
    public GameObject[] upgradeStations;
    public GameObject[] enemyPrefabs;
    public Slider musicSlider;
    public Slider effectsSlider;
    public TextMeshProUGUI musicSliderText;
    public TextMeshProUGUI effectsSliderText;
    public AudioMixer musicMixer;
    public AudioMixer effectsMixer;

    //On start set timescale
    void Start()
    {
        musicValue = 50;
        effectsValue = 50;
        musicSlider.value = musicValue;
        effectsSlider.value = effectsValue;
        MusicSlider();
        EffectsSlider();
        Application.targetFrameRate = 60;
        if (SceneManager.GetActiveScene().name == "Potion")
        {
            Time.timeScale = 1f;
        }
        else if (fromPotion)
        {
            fromPotion = false;
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-3.4f, 1.75f);
            upgradeStations[0].GetComponent<UpgradeStations>().Interact();
        }
        else
        {
            StartCoroutine(FadeLoad());
        }
    }

    IEnumerator FadeLoad()
    {
        if (fromMainMenu)
        {
            yield return new WaitUntil(() => MainMenuManager.doneLoading);
            loadingImage.gameObject.SetActive(true);
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (Data.Exists())
                {
                    TerrainGenerator.instance.Load();
                }
                else
                {
                    TerrainGenerator.instance.GenerateTerrain(50);
                }
            }
            if (Player.floatTime == 0f)
            {
                StartCoroutine(EnemyCoroutine());
            }
            AsyncOperation ao = SceneManager.UnloadSceneAsync("Main Menu");
            yield return new WaitUntil(() => ao.isDone);
            fromMainMenu = false;
        }
        else if (fromDungeon)
        {
            TerrainGenerator.instance.Load();
            loadingImage.gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(18.5f, -23.5f, 0f);
            cinemachineVirtualCamera.ForceCameraPosition(GameObject.FindGameObjectWithTag("Player").transform.position, transform.rotation);
            fromDungeon = false;
        }
        else
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                if (Player.floatTime == 0f)
                {
                    StartCoroutine(EnemyCoroutine());
                }
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
        if (SceneManager.GetActiveScene().name == "Game" && (Player.floatTime == 0f || Player.floatTime == 240f))
        {
            StartCoroutine(DayCoroutine());
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

    IEnumerator EnemyCoroutine()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().canOpen = false;
        }
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        while (Player.floatTime < 240f)
        {
            float rotation = Random.Range(0f, 360f);
            //Instantiate(enemyPrefabs[0], player.position + new Vector3(Mathf.Sin(rotation) * 10f, Mathf.Cos(rotation) * 10f), transform.rotation).GetComponent<Enemy>().enemyColor = (EnemyColor)Random.Range(0, 2);
            yield return new WaitForSeconds(Random.Range(5f, 5f));
        }
        ushort day = (ushort)(Player.inventoryProgress[16] / 4);
        Player.inventoryProgress[16] = (ushort)(day * 4);
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().canOpen = true;
        }
        StartCoroutine(DayCoroutine());
    }

    IEnumerator DayCoroutine()
    {
        yield break;
    }

    IEnumerator DayLightCoroutine()
    {
        yield break;
    }

    public void Pause()
    {
        paused = true;
        //Player.isPaused = true;
        pauseScreen.SetActive(true);
        timeScale0 = Time.timeScale == 0f;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        paused = false;
        //Player.isPaused = false;
        pauseScreen.SetActive(false);
        if (!timeScale0)
        {
            Time.timeScale = 1f;
        }
    }

    public void Retry()
    {
        Player.healed = false;
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
        Destroy(TerrainGenerator.instance);
        StartCoroutine(AppearLoad("Main Menu"));
    }

    //Quit game
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Tutorial(string title, string explaination)
    {
        tutorialScreen.SetActive(true);
        titleText.text = title;
        explainationText.text = explaination;
        Time.timeScale = 0f;
    }

    public void ExitTutorialScreen()
    {
        tutorialScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Build(int type)
    {
        Player.inventoryProgress[17] += (ushort)Mathf.Pow(2, type);
        upgradeStations[type].SetActive(true);
        upgradeStations[type].GetComponent<UpgradeStations>().Build();
    }

    public void LoadFromPotion()
    {
        Time.timeScale = 0f;
        fromPotion = true;
        SceneManager.LoadScene("House");
    }

    public void MusicSlider()
    {
        musicSliderText.text = musicSlider.value.ToString();
        musicValue = (int)musicSlider.value;
        musicMixer.SetFloat("Volume", (Mathf.Log10(musicSlider.value + 1f) * 50f) - 80f);
    }

    public void EffectsSlider()
    {
        effectsSliderText.text = effectsSlider.value.ToString();
        effectsValue = (int)effectsSlider.value;
        effectsMixer.SetFloat("Volume", (Mathf.Log10(effectsSlider.value + 1f) * 50f) - 80f);
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
