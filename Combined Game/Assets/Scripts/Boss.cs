using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Animator animator;
    public GameObject[] shields;
    private EnemyColor[] shieldColors;
    public SpriteRenderer[] shieldSprites;
    private bool invincible = true;
    private EnemyColor[] enemyColors;
    private int phase = -1;
    //private Coroutine phaseCoroutine;

    public GameObject[] enemies;
    public GameObject[] pots;
    private GameObject[] spawnedPots;
    public Transform[] enemySpawnLocations;
    public Transform[] potSpawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        Player.inventoryProgress[2] = 5;
        Player.inventoryProgress[3] = 5;
        Player.inventoryProgress[4] = 5;
        Player.inventoryProgress[5] = 5;

        spawnedPots = new GameObject[potSpawnLocations.Length];
        StartCoroutine(Phase());
    }

    IEnumerator Phase()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        animator.SetBool("Up", true);
        yield return new WaitForSeconds(.75f);
        phase++;
        int[] spawnable = phase == 6 ? new[] { 0, 1, 2, 3, 4, 5 } : phase >= 3 ? new[] { (phase + 1) % 3, (phase + 2) % 3, phase } : new[] { phase, (phase+1)%3+3, (phase + 1) % 3 + 3, (phase + 2) % 3 + 3 };
        int[] colors = phase == 6 ? new[] { 0, 1, 2 } : phase >= 3 ? new[] { (phase + 1) % 3, (phase + 2) % 3 } : new[] { phase };
        shieldColors = new EnemyColor[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            animator.SetTrigger("Shield");
            yield return new WaitForSeconds(5f/6f);
            shields[i].SetActive(true);
            shieldColors[i] = (EnemyColor) colors[i];
            shieldSprites[i].color = colors[i] == 0 ? Color.red : colors[i] == 1 ? Color.yellow : Color.blue;
            yield return new WaitForSeconds(1f/6f);
        }
        invincible = false;
        float chance = 0f;
        foreach (Transform spawnLocation in enemySpawnLocations)
        {
            chance += (phase/3 + 1) / 3f;
            if (Random.value <= chance)
            {
                chance = 0;
                Instantiate(enemies[spawnable[Random.Range(0, spawnable.Length)]], spawnLocation.position, transform.rotation);
            }
        }
        chance = 0f;
        while (!invincible)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            if (Random.value <= chance)
            {
                chance = 0;
                int potNum = Random.Range(0, potSpawnLocations.Length);
                if (spawnedPots[potNum] == null && Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, potSpawnLocations[potNum].position) < 5f)
                {
                    spawnedPots[potNum] = Instantiate(pots[colors[Random.Range(0, colors.Length)]], potSpawnLocations[potNum].position, transform.rotation);
                }
            }
            else
            {
                Instantiate(enemies[spawnable[Random.Range(0, spawnable.Length)]], enemySpawnLocations[Random.Range(0, enemySpawnLocations.Length)].position, transform.rotation);
            }
            chance += (3-(phase / 3)) / 6f;
        }
        if (phase == 6)
        {
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            Debug.Log("You Win");
        }
        else
        {
            StartCoroutine(Phase());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible)
        {
            if (collision.gameObject.CompareTag("Bullet"))// && collision.gameObject.GetComponent<Player>().attacking && !invincible)
            {
                Destroy(collision.gameObject);
                if (!shields[0].activeSelf && !shields[1].activeSelf && !shields[2].activeSelf)
                {
                    invincible = true;
                    animator.SetBool("Up", false);
                }
            }
            else if (collision.gameObject.CompareTag("Explosion"))
            {
                if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Bomb)
                {
                    if (!shields[0].activeSelf && !shields[1].activeSelf && !shields[2].activeSelf)
                    {
                        invincible = true;
                        animator.SetBool("Up", false);
                    }
                }
                else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Red && ((shields[0].activeSelf && shieldColors[0] == EnemyColor.Red) || (shields[1].activeSelf && shieldColors[1] == EnemyColor.Red) || (shields[2].activeSelf && shieldColors[2] == EnemyColor.Red)))
                {
                    if (shieldColors[0] == EnemyColor.Red)
                    {
                        shields[0].SetActive(false);
                    }
                    else if (shieldColors[1] == EnemyColor.Red)
                    {
                        shields[1].SetActive(false);
                    }
                    else
                    {
                        shields[2].SetActive(false);
                    }
                }
                else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Yellow && ((shields[0].activeSelf && shieldColors[0] == EnemyColor.Yellow) || (shields[1].activeSelf && shieldColors[1] == EnemyColor.Yellow) || (shields[2].activeSelf && shieldColors[2] == EnemyColor.Yellow)))
                {
                    if (shieldColors[0] == EnemyColor.Yellow)
                    {
                        shields[0].SetActive(false);
                    }
                    else if (shieldColors[1] == EnemyColor.Yellow)
                    {
                        shields[1].SetActive(false);
                    }
                    else
                    {
                        shields[2].SetActive(false);
                    }
                }
                else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Blue && ((shields[0].activeSelf && shieldColors[0] == EnemyColor.Blue) || (shields[1].activeSelf && shieldColors[1] == EnemyColor.Blue) || (shields[2].activeSelf && shieldColors[2] == EnemyColor.Blue)))
                {
                    if (shieldColors[0] == EnemyColor.Blue)
                    {
                        shields[0].SetActive(false);
                    }
                    else if (shieldColors[1] == EnemyColor.Blue)
                    {
                        shields[1].SetActive(false);
                    }
                    else
                    {
                        shields[2].SetActive(false);
                    }
                }
            }
        }
    }
}
