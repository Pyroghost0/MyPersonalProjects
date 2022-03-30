/* Caleb Kahn
 * Assignment 6
 * Manages endless mode through its spawning objects
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndlessManager : MonoBehaviour
{
    public float timer;
    public int enemyAmount;
    public int powerUpAmount;
    public GameObject[] prefabs;
    public float respawnTime;
    public float movementSpeed;
    public Text score;
    public Text finalScore;
    public GameObject menu;

    private float cooldown;
    private float minCooldown;
    private float randomCooldown;
    private int randomChance;
    private int maxEnemies;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        enemyAmount = 0;
        powerUpAmount = 0;
        respawnTime = 20f;
        movementSpeed = 4f;
        cooldown = 0;
        minCooldown = .8f;
        randomChance = 86;
        maxEnemies = 5;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.active && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
        timer += Time.deltaTime;
        score.text = "Time: " + (int)timer;
        cooldown += Time.deltaTime;
        randomCooldown += Time.deltaTime;
        if (minCooldown <= cooldown)
        {
            if (enemyAmount == 0)
            {
                SpawnEnemy();
            }
            else if (powerUpAmount == 0)
            {
                SpawnPowerUp();
            }
            if (randomCooldown >= .1f)
            {
                if (timer <= 30) {
                    minCooldown = .8f - (.4f * (timer / 30f));
                    randomChance = (int)(86f - (15f * (timer / 30f)));
                    movementSpeed = 4f + (1.5f * (timer / 30f));
                    respawnTime = 20f - (8f * (timer / 30f));
                }
                else if (timer <= 60)
                {
                    maxEnemies = 7;
                    randomChance = (int)(71f - (15f * ((timer-30f) / 30f)));
                    movementSpeed = 5.5f + (1.5f * ((timer-30f) / 30f));
                    respawnTime = 12f - (4f * (timer / 30f));
                }
                else if (timer <= 120)
                {
                    maxEnemies = 9;
                    randomChance = (int)(61f - (10f * ((timer - 60f) / 60f)));
                    movementSpeed = 7f + (5f * ((timer-60f) / 60f));
                }
                else
                {
                    maxEnemies = 11;
                    movementSpeed = 12f + (6f * ((timer - 120f) / 60f));
                }

                randomCooldown = 0;
                int choice = Random.Range(-1, randomChance);
                if ((choice == -1 || (choice == 0 && enemyAmount < powerUpAmount)) && enemyAmount < maxEnemies)
                {
                    SpawnEnemy();
                }
                else if (choice == 0 && powerUpAmount < maxEnemies)
                {
                    SpawnPowerUp();
                }
            }
        }
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
        if (FindObjectOfType<GameManager>() != null)
        {
            FindObjectOfType<GameManager>().canPause = false;
        }
        finalScore.text = "Time Survived: " + (int)timer;
        menu.SetActive(true);
    }

    private void SpawnEnemy()
    {
        cooldown = 0;
        Vector3 spawnPos = new Vector3(-53, 5, 12f * Random.Range(-1, 2));
        Instantiate(prefabs[3], spawnPos, prefabs[3].transform.rotation);
        enemyAmount++;
    }

    private void SpawnPowerUp()
    {
        cooldown = 0;
        int choice = Random.Range(0, 3);
        if (choice == 0)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-45, 1)+3, 10, 12f * Random.Range(-1, 2) );
            Instantiate(prefabs[0], spawnPos, prefabs[0].transform.rotation);
        }
        else if (choice == 1)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-45, 1) - 3, 10, (12f * Random.Range(-1, 2)) + 3);
            Instantiate(prefabs[1], spawnPos, prefabs[1].transform.rotation);
        }
        else
        {
            Vector3 spawnPos = new Vector3(Random.Range(-45, 1), 10, 12f * Random.Range(-1, 2));
            Instantiate(prefabs[2], spawnPos, prefabs[2].transform.rotation);
        }
        powerUpAmount++;
    }
}
