using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject[] shields;
    private EnemyColor[] shieldColors;
    public SpriteRenderer[] shieldSprites;
    private bool invincible = true;
    private EnemyColor[] enemyColors;
    private int phase = -1;
    public Transform player;
    //private Coroutine phaseCoroutine;

    public GameObject[] enemies;
    public GameObject[] pots;
    private GameObject[] spawnedPots;
    public Transform[] enemySpawnLocations;
    public Transform[] potSpawnLocations;
    private GameObject[] spawnedPots2;
    public Transform[] enemySpawnLocations2;
    public Transform[] potSpawnLocations2;
    public GameObject destroyedParticle;
    public GameObject[] bossLocations;

    public AudioSource hitSound;
    public AudioSource shieldUpSound;
    public AudioSource shieldDownSound;

    // Start is called before the first frame update
    void Start()
    {
        //Player.inventoryProgress[2] = 5;
        //Player.inventoryProgress[3] = 5;
        //Player.inventoryProgress[4] = 5;
        //Player.inventoryProgress[5] = 5;

        foreach (GameObject location in bossLocations)
        {
            location.GetComponent<SpriteRenderer>().sortingOrder = -(int)((location.transform.position.y * 10f)+1);
        }
        spawnedPots = new GameObject[potSpawnLocations.Length];
        spawnedPots2 = new GameObject[potSpawnLocations2.Length];
        StartCoroutine(Phase());
    }

    IEnumerator Phase()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        animator.SetBool("Up", true);
        yield return new WaitForSeconds(.75f);
        phase++;
        Debug.Log(phase);
        int[] spawnable = phase == 6 ? new[] { 0, 1, 2, 3, 4, 5 } : phase >= 3 ? new[] { (phase + 1) % 3, (phase + 2) % 3, phase } : new[] { phase, (phase+1)%3+3, (phase + 1) % 3 + 3, (phase + 2) % 3 + 3 };
        int[] colors = phase == 6 ? new[] { 0, 1, 2 } : phase >= 3 ? new[] { (phase + 1) % 3, (phase + 2) % 3 } : new[] { phase };
        shieldColors = new EnemyColor[colors.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            animator.SetTrigger("Shield");
            yield return new WaitForSeconds(2f/6f);
            shieldUpSound.Play();
            yield return new WaitForSeconds(3f/6f);
            shields[i].SetActive(true);
            shieldColors[i] = (EnemyColor) colors[i];
            shieldSprites[i].color = colors[i] == 0 ? Color.red : colors[i] == 1 ? Color.yellow : Color.blue;
            yield return new WaitForSeconds(1f/6f);
        }
        invincible = false;
        float chance = 0f;
        foreach (Transform spawnLocation in phase <= 2 ? enemySpawnLocations : enemySpawnLocations2)
        {
            chance += (phase/3 + 1) / 6f;
            if (Random.value <= chance)
            {
                chance = 0;
                Instantiate(enemies[spawnable[Random.Range(0, spawnable.Length)]], spawnLocation.position, transform.rotation);
            }
        }
        chance = 0f;
        while (!invincible)
        {
            yield return new WaitForSeconds(Random.Range(Mathf.Sqrt((8 - phase)*3f), Mathf.Sqrt((8 - phase)*3.5f)));
            if (Random.value <= chance)
            {
                chance = 0;
                if (phase <= 2)
                {
                    int potNum = Random.Range(0, potSpawnLocations.Length);
                    if (spawnedPots[potNum] == null && Vector2.Distance(player.position, potSpawnLocations[potNum].position) > 5f)
                    {
                        spawnedPots[potNum] = Instantiate(pots[colors[Random.Range(0, colors.Length)]], potSpawnLocations[potNum].position, transform.rotation);
                    }
                }
                else
                {
                    int potNum = Random.Range(0, potSpawnLocations2.Length);
                    if (spawnedPots2[potNum] == null && Vector2.Distance(player.position, potSpawnLocations2[potNum].position) > 5f)
                    {
                        spawnedPots2[potNum] = Instantiate(pots[colors[Random.Range(0, colors.Length)]], potSpawnLocations2[potNum].position, transform.rotation);
                    }
                }
            }
            else
            {
                do
                {
                    int pos = Random.Range(0, phase <= 2 ? enemySpawnLocations.Length : enemySpawnLocations2.Length);
                    if (((phase <= 2 ? enemySpawnLocations[pos].position : enemySpawnLocations2[pos].position) - player.position).magnitude > 1.5f)
                    {
                        Instantiate(enemies[spawnable[Random.Range(0, spawnable.Length)]], phase <= 2 ? enemySpawnLocations[pos].position : enemySpawnLocations2[pos].position, transform.rotation);
                        break;
                    }
                } while (true);
                
            }
            chance += (3-(phase / 3)) / 15f;
        }
        if (phase < 5)
        {
            StartCoroutine(Phase());
        }
    }

    IEnumerator Hit()
    {
        invincible = true;
        animator.SetBool("Up", false);
        hitSound.Play();
        yield return new WaitForSeconds(1f);
        Instantiate(destroyedParticle, transform.position, transform.rotation);
        Destroy(bossLocations[phase]);
        if (phase == 5)
        {
            Destroy(spriteRenderer);
            GetComponent<Collider2D>().enabled = false;
            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Win();
        }
        else
        {
            transform.position = bossLocations[phase + 1].transform.position;
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 10f);
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
                    StartCoroutine(Hit());
                }
            }
            else if (collision.gameObject.CompareTag("Explosion"))
            {
                if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Bomb)
                {
                    if (!shields[0].activeSelf && !shields[1].activeSelf && !shields[2].activeSelf)
                    {
                        StartCoroutine(Hit());
                    }
                }
                else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Red && ((shields[0].activeSelf && shieldColors[0] == EnemyColor.Red) || (shields[1].activeSelf && shieldColors[1] == EnemyColor.Red) || (shields[2].activeSelf && shieldColors[2] == EnemyColor.Red)))
                {
                    shieldDownSound.Play();
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
                    shieldDownSound.Play();
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
                    shieldDownSound.Play();
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
