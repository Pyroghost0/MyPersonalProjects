/* Caleb Kahn
 * Enemy
 * Assignment 6 (Hard)
 * Observer enemies that move accross the screen and kill the player when hit
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    //public int moneyAmount = 1;
    public int health = 1;
    public bool offColored;
    public GameObject smoke;
    //public Vector3 initPos;
    public float speed = 3f;
    public float followDistance = 10f;
    public bool knockback = false;
    //public bool invincible = false;
    public Spawner spawner;
    public Animator animator;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Sets up variables
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //initPos = transform.position;
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

    //Changes Spider to offset color
    public void SetOffset()
    {
        offColored = true;
        health = 1;
        int ran = Random.Range(0, 6);
        GetComponent<SpriteRenderer>().color = new Color(ran < 2 ? 7 / 8f : ran % 2 == 0 ? 3 / 4f : 1f, ran / 2 == 1 ? 7 / 8f : ran % 2 == 0 ? 3 / 4f : 1f, ran > 3 ? 7 / 8f : ran % 2 == 0 ? 3 / 4f : 1f);
        StartCoroutine(DestroyInTime());
    }

    //Destroys the offset spider in 4 seconds
    IEnumerator DestroyInTime()
    {
        yield return new WaitForSeconds(4f);
#pragma warning disable CS0618 // Type or member is obsolete
        Instantiate(smoke, transform.position, transform.rotation).GetComponent<ParticleSystem>().startColor = Color.green;
#pragma warning restore CS0618 // Type or member is obsolete
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        spawners[Random.Range(0, spawners.Length)].GetComponent<Spawner>().offsetSpawn = true;
        Destroy(gameObject);
    }

    //Take damage from contact
    //private void OnCollisionStay2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))// && collision.gameObject.GetComponent<Player>().attacking && !invincible)
        {
            Destroy(collision.gameObject);
            health --;
            if (health <= 0)
            {
                spawner.EnemyDeath();
                if (offColored)
                {
                    GameObject.FindGameObjectWithTag("Creator").GetComponent<ItemCreator>().SpawnObject(transform);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Creator").GetComponent<BodyCreator>().SpawnObject(transform);
                }
                gameObject.SetActive(false);
            }
            else
            {
                //StartCoroutine(IFrames());
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, 1f));// collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 3f));
            }
        }
    }


    //Invincible for time
    /*IEnumerator IFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(.4f);
        invincible = false;
    }*/

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
