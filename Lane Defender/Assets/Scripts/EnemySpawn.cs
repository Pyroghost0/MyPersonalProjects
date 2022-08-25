using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySpawn : MonoBehaviour
{
    public int livesNum;
    public int scoreNum;
    public Text livesText;
    public Text scoreText;
    public Text highScoreText;
    public GameObject gameOverText;
    public GameObject[] enemies;
    public Transform[] spawnPositions;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("High").ToString();
        StartCoroutine(EndlessSpawn());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
        }
    }

    IEnumerator EndlessSpawn()
    {
        while (true)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPositions[Random.Range(0, 5)]);
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }
    }

    public void UpdateScore(int addedNum)
    {
        scoreNum += addedNum;
        scoreText.text = "Score: " + scoreNum;
    }

    public void TakeDamage()
    {
        if (livesNum > 0)
        {
            livesNum--;
            livesText.text = "Lives: " + livesNum;
            if (livesNum == 0)
            {
                Time.timeScale = 0f;
                gameOverText.SetActive(true);
            }
        }
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("High", scoreNum);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
