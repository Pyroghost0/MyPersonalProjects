/* Caleb Kahn
 * Enemy
 * Assignment 6 (Hard)
 * Observer enemies that move accross the screen and kill the player when hit
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyColor
{
    Red = 0,
    Yellow = 1,
    Blue = 2,
    Green = 3,
    Purple = 4,
    Orange = 5,
    All = 6
}

public class Enemy : MonoBehaviour
{
    public Transform target;
    //public int moneyAmount = 1;
    public int health = 1;
    public GameObject smoke;
    //public Vector3 initPos;
    public float speed = 3f;
    public float slowedSpeed = 1.5f;
    //public float followDistance = 10f;
    private bool slowed = false;
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
    public EnemyColor enemyColor;

    public 

    //Sets up variables
    void Start()
    {
        if (enemyColor == EnemyColor.Yellow)
        {
            animator.SetTrigger("Yellow");
        }
        else if (enemyColor == EnemyColor.Blue)
        {
            animator.SetTrigger("Blue");
        }
        target = GameObject.FindGameObjectWithTag("Player").transform;
        //initPos = transform.position;
    }

    //Sets enemy's velocity
    void Update()
    {
        if (!knockback)
        {
            //if ((target.position - transform.position).magnitude < followDistance)
            //{
                rigidbody.velocity = ((target.position - transform.position).normalized) * (slowed ? slowedSpeed : speed);
            /*}
            else
            {
                rigidbody.velocity = Vector2.zero;
            }*/
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
                Death();
            }
            else
            {
                //StartCoroutine(IFrames());
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, 1f));// collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 3f));
            }
        }
        else if (collision.gameObject.CompareTag("Explosion"))
        {
            if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Bomb)
            {
                health = health <= 2 ? 0 : health-2;
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, 2f));
            }
            else if  (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Red && enemyColor  == EnemyColor.Red)
            {
                health = 0;
            }
            else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Yellow && enemyColor == EnemyColor.Yellow)
            {
                health = 0;
            }
            else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Blue && enemyColor == EnemyColor.Blue)
            {
                health = 0;
            }

            if (health == 0)
            {
                if (spawner != null)
                {
                    spawner.EnemyDeath();
                }
                Destroy(gameObject);
            }
        }
    }

    public void Death()
    {
        if (spawner != null)
        {
            spawner.EnemyDeath();
        }

        //Body
        Sprite enemySprite = GetComponent<SpriteRenderer>().sprite;
        if (enemySprite == enemySprites[((int)enemyColor) * 12 + 0] || enemySprite == enemySprites[((int)enemyColor) * 12 + 1] || enemySprite == enemySprites[((int)enemyColor) * 12 + 2])
        {
            Instantiate(bodyPrefab, transform.position + new Vector3(0f, .125f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[((int)enemyColor) * 8 + 0];
            Instantiate(bodyPrefab, transform.position + new Vector3(0f, -.2f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[((int)enemyColor) * 8 + 1]);
        }
        else if (enemySprite == enemySprites[((int)enemyColor) * 12 + 3] || enemySprite == enemySprites[((int)enemyColor) * 12 + 4] || enemySprite == enemySprites[((int)enemyColor) * 12 + 5])
        {
            Instantiate(bodyPrefab, transform.position + new Vector3(.16f, 0f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[((int)enemyColor) * 8 + 2];
            Instantiate(bodyPrefab, transform.position + new Vector3(-.24f, 0f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[((int)enemyColor) * 8 + 3]);
        }
        else if (enemySprite == enemySprites[((int)enemyColor) * 12 + 6] || enemySprite == enemySprites[((int)enemyColor) * 12 + 7] || enemySprite == enemySprites[((int)enemyColor) * 12 + 8])
        {
            Instantiate(bodyPrefab, transform.position + new Vector3(0f, -.13f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[((int)enemyColor) * 8 + 4];
            Instantiate(bodyPrefab, transform.position + new Vector3(0f, .25f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[((int)enemyColor) * 8 + 5]);
        }
        else// if (enemySprite == enemySprites[((int)enemyColor)*12+9] || enemySprite == enemySprites[((int)enemyColor)*12+10] || enemySprite == enemySprites[((int)enemyColor)*12+11])
        {
            Instantiate(bodyPrefab, transform.position + new Vector3(-.16f, 0f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[((int)enemyColor) * 8 + 6];
            Instantiate(bodyPrefab, transform.position + new Vector3(.24f, 0f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[((int)enemyColor) * 8 + 7]);
        }
        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Slow Enemy"))
        {
            slowed = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Slow Enemy"))
        {
            slowed = false;
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
