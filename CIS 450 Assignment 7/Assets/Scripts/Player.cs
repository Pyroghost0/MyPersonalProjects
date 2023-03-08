/* Caleb Kahn
 * Player
 * Assignment 7 (Hard)
 * Player that moves acconding to controls takes damage
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Transform enemy;
    public Transform enemyFloatThing;
    public GameObject EnemyHitBulletPrefab;
    public GameObject key;
    private float speed = 4f;
    public Image[] hearts;
    private int health = 5;
    public bool canMove = true;
    //public Animator animator;
    public bool invincible = false;
    //public TextMeshProUGUI healthText;
    //public TextMeshProUGUI moneyText;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public AudioSource hitAudio;
    //public AudioSource goodHitAudio;

    void Start()
    {
        health = hearts.Length;
    }

    //Updates movement
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resultText.text = "You Died";
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();
        }
        if (canMove)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            float magnitude = Mathf.Sqrt(x * x + y * y);
            if (magnitude != 0)
            {
                x *= Mathf.Abs(x) / magnitude;
                y *= Mathf.Abs(y) / magnitude;
            }
            rigidbody.velocity = new Vector2(x, y) * speed;
        }
    }

    //Invincible for time
    IEnumerator IFrames()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        invincible = true;
        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, i % 2 == 0 ? .5f : .75f);
            yield return new WaitForSeconds(.25f);
        }
        spriteRenderer.color = Color.white;
        invincible = false;
    }
    
    IEnumerator Heart(Image heart)
    {
        float timer = 0f;
        float red = 1f;
        float green = 1f;
        float blue = 1f;
        while (timer < 3f)
        {
            red -= Random.value < .5f ? Time.deltaTime : 0f;
            green -= Random.value < .5f ? Time.deltaTime : 0f;
            blue -= Random.value < .5f ? Time.deltaTime : 0f;
            if (red < 0f)
            {
                red = 0f;
            }
            if (green < 0f)
            {
                green = 0f;
            }
            if (blue < 0f)
            {
                blue = 0f;
            }
            heart.color = new Color(red, green, blue, (3f - timer) / 3f);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        heart.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile") && !collision.GetComponent<Projectile>().hitPlayer)//!attacking
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            if (projectile.blackBullet)
            {
                //goodHitAudio.Play();
                Instantiate(EnemyHitBulletPrefab, enemyFloatThing.position, transform.rotation).transform.parent = enemy;
            }
            else if (!invincible)
            {
                health--;
                hitAudio.Play();
                //healthText.text = "Health: " + health;
                if (health > 0)
                {
                    //StopAllCoroutines();
                    StartCoroutine(IFrames());
                    StartCoroutine(Heart(hearts[health]));
                }
                else
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resultText.text = "You Died";
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();
                }
            }
            projectile.hitPlayer = true;
            Destroy(projectile.gameObject);
        }
    }

}
