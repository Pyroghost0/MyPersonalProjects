/* Caleb Kahn
 * Enemy
 * Assignment 7 (Hard)
 * Controls enemy damage
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float health = 300;
    public int maxHealth = 300;
    public GameObject hitParticle;
    private float timer = 0f;
    public Transform floatingThing;
    //public GameObject tempBullet;
    public Image enemyHealthbar;
    public Image enemyHealthbarGrey;
    public Animator animator;
    //public AudioSource hitAudio;


    //Sets enemy's velocity
    void Update()
    {
        /*if (Random.value > .95f && Time.timeScale != 0f)
        {
            Quaternion rotation = Quaternion.Euler(0, 0f, - 180f);
            Instantiate(tempBullet, new Vector3(4.9f, -.65f, 0f), rotation).GetComponent<Rigidbody2D>().velocity = (new Vector2(0f, -1f)).normalized * 10f;
        }*/
        timer -= Time.deltaTime;//Negetive for oposite direction
        floatingThing.localPosition = new Vector2(Mathf.Cos(timer), Mathf.Sin(timer)) * 2.5f;
        floatingThing.transform.rotation = Quaternion.Euler(0f, 0f, timer * 150f);
        /*
        animator.SetFloat("X", rigidbody.velocity.normalized.x);
        animator.SetFloat("Y", rigidbody.velocity.normalized.y);
        */
    }

    public void Hit()
    {
        if (health > 0)
        {
            health--;
            enemyHealthbar.fillAmount = health / maxHealth;
            enemyHealthbarGrey.fillAmount = 1f - enemyHealthbar.fillAmount;
            if (health == 0)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();
                gameObject.SetActive(false);
            }
        }
    }

    //Take damage from contact
    //private void OnCollisionStay2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !collision.GetComponent<EnemyHitBullet>().hit)
        {
            //hitAudio.Play();
            collision.GetComponent<EnemyHitBullet>().hit = true;
            Instantiate(hitParticle, collision.transform.position, Quaternion.Euler(Mathf.Atan2(-collision.transform.localPosition.y, collision.transform.localPosition.x) * 57.2958f, 90f, -90f));
            Destroy(collision.gameObject);
            Hit();
        }
    }
}
