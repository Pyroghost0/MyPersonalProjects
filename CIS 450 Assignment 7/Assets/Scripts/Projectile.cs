/* Caleb Kahn
 * Bullet
 * Assignment 7 (Hard)
 * Destroys bullet on contact with walls
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public bool hitPlayer = false;
    public bool blackBullet = true;
    public bool destroyedFromExit = true;

    public void FollowPlayer(float amount, float time)
    {
        StartCoroutine(Following(amount, time));
    }

    IEnumerator Following(float amount, float time)
    {
        float initSpeed = rigidbody.velocity.magnitude;
        float timer = 0f;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        while (timer < time)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            float right = new Vector2(transform.position.x + rigidbody.velocity.y - player.position.x, transform.position.y - rigidbody.velocity.x - player.position.y).magnitude;
            float left = new Vector2(transform.position.x - rigidbody.velocity.y - player.position.x, transform.position.y + rigidbody.velocity.x - player.position.y).magnitude;
            rigidbody.velocity += left > right ? new Vector2(rigidbody.velocity.y, -rigidbody.velocity.x).normalized * amount * Time.deltaTime : new Vector2(-rigidbody.velocity.y, rigidbody.velocity.x).normalized * amount * Time.deltaTime;
            rigidbody.velocity = rigidbody.velocity.normalized * initSpeed;
            //Debug.Log(left + "\n" + right);
        }
    }

    public void Accelorate(Vector2 dir)
    {
        StartCoroutine(AccelorateCoroutine(dir));
    }

    IEnumerator AccelorateCoroutine(Vector2 dir)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            rigidbody.velocity += dir * Time.deltaTime;
        }
    }

    public void Turn(float amount)
    {
        StartCoroutine(TurnCoroutine(amount));
    }

    IEnumerator TurnCoroutine(float amount)
    {
        float rotation = Random.Range(0f, 360f);
        while (true)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            yield return new WaitForFixedUpdate();
            rotation += amount * Time.deltaTime;
        }
    }

    public void Look()
    {
        StartCoroutine(LookCoroutine());
    }

    IEnumerator LookCoroutine()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            transform.rotation = Quaternion.Euler(0, 0f, Mathf.Atan2(rigidbody.velocity.y, rigidbody.velocity.x) * 57.2958f - 90f);
        }
    }

    /*//Destroyed from contact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Door"))
        {
            Destroy(gameObject);
        }
    }*/

    //Destroyed from contact
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
