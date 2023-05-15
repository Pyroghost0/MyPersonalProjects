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
    public AudioSource powerupSound;
    public AudioSource winSound;

    //Sets enemy's velocity
    void Update()
    {
        /*if (Random.value > .95f && Time.timeScale != 0f)
        {
            Quaternion rotation = Quaternion.Euler(0, 0f, - 180f);
            Instantiate(tempBullet, new Vector3(4.9f, -.65f, 0f), rotation).GetComponent<Rigidbody2D>().velocity = (new Vector2(0f, -1f)).normalized * 10f;
        }*/
        timer -= Time.deltaTime;//Negetive for oposite direction
        floatingThing.localPosition = new Vector2(Mathf.Cos(timer), Mathf.Sin(timer)) * 3f;
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
            if (health == 0 && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health > 0)
            {
                StartCoroutine(Death());
            }
        }
    }

    IEnumerator Death()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Dead");
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.canPause = false;
        float soundTimer = 0f;
        float volume = gc.sceneBGM.GetComponent<AudioSource>().volume;
        //Other Sounds
        while (soundTimer < .5f)
        {
            gc.sceneBGM.GetComponent<AudioSource>().volume = volume * ((.625f - soundTimer) / .625f);
            yield return new WaitForFixedUpdate();
            soundTimer += Time.deltaTime;
        }
        winSound.Play();
        gc.sceneBGM.GetComponent<AudioSource>().volume = volume / 4f;
        yield return new WaitForSeconds(4.5f);
        soundTimer = 0f;
        while (soundTimer < .5f)
        {
            gc.sceneBGM.GetComponent<AudioSource>().volume = volume * ((.125f + soundTimer) / .625f);
            yield return new WaitForFixedUpdate();
            soundTimer += Time.deltaTime;
        }
        gc.sceneBGM.GetComponent<AudioSource>().volume = volume;
        gc.EndGame(true);
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
