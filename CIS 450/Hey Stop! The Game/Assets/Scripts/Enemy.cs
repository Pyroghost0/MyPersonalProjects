/* Caleb Kahn
 * Enemy
 * Assignment 3 (Hard)
 * Observer enemies that move accross the screen and kill the player when hit
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Observer
{
    public Transform target;
    //public GameController gameController;
    //public float health = 1.7f;
    public Vector3 initPos;
    public float speed = 4f;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public bool followType = false;
    private bool left = false;
    private bool found = false;
    private bool move = true;

    //Sets up variables
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        target.GetComponent<Player>().AddObserver(this);
        initPos = transform.position;
    }

    //Sets enemy's velocity
    void Update()
    {
        if ((followType && move) || found)
        {
            rigidbody.velocity = ((target.position - transform.position).normalized) * speed;
        }
        else if (followType)
        {
            rigidbody.velocity = Vector2.zero;
        }
        else if (left)
        {
            rigidbody.velocity = new Vector2(-speed, 0f);
        }
        else
        {
            rigidbody.velocity = new Vector2(speed, 0f);
        }
    }

    //Sends enemy back to initial state
    public void Reset()
    {
        transform.position = initPos;
        move = true;
        found = false;
        left = Random.value < .5f;
    }

    //Changes the enemy's behaivior based on the subject's state
    public void Change(bool tileOn, bool moved)
    {
        if (followType)
        {
            move = tileOn || moved;
        }
        else
        {
            found = moved;
        }
    }
    
    //Turn back when hit a wall
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            left = transform.position.x > 0f ? true : false;
        }
    }

    //Take damage from bullet when in contact
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            health -= other.transform.localScale.magnitude;
            if (health <= 0f)
            {
                gameController.EnemyDead();
                Destroy(gameObject);
            }
        }
    }*/
}
