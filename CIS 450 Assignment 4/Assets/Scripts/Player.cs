/* Caleb Kahn
 * Player
 * Assignment 4 (Hard)
 * Subject player that moves acconding to controls and changes tilemap color
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public AttackComponent attack;
    private bool canAttack = true;
    private bool invincible = false;
    private bool knockback = false;
    public int health = 3;
    public Transform checkpoint;
    public TextMeshProUGUI healthText;
    public Image cooldownImage;
    public Animator animator;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Initialises variables
    private void Start()
    {
        attack = new AttackPattern(new Attack(), AttackType.HeavyAttack, true);
    }

    //Updates movement
    void Update()
    {
        //Debug.Log(attack.attacking);
        if (!(attack != null && attack.inAction) && !knockback)
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
            rigidbody.velocity = new Vector2(x, y) * 2.2f;
            if (canAttack && Input.GetKeyDown(KeyCode.Space) && attack != null)
            {
                attack.StartAttack();
                StartCoroutine(Cooldown());
            }
        }
    }

    //Cooldown before attack is ready
    IEnumerator Cooldown()
    {
        canAttack = false;
        cooldownImage.fillAmount = 0f;
        yield return new WaitForSeconds(attack.totalDelay);
        float timer = 0f;
        while (timer < .75f)
        {
            cooldownImage.fillAmount = timer/.75f;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        cooldownImage.fillAmount = 1f;
        canAttack = true;
    }

    //Resets variables to scheckpoint
    public void Reset()
    {
        StopAllCoroutines();
        attack.Reset();
        rigidbody.velocity = Vector2.zero;
        canAttack = true;
        invincible = false;
        knockback = false;
        health = 3;
        healthText.text = "Health: " + health;
        transform.position = checkpoint.position;
        cooldownImage.fillAmount = 1f;
    }

    //Invincible for time
    IEnumerator IFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(2f);
        invincible = false;
    }

    //Monobehaivior for IEnumerators to activate
    public void InstantiateCoroutine(IEnumerator ienumerator)
    {
        StartCoroutine(ienumerator);
    }

    //Determines if dead/won game or hit
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !attack.attacking)
        {
            if (!invincible)
            {
                health--;
                healthText.text = "Health: " + health;
                if (health > 0)
                {
                    if (attack.inAction)
                    {
                        attack.Reset();
                        StopAllCoroutines();
                        cooldownImage.fillAmount = 1f;
                        canAttack = true;
                    }
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
        while (timer < power)
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
            rigidbody.velocity += new Vector2(x, y) * 1.1f;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime * (1f + (rigidbody.velocity.magnitude * rigidbody.velocity.magnitude / 4f));
        }
        knockback = false;
    }

    //Sets checkpoint
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            checkpoint = collision.transform;
        }
    }
}
