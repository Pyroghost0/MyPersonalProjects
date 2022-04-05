/* Caleb Kahn
 * Assignment 7
 * Manages the spawning of enemies and manages text UI
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    private float spawnRamge = 9;
    public int enemyCount;
    public int waveNumber = 1;
    public bool lost = false;
    public bool won = false;
    public Text waveText;
    private bool displayingStart = true;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(1);
        SpawnPowerup(1);
        Time.timeScale = 0;
    }

    private void SpawnEnemyWave(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private void SpawnPowerup(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRamge, spawnRamge);
        float spawnPosZ = Random.Range(-spawnRamge, spawnRamge);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (displayingStart && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            waveText.text = "Wave: 1";
        }
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(enemyCount == 0)
        {
            if (waveNumber == 10)
            {
                waveText.text = "You Win! Press R to Restart";
                won = true;
                Time.timeScale = 0f;
            }
            else
            {
                waveNumber++;
                SpawnEnemyWave(waveNumber);
                SpawnPowerup(1);
                waveText.text = "Wave: " + waveNumber;
            }
        }
        if (lost || won)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
