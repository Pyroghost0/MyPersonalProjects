/* Caleb Kahn
 * Player
 * Assignment 6 (Hard)
 * Player that moves acconding to controls and shoots
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject key;
    public float speed = 2.2f;
    //public int money = 0;
    //public int health = 5;
    public bool hasKey = false;
    public Animator animator;
    public bool bulletCooldown = false;
    public bool invincible = false;
    public bool knockback = false;
    //public TextMeshProUGUI healthText;
    //public TextMeshProUGUI moneyText;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Updates movement and tilemap color
    void Update()
    {
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
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(AttackBehaivior());
            }*/        
        }
        if (Input.GetKey(KeyCode.Mouse0) && !bulletCooldown)
        {
            Vector3 position = camera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0f;
            Quaternion rotation = Quaternion.Euler(0, 0f, Mathf.Atan2((position - transform.position).y, (position - transform.position).x) * 57.2958f - 90f);
            //Instantiate(bulletPrefab, transform.position, rotation);
            Instantiate(bulletPrefab, transform.position, rotation).GetComponent<Rigidbody2D>().velocity = (new Vector2( position.x - transform.position.x, position.y - transform.position.y)).normalized * 10f;
            //Bullet bullet = Instantiate(bulletPrefab, transform.position, rotation).GetComponent<Bullet>();
            //bullet.goal = position;
            StartCoroutine(Cooldown());
        }
    }

    //Cooldown fpr shooting the gun
    IEnumerator Cooldown()
    {
        bulletCooldown = true;
        yield return new WaitForSeconds(.4f);
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
            /*health--;
            healthText.text = "Health: " + health;
            if (health > 0)
            {
                //StopAllCoroutines();
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, 3));
                StartCoroutine(IFrames());
            }
            else
            {*/
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resultText.text = "You Died";
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();
            //}
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resultText.text = "You Win";
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();
        }
    }

    //Knockback
    IEnumerator Knockback(Vector3 dir, int power)
    {
        knockback = true;
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
