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
    public const float timeMultiplier = 3f;
    public GameObject bulletPrefab;
    public GameObject key;
    public float speed = 2.2f;
    public static int health = 5;
    public static int maxHealth = 5;
    public bool hasKey = false;
    public Animator animator;
    public static float bulletCooldownTime = .6f;
    public bool bulletCooldown = false;
    public bool invincible = false;
    public bool knockback = false;
    public SpriteRenderer spriteRenderer;
    public Collider2D solidCollider;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public static float[] gatherEfficiency = { 1f, 1f, 1f };

    //0-15 = inventory, 16 = day, 17 = upgrade stations, 18 = cracked walls, 19 = desk upgrades, 20 = health upgaredes, 21 = fire status, 22 = dugeonLevel (health reset to 100% due to fairity), 23 = volume settings
    public static ushort[] inventoryProgress = new ushort[24];
    //public Sprite[] itemSprites;
    //public Image inventorySprite;
    public static int select = 0;
    public GameObject bomb;
    public GameObject red;
    public GameObject yellow;
    public GameObject blue;
    public List<Transform> objectsInRadius = new List<Transform>();

    public static Player instance;
    //public static bool isPaused = false;
    public InteractableObject interaction;

    public static bool healed = false;
    public bool inHouse = false;
    public GameObject buildButton;
    public bool buildMode = false;
    public GameObject buildUI;

    public bool buttonClick = false;
    public TextMeshProUGUI[] itemTexts;
    public Image[] items;
    public Image healthImage;
    public Transform clock;
    public static float floatTime;
    public static ushort dayTime;
    public TextMeshProUGUI dayTimeText;

    public AudioSource gatherSound;
    public AudioSource chopSound;
    public AudioSource miningSound;

    void Start()
    {
        instance = this;
        Select(select);
        UpdateTime();
        UpdateItemAmount();
    }

    //Updates movement and attacks
    void Update()
    {
        //if (!isPaused)
        if (Time.timeScale == 1f)
        {
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 10f);

            //Inventory Selection
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Select(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Select(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Select(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Select(3);
            }

            if (inHouse)
            {
                if (!buildMode)
                {
                    //Movement and interact
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
            }
            else
            {
                //Time
                floatTime += Time.deltaTime * timeMultiplier;
                if (floatTime >= 1200f)
                {
                    floatTime = 1200f;
                    Debug.Log("Change");
                }
                UpdateTime();

                //Movement and interact
                if (!knockback)
                {
                    if (interaction == null)
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
                    }

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
                                if (best.GetComponent<Resource>() != null)
                                {
                                    animator.SetFloat("X", 0f);
                                    animator.SetFloat("Y", 0f);
                                    animator.SetInteger("Direction", Mathf.Abs(best.position.x - transform.position.x) > Mathf.Abs(best.position.y - transform.position.y) ? (int)Mathf.Sign(best.position.x - transform.position.x) * 2 : (int)Mathf.Sign(best.position.y - transform.position.y));
                                    animator.SetTrigger(best.GetComponent<Resource>().miningType.ToString());
                                    rigidbody.velocity = Vector2.zero;
                                }
                            }
                        }
                        else
                        {
                            interaction.Cancel();
                        }
                    }
                }

                //Shooting
                if (Input.GetKey(KeyCode.Mouse0) && !bulletCooldown && !buttonClick)
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

                //Throwing
                if (Input.GetKeyDown(KeyCode.Mouse1))// && inventory != ItemType.Empty)
                {
                    GameObject spawned = null;
                    if (select == 0 && inventoryProgress[3] > 0)
                    {
                        inventoryProgress[3]--;
                        spawned = red;
                        itemTexts[0].text = "x" + inventoryProgress[3];
                    }
                    else if (select == 1 && inventoryProgress[4] > 0)
                    {
                        inventoryProgress[4]--;
                        spawned = yellow;
                        itemTexts[1].text = "x" + inventoryProgress[4];
                    }
                    else if (select == 2 && inventoryProgress[5] > 0)
                    {
                        inventoryProgress[5]--;
                        spawned = blue;
                        itemTexts[2].text = "x" + inventoryProgress[5];
                    }
                    else if (select == 3 && inventoryProgress[2] > 0)
                    {
                        inventoryProgress[2]--;
                        spawned = bomb;
                        itemTexts[3].text = "x" + inventoryProgress[2];
                    }

                    if (spawned != null)
                    {
                        if (interaction != null)
                        {
                            interaction.Cancel();
                        }
                        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        worldPos.z = 0f;
                        Vector2 delta = worldPos - transform.position;
                        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                        GameObject thrownObject = Instantiate(spawned, transform.position, Quaternion.Euler(0f, 0f, angle - 90f));
                        thrownObject.GetComponent<ThrownObject>().time = (delta.magnitude + 1f) / 8f;
                        thrownObject.GetComponent<ThrownObject>().velocity = delta / thrownObject.GetComponent<ThrownObject>().time;
                        thrownObject.GetComponent<ThrownObject>().target = worldPos;
                    }
                    //inventory = ItemType.Empty;
                    //inventorySprite.sprite = itemSprites[0];
                }
            }
        }
    }

    public void UpdateItemAmount()
    {
        itemTexts[0].text = "x" + inventoryProgress[3];
        itemTexts[1].text = "x" + inventoryProgress[4];
        itemTexts[2].text = "x" + inventoryProgress[5];
        itemTexts[3].text = "x" + inventoryProgress[2];
    }

    public void UpdateTime()
    {
        dayTime = (ushort)floatTime;
        //dayTimeText.text = (dayTime % 720 / 60 == 0 ? 12 : dayTime % 720 / 60).ToString() + ":" + (dayTime % 60 / 10 == 0 ? "0" + dayTime % 60 : dayTime % 60) + (dayTime / 720 == 0 ? "AM" : "PM");
        dayTimeText.text = (dayTime % 720 / 60 == 0 ? 12 : dayTime % 720 / 60).ToString() + ":" + (dayTime % 60 / 15 == 0 ? "00" : dayTime % 60 / 15 * 15) + (dayTime / 720 == 0 ? "AM" : "PM");
        clock.rotation = Quaternion.Euler(0f, 0f, (floatTime-240f) / 4f);
    }

    /*public void ButtonSelect(int num)
    {
        Debug.Log(1);
        Select(num);
        StartCoroutine(ButtonCancel());
    }

    IEnumerator ButtonCancel()
    {
        Debug.Log(2);
        buttonClick = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        Debug.Log(buttonClick);
        buttonClick = false;
    }*/

    public void Select(int num)
    {
        select = num;
        for (int i = 0; i < 4; i++)
        {
            items[i].color = i == num ? Color.white : Color.grey;
        }
    }

    public void UpdateHealth(int difference)
    {
        health += difference;
        if (health <= 0){
            health = 0;
            Debug.Log("Died");
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthImage.fillAmount = ((float)health) / maxHealth;
        //Check possibility
        //update UI
    }

    //Cooldown fpr shooting the gun
    IEnumerator Cooldown()
    {
        bulletCooldown = true;
        yield return new WaitForSeconds(bulletCooldownTime);
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
            UpdateHealth(-1);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Slime") && collision.IsTouching(solidCollider) && !invincible)//!attacking
        {
            UpdateHealth(-1);
            //healthText.text = "Health: " + health;
            if (health > 0)
            {
                //StopAllCoroutines();
                StartCoroutine(IFrames());
            }
            else
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame(false);
            }
        }
    }

    //Knockback
    public IEnumerator Knockback(Vector3 dir, int power)
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

    public void BuildMode()
    {
        buildMode = !buildMode;
        buildUI.SetActive(buildMode);
        buildButton.SetActive(!buildMode);
        Time.timeScale = buildMode ? 0f : 1f;
        if (buildMode)
        {
            transform.position = new Vector3(-.5f, -.25f);
            animator.SetFloat("X", 0f);
            animator.SetFloat("Y", 0f);
            rigidbody.velocity = Vector2.zero;
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
