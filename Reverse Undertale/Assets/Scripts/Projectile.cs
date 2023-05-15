/* Caleb Kahn
 * Projectile
 * Assignment 7 (Hard)
 * Projectile that can follow the player and other functions if nessisary
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
    public bool grow = false;
    public bool destroyedFromExit = true;
    private bool following = false;
    //private bool currentlyFollowing = false;

    void Start()
    {
        if (grow)
        {
            StartCoroutine(Grow());
        }
    }

    public void Truck()
    {
        StartCoroutine(TruckCoroutine());
    }

    IEnumerator TruckCoroutine()
    {
        //Truck Size = 2.45098
        //Sprite Size = 2.45098
        //Truck Distance = 8.7      | 9.85
        //Real Distance = 6.25      | 7.4
        SpriteRenderer backTruck = transform.GetChild(0).GetComponent<SpriteRenderer>();
        SpriteRenderer frontTruck = transform.GetChild(1).GetComponent<SpriteRenderer>();
        float growTime = 2.45098f / Mathf.Abs(rigidbody.velocity.x);
        float endingTime = 9.85f / Mathf.Abs(rigidbody.velocity.x);
        if (transform.position.x > 4.9f)
        {
            //transform.localScale = Vector3.one;
            //backTruck.transform.localScale = Vector3.one;
            //frontTruck.transform.localScale = Vector3.one;
        //}
        //else
        //{
            transform.localScale = new Vector3(-1f, 1f, 1f);
            //backTruck.transform.localScale = new Vector3(-1f, 1f, 1f);
            //frontTruck.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        backTruck.gameObject.SetActive(false);
        float timer = 0f;
        while (timer < growTime)
        {
            frontTruck.size = new Vector2(timer * 2.45098f / growTime, frontTruck.size.y);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        frontTruck.size = new Vector2(frontTruck.size.y, frontTruck.size.y);

        yield return new WaitForSeconds(endingTime - (growTime * 2f));

        frontTruck.gameObject.SetActive(false);
        backTruck.gameObject.SetActive(true);
        timer = growTime;
        while (timer > 0f)
        {
            backTruck.size = new Vector2(timer * 2.45098f / growTime, backTruck.size.y);
            yield return new WaitForFixedUpdate();
            timer -= Time.deltaTime;
        }
        Destroy(gameObject);
    }

    IEnumerator Grow()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            transform.localScale = Vector3.one * timer;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            //Debug.Log(left + "\n" + right);
        }
        transform.localScale = Vector3.one;
    }

    public void FollowPlayer(float amount, float time)
    {
        StartCoroutine(Following(amount, time));
    }

    IEnumerator Following(float amount, float time)
    {
        following = amount > 0f;
        //currentlyFollowing = amount > 0f;
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
        StartCoroutine(DestroyInTime(15f));
        //currentlyFollowing = false;
    }

    IEnumerator DestroyInTime(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Failed To Destroy Projectile");
        Destroy(gameObject);
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
        if (collision.gameObject.CompareTag("Wall"))// && !following)
        {
            if (destroyedFromExit && (!following || (transform.position.y< -4f && transform.position.x > -9f && rigidbody.velocity.y < 0f)))
            {
                Destroy(gameObject);
            }
            else
            {
                destroyedFromExit = true;
            }
        }
    }
}
