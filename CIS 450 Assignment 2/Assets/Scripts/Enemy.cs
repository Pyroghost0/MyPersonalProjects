/* Caleb Kahn
 * Enemy
 * Assignment 2 (Hard)
 * Enemies that take damage and move accross the screen
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public GameController gameController;
    public Rigidbody rigidbody;
    public float health = 1.7f;//2 normal bullets

    //Calls Move()
    void Start()
    {
        StartCoroutine(Move());
    }

    //Sets enemy's velocity and destroys itself when hit target
    IEnumerator Move()
    {
        rigidbody.velocity = ((target.position - transform.position).normalized) * 3f;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (target.position.x < 0f ? transform.position.x < target.position.x : transform.position.x > target.position.x)
            {
                Destroy(gameObject);
            }
        }
    }

    //Take damage from bullet when in contact
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (other.name == "Straight Bullet")
            {
                Destroy(other.gameObject);
            }
            health -= other.transform.localScale.magnitude;
            if (health <= 0f)
            {
                gameController.EnemyDead();
                Destroy(gameObject);
            }
        }
    }
}
