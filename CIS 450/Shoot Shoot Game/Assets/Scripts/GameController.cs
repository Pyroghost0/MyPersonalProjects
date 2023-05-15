/* Caleb Kahn
 * GameConroller
 * Assignment 2 (Hard)
 * Summons enemies, randomises gun, keeps track of score, starts/resets game
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject gameScreen;
    public GameObject endScreen;
    public Transform[] spawnLocations;
    public Transform[] toLocations;
    public GameObject enemyPrefab;
    private int previousSpawnNumber = 0;
    public int score = 0;
    public float timer = 60f;
    public float gunTimer = 10f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gunTimerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI finalScoreText;
    public Image gunImage;
    public Image bulletImage;
    public Sprite autoGunSprite;
    public Sprite chargeGunSprite;
    public Sprite straightBulletSprite;
    public Sprite spinBulletSprite;
    public GameObject bulletReference;

    //Updates timer values
    void Update()
    {
        if (Time.timeScale == 1f)
        {
            timer -= Time.deltaTime;
            timerText.text = "Time Remaining: " + ((int)timer) + " Seconds";
            gunTimer -= Time.deltaTime;
            gunTimerText.text = "Gun Refresh: " + ((int)gunTimer) + " Seconds";
            if (timer <= 0f)
            {
                resultText.text = "You Lose!";
                EndGame();
            }
        }
    }

    //Sets up the game
    public void StartGame()
    {
        startScreen.SetActive(false);
        endScreen.SetActive(false);
        gameScreen.SetActive(true);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            Destroy(bullets[i]);
        }
        Time.timeScale = 1f;
        timer = 60f;
        score = 0;
        scoreText.text = "Killed: None";
        StartCoroutine(Spawn());
        StartCoroutine(SwitchGun());
    }

    //Spawns enemies
    IEnumerator Spawn()
    {
        while (Time.timeScale == 1f)
        {
            yield return new WaitForSeconds(Random.Range(.3f, 1f));
            int num = (previousSpawnNumber + Random.Range(1, spawnLocations.Length)) % spawnLocations.Length;
            previousSpawnNumber = num;
            Enemy enemy = GameObject.Instantiate(enemyPrefab, spawnLocations[num]).GetComponent<Enemy>();
            enemy.gameController = this;
            enemy.target = toLocations[num];
        }
    }

    //Switches gun type every 10 seconds
    IEnumerator SwitchGun()
    {
        while (Time.timeScale == 1f)
        {
            gunTimer = 10f;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player.GetComponent<AutoGun>() != null)
            {
                if (player.GetComponent<ChargeGun>() != null)
                {
                    if (player.GetComponent<AutoGun>().enabled)
                    {
                        player.GetComponent<AutoGun>().enabled = false;
                        player.GetComponent<ChargeGun>().enabled = true;
                        player.GetComponent<ChargeGun>().bulletClass = Random.value < .5f ? (player.GetComponent<SpinBullet>() == null ? player.AddComponent<SpinBullet>() : player.GetComponent<SpinBullet>()) : (player.GetComponent<StraightBullet>() == null ? player.AddComponent<StraightBullet>() : player.GetComponent<StraightBullet>());
                        bulletImage.sprite = player.GetComponent<ChargeGun>().bulletClass is SpinBullet ? spinBulletSprite : straightBulletSprite;
                        gunImage.sprite = chargeGunSprite;
                    }
                    else
                    {
                        player.GetComponent<ChargeGun>().enabled = false;
                        player.GetComponent<AutoGun>().enabled = true;
                        player.GetComponent<AutoGun>().bulletClass = Random.value < .5f ? (player.GetComponent<SpinBullet>() == null ? player.AddComponent<SpinBullet>() : player.GetComponent<SpinBullet>()) : (player.GetComponent<StraightBullet>() == null ? player.AddComponent<StraightBullet>() : player.GetComponent<StraightBullet>());
                        bulletImage.sprite = player.GetComponent<AutoGun>().bulletClass is SpinBullet ? spinBulletSprite : straightBulletSprite;
                        gunImage.sprite = autoGunSprite;
                    }
                }
                else
                {
                    player.GetComponent<AutoGun>().enabled = false;
                    player.AddComponent<ChargeGun>();
                    gunImage.sprite = chargeGunSprite;
                }
            }
            else if (player.GetComponent<ChargeGun>() != null)
            {

                player.GetComponent<ChargeGun>().enabled = false;
                player.AddComponent<AutoGun>();
                gunImage.sprite = autoGunSprite;
            }
            else if (Random.value < .5f)
            {
                player.AddComponent<AutoGun>();
                gunImage.sprite = autoGunSprite;
            }
            else
            {
                player.AddComponent<ChargeGun>();
                gunImage.sprite = chargeGunSprite;
            }
            yield return new WaitForSeconds(10f);
        }
    }

    //Add to kill count
    public void EnemyDead()
    {
        score++;
        scoreText.text = "Killed: " + score;
        if (score >= 25)
        {
            resultText.text = "You Win!";
            EndGame();
        }
    }

    //Sets up end game screen
    public void EndGame()
    {
        StopAllCoroutines();
        Time.timeScale = 0f;
        gameScreen.SetActive(false);
        endScreen.SetActive(true);
        finalScoreText.text = "Score: " + score + " Kills\nTime Remaining: " + (int)timer + " Seconds";
    }

    //Switches to main menu screen
    public void MainMenu()
    {
        endScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    //Quit game
    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
