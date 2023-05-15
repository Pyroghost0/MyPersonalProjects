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
    public Image[] easyHearts;
    public int health = 5;
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
    public AudioSource deathAudio;
    //public AudioSource goodHitAudio;

    void Start()
    {
        if (!GameController.openMainMenu)
        {
            if (GameController.easyMode)
            {
                hearts = easyHearts;
            }
            hearts[0].transform.parent.gameObject.SetActive(true);
            health = hearts.Length;
        }
    }

    //Updates movement
    void Update()
    {
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
        for (int i = 0; i < 12; i++)
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

    IEnumerator Death()
    {
        deathAudio.Play();
        float timer = 0f;
        float red = 1f;
        float green = 1f;
        float blue = 1f;
        SpriteRenderer heart = GetComponent<SpriteRenderer>();
        while (timer < 3f)
        {
            transform.localScale = Vector3.one * ((3f - timer) / 3f);
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
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame(false);
        gameObject.SetActive(false);
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
                invincible = true;
                hitAudio.Play();
                StartCoroutine(Heart(hearts[health]));
                //healthText.text = "Health: " + health;
                if (health > 0)
                {
                    //StopAllCoroutines();
                    StartCoroutine(IFrames());
                }
                else
                {
                    StartCoroutine(Death());
                }
            }
            projectile.hitPlayer = true;
            Destroy(projectile.gameObject);
        }
    }

}
