/* Caleb Kahn
 * Assignment 4
 * Controls score and allows player to replay game
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Text scoreText;
    public int score = 0;

    public PlayerControler playerControlerScript;
    public bool won = false;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreText == null) {
            scoreText = FindObjectOfType<Text>();
        }

        if (playerControlerScript == null) {
            playerControlerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        }

        scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControlerScript.gameOver) {
            scoreText.text = "Score " + score;
        }

        if (playerControlerScript.gameOver && !won) {
            scoreText.text = "You Lose\nPress R to try again";
        }

        if (score >= 10) {
            playerControlerScript.gameOver = true;
            won = true;
            scoreText.text = "You Win\nPress R to try again";
        }

        if (playerControlerScript.gameOver && Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
