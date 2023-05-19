/* Caleb Kahn
 * Player
 * Assignment 6 (Hard)
 * Player that moves acconding to controls and shoots
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject key;
    public float speed = 2.2f;
    public int health = 5;
    public bool hasKey = false;
    public Animator animator;
    public bool bulletCooldown = false;
    public bool invincible = false;
    public bool knockback = false;
    public SpriteRenderer spriteRenderer;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public static float[] gatherEfficiency = { 1f, 1f, 1f };

    public static short[] inventoryProgress = new short[20];
    //public Sprite[] itemSprites;
    //public Image inventorySprite;
    public GameObject bomb;
    public List<Transform> objectsInRadius = new List<Transform>();

    public static Player instance;
    public bool isPaused = false;
    public InteractableObject interaction;

    void Start()
    {
        instance = this;
    }

    //Updates movement and attacks
    void Update()
    {
        if (!isPaused)
        {
            //Movement and interact
            spriteRenderer.sortingOrder = -(int)transform.position.y * 10;
            if (!knockback)
            {
                float x = Input.GetAxis("Horizontal");
                float y = Input.GetAxis("Vertical");
                animator.SetFloat("X", x);
                animator.SetFloat("Y", y);
                float magnitude = Mathf.Sqrt(x * x + y * y);
                if (magnitude != 0)
                {
                    x *= Mathf.Abs(x) / magnitude;
                    y *= Mathf.Abs(y) / magnitude;
                }
                rigidbody.velocity = new Vector2(x, y) * speed;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (interaction == null)
                    {
                        if (objectsInRadius.Count > 0)
                        {
                            Transform best = objectsInRadius[0];
                            for (int i = 1; i < objectsInRadius.Count; i++)
                            {
                                if ((objectsInRadius[i].position - transform.position).magnitude < (best.position - transform.position).magnitude)
                                {
                                    best = objectsInRadius[i];
                                }
                            }
                            interaction = best.GetComponent<InteractableObject>();
                            interaction.Interact();
                        }
                    }
                    else
                    {
                        interaction.Cancel();
                    }
                }
            }

            //Shooting
            if (Input.GetKey(KeyCode.Mouse0) && !bulletCooldown)
            {
                if (interaction != null)
                {
                    interaction.Cancel();
                }
                Vector3 position = camera.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0f;
                Quaternion rotation = Quaternion.Euler(0, 0f, Mathf.Atan2((position - transform.position).y, (position - transform.position).x) * 57.2958f - 90f);
                //Instantiate(bulletPrefab, transform.position, rotation);
                Instantiate(bulletPrefab, transform.position, rotation).GetComponent<Rigidbody2D>().velocity = (new Vector2(position.x - transform.position.x, position.y - transform.position.y)).normalized * 10f;
                //Bullet bullet = Instantiate(bulletPrefab, transform.position, rotation).GetComponent<Bullet>();
                //bullet.goal = position;
                StartCoroutine(Cooldown());
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))// && inventory != ItemType.Empty)
            {
                if (interaction != null)
                {
                    interaction.Cancel();
                }
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPos.z = 0f;
                Vector2 delta = worldPos - transform.position;
                float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                GameObject thrownObject = Instantiate(bomb, transform.position, Quaternion.Euler(0f, 0f, angle - 90f));
                thrownObject.GetComponent<ThrownObject>().time = (delta.magnitude + 1f) / 8f;
                thrownObject.GetComponent<ThrownObject>().velocity = delta / thrownObject.GetComponent<ThrownObject>().time;
                thrownObject.GetComponent<ThrownObject>().target = worldPos;

                //inventory = ItemType.Empty;
                //inventorySprite.sprite = itemSprites[0];
            }
        }
    }

    //Cooldown fpr shooting the gun
    IEnumerator Cooldown()
    {
        bulletCooldown = true;
        yield return new WaitForSeconds(.6f);
        bulletCooldown = false;
    }

    //Invincible for time
    IEnumerator IFrames()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        invincible = true;
        for (int i = 0; i < 6; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, i % 2 == 0 ? .5f : .75f);
            yield return new WaitForSeconds(.5f);
        }
        spriteRenderer.color = Color.white;
        invincible = false;
    }

    //Determines if dead/won game or hit
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !knockback && !invincible)//!attacking
        {
            health--;
            //healthText.text = "Health: " + health;
            if (health > 0)
            {
                //StopAllCoroutines();
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, 3));
                StartCoroutine(IFrames());
            }
            else
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame(false);
            }
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame(true);
        }
    }

    //Knockback
    IEnumerator Knockback(Vector3 dir, int power)
    {
        knockback = true;
        if (interaction != null)
        {
            interaction.Cancel();
        }
        float timer = 0f;
        dir *= power;
        while (timer < power / 2f)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            animator.SetFloat("X", x);
            animator.SetFloat("Y", y);
            float magnitude = Mathf.Sqrt(x * x + y * y);
            if (magnitude != 0)
            {
                x *= Mathf.Abs(x) / magnitude;
                y *= Mathf.Abs(y) / magnitude;
            }
            rigidbody.velocity = dir * (1f - (timer / power));
            rigidbody.velocity += new Vector2(x, y) * .9f;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime * (1f + (rigidbody.velocity.magnitude * rigidbody.velocity.magnitude / 4f));
        }
        knockback = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableObject"))
        {
            objectsInRadius.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableObject"))
        {
            objectsInRadius.Remove(collision.transform);
        }
    }

    //Attacks with a charge
    /*IEnumerator AttackBehaivior()
    {
        attacking = true;
        yield return new WaitForFixedUpdate();
        Rigidbody2D rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        Animator animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        Vector2 dir;
        if (rigidbody.velocity == Vector2.zero)
        {
            dir = new Vector2(0f, 1f);
        }
        else
        {
            dir = rigidbody.velocity.normalized;
        }
        float timer = 0f;
        while (timer < .75f)
        {
            rigidbody.velocity = dir * 5f * (1f - (timer / .75f));
            animator.SetFloat("X", rigidbody.velocity.x);
            animator.SetFloat("Y", rigidbody.velocity.y);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        attacking = false;
        knockback = true;
        yield return new WaitForSeconds(.25f);
        knockback = false;
    }*/
}
