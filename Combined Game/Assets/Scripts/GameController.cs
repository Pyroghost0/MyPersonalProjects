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

    public GameObject tiredScreen;
    public GameObject tutorialScreen;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI explainationText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI savingText;
    public RectTransform dayTextTransform;
    public RectTransform savingTextTransform;

    public static int musicValue { get { return Player.inventoryProgress[23] % 101; } set { Player.inventoryProgress[23] += (ushort)(value - (Player.inventoryProgress[23] % 101)); } }
    public static int effectsValue { get { return Player.inventoryProgress[23] / 101; } set { Player.inventoryProgress[23] = (ushort)((value*101) + (Player.inventoryProgress[23] % 101)); } }
    public GameObject[] upgradeStations;
    public GameObject[] enemyPrefabs;
    public GameObject pickaxeSlimePrefab;
    public Slider musicSlider;
    public Slider effectsSlider;
    public TextMeshProUGUI musicSliderText;
    public TextMeshProUGUI effectsSliderText;
    public AudioMixer musicMixer;
    public AudioMixer effectsMixer;

    public AudioSource longButtonSound;
    public AudioSource shortButtonSound;

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
            shortButtonSound.Play();
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
            loadingImage.gameObject.SetActive(true);
            loadingImage.color = Color.black;
            yield return new WaitUntil(() => MainMenuManager.doneLoading);
            yield return new WaitForEndOfFrame();
            if (SceneManager.GetSceneByName("Game").IsValid())
            {
                if (Player.floatTime == 0f)
                {
                    TerrainGenerator.instance.GenerateTerrain(35);
                    StartCoroutine(EnemyCoroutine());
                }
                else if (Player.inventoryProgress[16] == 0)
                {
                    Debug.Log("Tutorial");
                }
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
                if (TerrainGenerator.instance != null)
                {
                    TerrainGenerator.parent = SceneManager.GetSceneByName("Game").GetRootGameObjects()[0].transform;
                    TerrainGenerator.instance.Load();
                }
                else
                {
                    yield return new WaitForEndOfFrame();//Wait for it to generate if load to house
                }
                if (Player.floatTime == 0f)
                {
                    TerrainGenerator.instance.GenerateTerrain(35);
                    StartCoroutine(EnemyCoroutine());
                }
            }
            else if (Player.floatTime == 1200f)
            {
                StartCoroutine(SavingCoroutine());
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
        Resume(false);
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
                Resume(false);
            }
            else
            {
                Pause();
            }
        }
    }

    IEnumerator EnemyCoroutine()
    {
        if (House.isMade)
        {
            StartCoroutine(SavingCoroutine());
        }
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().canOpen = false;
        }
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        ushort day = (ushort)(Player.inventoryProgress[16] / 4);
        bool spawnedPickaxe = Player.hasPickaxe;
        while (Player.floatTime < 240f)
        {
            float rotation = Random.Range(0f, 360f);
            if (spawnedPickaxe || Player.floatTime < 120f)
            {
                Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], player.position + new Vector3(Mathf.Sin(rotation) * 10f, Mathf.Cos(rotation) * 10f), transform.rotation);
            }
            else
            {
                spawnedPickaxe = true;
                Instantiate(pickaxeSlimePrefab, player.position + new Vector3(Mathf.Sin(rotation) * 10f, Mathf.Cos(rotation) * 10f), transform.rotation);
            }
            yield return new WaitForSeconds(Mathf.Min(Random.Range(3f * Mathf.Pow(1.5f, -1.5f*day)+1f, 5f * Mathf.Pow(1.5f, -day)+2f), (240f - Player.floatTime) / Player.timeMultiplier));
        }
        Player.inventoryProgress[16] = (ushort)((day+1) * 4);
        if (House.isMade)
        {
            Data.Save(TerrainGenerator.resourceTypes, Player.inventoryProgress);
            StartCoroutine(SavingCoroutine());
        }
        foreach (GameObject door in doors)
        {
            door.GetComponent<Door>().canOpen = true;
        }
        StartCoroutine(DayCoroutine());
    }

    IEnumerator DayCoroutine()
    {
        dayText.gameObject.SetActive(true);
        dayText.text = (Player.floatTime != 0f ? "Day " : "Night ")+ (Player.inventoryProgress[16] / 4 + 1);
        float timer = 0f;
        while (timer < 1f)
        {
            dayTextTransform.anchoredPosition = new Vector2(0f, Mathf.Sin(timer * .5f * Mathf.PI) * -50f);
            dayText.alpha = timer;
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        timer = 0f;
        dayTextTransform.anchoredPosition = new Vector2(0f, -50f);
        dayText.alpha = 1f;
        yield return new WaitForSeconds(2f);
        while (timer < 1f)
        {
            dayTextTransform.anchoredPosition = new Vector2(0f, -50f + (Mathf.Sin(timer * .5f * Mathf.PI) * 50f));
            dayText.alpha = 1f - timer;
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        dayText.gameObject.SetActive(false);
    }

    IEnumerator SavingCoroutine()
    {
        yield return new WaitForSeconds(2f);
        savingText.gameObject.SetActive(true);
        float timer = 0f;
        while (timer < 1f)
        {
            savingTextTransform.anchoredPosition = new Vector2(-320f,-50f + (Mathf.Sin(timer * .5f * Mathf.PI) * 50f));
            savingText.alpha = timer;
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        timer = 0f;
        savingTextTransform.anchoredPosition = new Vector2(-320f, 0f);
        savingText.alpha = 1f;
        yield return new WaitForSeconds(2f);
        while (timer < 1f)
        {
            savingTextTransform.anchoredPosition = new Vector2(-320f, Mathf.Sin(timer * .5f * Mathf.PI) * -50f);
            savingText.alpha = 1f - timer;
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        savingText.gameObject.SetActive(false);
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

    public void Resume(bool clicked = true)
    {
        if (clicked)
        {
            shortButtonSound.Play();
        }
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
        shortButtonSound.Play();
        ushort volume = Player.inventoryProgress[23];
        Player.inventoryProgress = Data.Exists() ? Data.Progress() : new ushort[24];
        Player.inventoryProgress[23] = volume;
        MainMenuManager.UpdatePlayer();
        LoadScene("Game");//Can't die in house
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
        shortButtonSound.Play();
        if (TerrainGenerator.instance != null)
        {
            TerrainGenerator.terrain = null;
            TerrainGenerator.resourceTypes = null;
            Destroy(TerrainGenerator.instance.gameObject);
        }
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

    public void Tired()
    {
        Player.inventoryProgress[16]++;
        Data.Save(TerrainGenerator.resourceTypes, Player.inventoryProgress);
        tiredScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ContinueTired()
    {
        shortButtonSound.Play();
        StartCoroutine(AppearLoad("House"));
    }

    public void StartDayCoroutineDueToNewDay()
    {
        StartCoroutine(EnemyCoroutine());
        StartCoroutine(DayCoroutine());
    }

    public void ExitTutorialScreen()
    {
        shortButtonSound.Play();
        tutorialScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Build(int type)
    {
        longButtonSound.Play();
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
