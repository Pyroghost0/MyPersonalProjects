/* Caleb Kahn
 * Enemy
 * Assignment 5 (Hard)
 * Observer enemies that move accross the screen and kill the player when hit
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public int moneyAmount = 1;
    public int health = 1;
    public Vector3 initPos;
    public float speed = 3f;
    public float followDistance = 10f;
    public bool knockback = false;
    public bool invincible = false;
    public Spawner spawner;
    public Animator animator;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Sets up variables
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initPos = transform.position;
    }

    //Sets enemy's velocity
    void Update()
    {
        if (!knockback)
        {
            if ((target.position - transform.position).magnitude < followDistance)
            {
                rigidbody.velocity = ((target.position - transform.position).normalized) * speed;
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
            animator.SetFloat("X", rigidbody.velocity.normalized.x);
            animator.SetFloat("Y", rigidbody.velocity.normalized.y);
        }
    }

    //Take damage from contact
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<Player>().attacking && !invincible)
        {
            health --;
            if (health <= 0)
            {
                spawner.EnemyDeath();
                GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemCreator>().SpawnItem(false, transform.position, moneyAmount);
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(IFrames());
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 3f));
            }
        }
    }


    //Invincible for time
    IEnumerator IFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(.4f);
        invincible = false;
    }

    //Knockback
    IEnumerator Knockback(Vector3 dir, float power)
    {
        knockback = true;
        float timer = 0f;
        dir *= power;
        while (timer < power)
        {
            rigidbody.velocity = dir * (1f - (timer / power));
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime * (1f + (rigidbody.velocity.magnitude * rigidbody.velocity.magnitude / 4f));
        }
        knockback = false;
    }
}
