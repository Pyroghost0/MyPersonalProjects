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

    public Sprite[] bodySprites;
    public Sprite[] enemySprites;
    public GameObject bodyPrefab;
    public GameObject itemPrefab;

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

    //Take damage from contact
    //private void OnCollisionStay2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))// && collision.gameObject.GetComponent<Player>().attacking && !invincible)
        {
            Destroy(collision.gameObject);
            health--;
            if (health <= 0)
            {
                spawner.EnemyDeath();

                //Body
                Sprite enemySprite = GetComponent<SpriteRenderer>().sprite;
                if (enemySprite == enemySprites[0] || enemySprite == enemySprites[1] || enemySprite == enemySprites[2])
                {
                    Instantiate(bodyPrefab, transform.position + new Vector3(0f, .125f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[0];
                    Instantiate(bodyPrefab, transform.position + new Vector3(0f, -.2f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[1]);
                }
                else if (enemySprite == enemySprites[3] || enemySprite == enemySprites[4] || enemySprite == enemySprites[5])
                {
                    Instantiate(bodyPrefab, transform.position + new Vector3(.16f, 0f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[2];
                    Instantiate(bodyPrefab, transform.position + new Vector3(-.24f, 0f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[3]);
                }
                else if (enemySprite == enemySprites[6] || enemySprite == enemySprites[7] || enemySprite == enemySprites[8])
                {
                    Instantiate(bodyPrefab, transform.position + new Vector3(0f, -.13f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[4];
                    Instantiate(bodyPrefab, transform.position + new Vector3(0f, .25f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[5]);
                }
                else// if (enemySprite == enemySprites[9] || enemySprite == enemySprites[10] || enemySprite == enemySprites[11])
                {
                    Instantiate(bodyPrefab, transform.position + new Vector3(-.16f, 0f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[6];
                    Instantiate(bodyPrefab, transform.position + new Vector3(.24f, 0f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[7]);
                }

                //Item
                //Instantiate(itemPrefab, transform.position, transform.rotation);

                Destroy(gameObject);
                //gameObject.SetActive(false);
            }
            else
            {
                //StartCoroutine(IFrames());
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, 1f));// collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 3f));
            }
        }
        else if (collision.gameObject.CompareTag("Explosion"))
        {
            health = 0;
            spawner.EnemyDeath();

            //Body
            Sprite enemySprite = GetComponent<SpriteRenderer>().sprite;
            if (enemySprite == enemySprites[0] || enemySprite == enemySprites[1] || enemySprite == enemySprites[2])
            {
                Instantiate(bodyPrefab, transform.position + new Vector3(0f, .125f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[0];
                Instantiate(bodyPrefab, transform.position + new Vector3(0f, -.2f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[1]);
            }
            else if (enemySprite == enemySprites[3] || enemySprite == enemySprites[4] || enemySprite == enemySprites[5])
            {
                Instantiate(bodyPrefab, transform.position + new Vector3(.16f, 0f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[2];
                Instantiate(bodyPrefab, transform.position + new Vector3(-.24f, 0f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[3]);
            }
            else if (enemySprite == enemySprites[6] || enemySprite == enemySprites[7] || enemySprite == enemySprites[8])
            {
                Instantiate(bodyPrefab, transform.position + new Vector3(0f, -.13f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[4];
                Instantiate(bodyPrefab, transform.position + new Vector3(0f, .25f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[5]);
            }
            else// if (enemySprite == enemySprites[9] || enemySprite == enemySprites[10] || enemySprite == enemySprites[11])
            {
                Instantiate(bodyPrefab, transform.position + new Vector3(-.16f, 0f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[6];
                Instantiate(bodyPrefab, transform.position + new Vector3(.24f, 0f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[7]);
            }
            Destroy(gameObject);
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
