using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{

    public static bool gameOver;
    public static bool won;
    public static int score;

    public Text textbox;

    void Start() {
        gameOver = false;
        won = false;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver) {
            textbox.text = "Score: " + score;
        }

        if (score > 2) {
            won = true;
            gameOver = true;
        }

        if (gameOver) { 
            if (won) {
                textbox.text = "You win!\nPress R to Try Again";
            }
            else {
                textbox.text = "You lose!\nPress R to Try Again";
            }
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
