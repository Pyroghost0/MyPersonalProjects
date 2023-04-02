/* Caleb Kahn
 * Enemy
 * Assignment 9 (Hard)
 * Observer enemies that move accross the screen and kill the minions when hit
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public int health = 1;
    public GameObject smoke;
    public float speed = 3f;
    public bool knockback = false;
    public Spawner spawner;
    public Animator animator;
    private GameObject shotBy;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Sets enemy's velocity
    void Update()
    {
        if (!knockback)
        {
            if (target == null)
            {
                GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
                if (minions.Length > 0)
                {
                    Transform closest = minions[0].transform;
                    for (int i = 1; i < minions.Length; i++)
                    {
                        if ((minions[i].transform.position - transform.position).magnitude < (closest.position - transform.position).magnitude)
                        {
                            closest = minions[i].transform;
                        }
                    }
                    target = closest;
                }
                else
                {
                    Debug.Log("Can't find minion");
                    rigidbody.velocity = Vector2.zero;
                }
            }
            else
            {
                rigidbody.velocity = ((target.position - transform.position).normalized) * speed;
            }
            animator.SetFloat("X", rigidbody.velocity.normalized.x);
            animator.SetFloat("Y", rigidbody.velocity.normalized.y);
        }
    }

    //Take damage from contact
    //private void OnCollisionStay2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (shotBy != collision.gameObject)
            {
                shotBy = collision.gameObject;
                Destroy(collision.gameObject);
                health--;
                if (health <= 0)
                {
                    spawner.EnemyDeath();
                    Instantiate(smoke, transform.position, transform.rotation);
                    Destroy(gameObject);
                    //gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, 1f));// collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 3f));
                }
            }
        }
    }

    //Knockback
    IEnumerator Knockback(Vector3 dir, float power)
    {
        animator.SetFloat("X", 0f);
        animator.SetFloat("Y", 0f);
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
