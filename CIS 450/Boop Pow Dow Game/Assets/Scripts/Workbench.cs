/* Caleb Kahn
 * Workbench
 * Assignment 4 (Hard)
 * Lets player assign attacks
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Workbench : MonoBehaviour
{
    public GameObject workbenchScreen;
    public AttackType[] initialAttacks;
    public int[] chosenAttacks;
    public Image[] initialAttackImages;
    public Button[] initialAttackButtons;
    public TextMeshProUGUI[] initialAttackTexts;
    public Image[] chosenAttackImages;
    public Button[] chosenAttackButtons;
    public Button[] directionButtons;
    public TextMeshProUGUI[] chosenAttackTexts;
    public bool[] chosenAttackYDirection;
    public bool[] chosenAttackNegetiveDirection;
    public Button finishedButton;
    public GameObject spacePrompt;
    public bool inBench = false;
    public bool inRadius = false;
    private int selected = -1;

    //Initialises variables
    void Start()
    {
        //chosenAttacks = new int[initialAttacks.Length];
        chosenAttackYDirection = new bool[initialAttacks.Length];
        chosenAttackNegetiveDirection = new bool[initialAttacks.Length];
        for (int i = 0; i < chosenAttacks.Length; i++)
        {
            chosenAttacks[i] = -1;
            chosenAttackYDirection[i] = true;
        }
    }

    //Sets up screen when E is pressed
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !inBench && inRadius)
        {
            Time.timeScale = 0f;
            inBench = true;
            workbenchScreen.SetActive(true);
            selected = -1;
            for (int i = 0; i < initialAttackButtons.Length; i++)
            {
                initialAttackButtons[i].onClick.RemoveAllListeners();
                int num = i;
                initialAttackButtons[i].onClick.AddListener(() => Select(num));
                //initialAttackButtons[i].onClick.AddListener(delegate { Select(i); });
            }
            for (int i = 0; i < chosenAttackButtons.Length; i++)
            {
                chosenAttackButtons[i].onClick.RemoveAllListeners();
                int num = i;
                chosenAttackButtons[i].onClick.AddListener(() => Place(num));
                //chosenAttackButtons[i].onClick.AddListener(delegate { Place(i); });
                for (int j = 0; j < 4; j++)
                {
                    directionButtons[(i*4)+j].onClick.RemoveAllListeners();
                    int jnum = j;
                    directionButtons[(i*4)+j].onClick.AddListener(() => Direction(num, jnum % 2 == 0, jnum/2 == 1));
                    //directionButtons[(i*4)+j].onClick.AddListener(delegate { Direction(i, j % 2 == 0, j/2 == 1); });
                }
            }
            finishedButton.onClick.RemoveAllListeners();
            finishedButton.onClick.AddListener(() => ExitBench());
            //finishedButton.onClick.AddListener(ExitBench);
            UpdateDisplay();
        }
    }

    //Show prompt when in circle
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRadius = true;
            spacePrompt.SetActive(true);
        }
    }

    //Hide prompt when outside circle
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spacePrompt.SetActive(false);
            inRadius = false;
        }
    }

    //Updates changes in UI display
    public void UpdateDisplay()
    {
        for (int i = 0; i < initialAttackButtons.Length; i++)
        {
            if (i < initialAttacks.Length)
            {
                initialAttackButtons[i].gameObject.SetActive(true);
                initialAttackTexts[i].text = initialAttacks[i].ToString();
                bool prevSelected = false;
                for (int j = 0; j < chosenAttacks.Length; j++)
                {
                    if (chosenAttacks[j] == i)
                    {
                        prevSelected = true;
                        break;
                    }
                }
                if (selected == i)
                {
                    initialAttackImages[i].color = Color.cyan;
                }
                else if (prevSelected)
                {
                    initialAttackImages[i].color = Color.gray;
                }
                else
                {
                    initialAttackImages[i].color = Color.white;
                }
            }
            else
            {
                initialAttackButtons[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < chosenAttackButtons.Length; i++)
        {
            if (i < chosenAttacks.Length)
            {
                chosenAttackButtons[i].gameObject.SetActive(true);
                if (chosenAttacks[i] == -1)
                {
                    chosenAttackTexts[i].text = "None";
                    chosenAttackImages[i].color = Color.gray;
                }
                else
                {
                    chosenAttackTexts[i].text = initialAttacks[chosenAttacks[i]].ToString();
                    chosenAttackImages[i].color = Color.white;
                }
                for (int j = 0; j < 4; j++)
                {
                    directionButtons[(i * 4) + j].GetComponent<Image>().color = Color.gray;
                }
                directionButtons[(i * 4) + (chosenAttackYDirection[i] ? 0 : 1) + (chosenAttackNegetiveDirection[i] ? 2 : 0)].GetComponent<Image>().color = Color.white;
            }
            else
            {
                chosenAttackButtons[i].gameObject.SetActive(false);
            }
        }
    }

    //Selects attack to swap
    public void Select(int num)
    {
        /*bool prevSelected = false;
        for (int i = 0; i < chosenAttacks.Length; i++)
        {
            if (chosenAttacks[i] == num)
            {
                prevSelected = true;
                break;
            }
        }
        if (!prevSelected)
        {
            selected = num;
            UpdateDisplay();
        }*/
        if (selected == num)
        {
            selected = -1;
        }
        else
        {
            selected = num;
        }
        UpdateDisplay();
    }

    //Places/removes attack
    public void Place(int num)
    {
        if (selected == -1)
        {
            chosenAttacks[num] = -1;
        }
        else
        {
            chosenAttacks[num] = selected;
            selected = -1;
        }
        UpdateDisplay();
    }

    //Directional buttons
    public void Direction(int num, bool yDir, bool neg)
    {
        chosenAttackYDirection[num] = yDir;
        chosenAttackNegetiveDirection[num] = neg;
        UpdateDisplay();
    }

    //Leave screen
    public void ExitBench()
    {
        Time.timeScale = 1f;
        inBench = false;
        workbenchScreen.SetActive(false);
        AttackComponent ac = new Attack();
        for (int i = 0; i < chosenAttacks.Length; i++)
        {
            if (chosenAttacks[i] != -1)
            {
                ac = new AttackPattern(ac, initialAttacks[chosenAttacks[i]], chosenAttackYDirection[i], chosenAttackNegetiveDirection[i] ? -1 : 1);
            }
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().attack = ac;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health = 3;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().healthText.text = "Health: 3";
    }
}
