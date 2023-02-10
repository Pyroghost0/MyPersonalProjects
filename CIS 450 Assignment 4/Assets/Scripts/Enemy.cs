/* Caleb Kahn
 * Enemy
 * Assignment 4 (Hard)
 * Observer enemies that move accross the screen and kill the player when hit
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public int health = 10;
    public Vector3 initPos;
    public float speed = 4f;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    private int initHealth;
    private bool knockback = false;

    //Sets up variables
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initPos = transform.position;
        initHealth = health;
    }

    //Sets enemy's velocity
    void Update()
    {
        if (!knockback)
        {
            if ((transform.position - target.position).magnitude < 10f)
            {
                rigidbody.velocity = ((target.position - transform.position).normalized) * speed;
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
        }
    }

    //Sends enemy back to initial state
    public void Reset()
    {
        transform.position = initPos;
        health = initHealth;
        gameObject.SetActive(true);
        StopAllCoroutines();
    }

    //Take damage from contact
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().attack.attacking)
        {
            health -= 5;
            if (health <= 0f)
            {
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, collision.gameObject.GetComponent<Player>().attack.activeAttackPower));
            }
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
            rigidbody.velocity = dir * (1f -(timer / power));
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime * (1f + (rigidbody.velocity.magnitude * rigidbody.velocity.magnitude / 4f));
        }
        knockback = false;
    }
}
