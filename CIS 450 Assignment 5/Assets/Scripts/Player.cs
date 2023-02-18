/* Caleb Kahn
 * Player
 * Assignment 5 (Hard)
 * Player that moves acconding to controls
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed = 2.2f;
    public int money = 0;
    public int health = 5;
    public KeyType keyType = KeyType.None;
    public Animator animator;
    public bool attacking = false;
    public bool invincible = false;
    public bool knockback = false;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moneyText;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Updates movement and tilemap color
    void Update()
    {
        if (!attacking && !knockback)
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
                StartCoroutine(AttackBehaivior());
            }
        }
    }

    //Invincible for time
    IEnumerator IFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(3f);
        invincible = false;
    }

    //Determines if dead/won game or hit
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !attacking && !knockback)
        {
            if (!invincible)
            {
                health--;
                healthText.text = "Health: " + health;
                if (health > 0)
                {
                    StopAllCoroutines();
                    StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, 3));
                    StartCoroutine(IFrames());
                }
                else
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resultText.text = "You Died";
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();
                }
            }
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
    IEnumerator AttackBehaivior()
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
    }
}
