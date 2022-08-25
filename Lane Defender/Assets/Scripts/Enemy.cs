using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int numLives = 3;
    public int addedScore = 1000;
    public float speed = 2f;
    public float hitTime = .25f;
    private bool hit = false;
    private float timer = 0f;
    public Animator animator;
    public GameObject explosion;

    // Update is called once per frame
    void Update()
    {
        if (hit)
        {
            timer += Time.deltaTime;
            if (timer > hitTime)
            {
                hit = false;
                timer = 0f;
            }
        }
        else
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
            if (transform.position.x < -9f)
            {
                GameObject.FindGameObjectWithTag("Spawn").GetComponent<EnemySpawn>().TakeDamage();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<TankMove>().ExplodeMethod(Instantiate(explosion, collision.transform.position, transform.rotation));
            Destroy(collision.gameObject);
            numLives--;
            hit = true;
            if (numLives > 0)
            {
                animator.SetTrigger("Hit");
                GameObject.FindGameObjectWithTag("Player").GetComponent<TankMove>().hit.Play();
            }
            else
            {
                GameObject.FindGameObjectWithTag("Spawn").GetComponent<EnemySpawn>().UpdateScore(addedScore);
                animator.SetTrigger("Dead");
                GameObject.FindGameObjectWithTag("Player").GetComponent<TankMove>().death.Play();
                StartCoroutine(DelayDeath());
            }
        }
        else if (collision.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Spawn").GetComponent<EnemySpawn>().TakeDamage();
            Destroy(gameObject);
        }
    }

    IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(hitTime);
        Destroy(gameObject);
    }
}
