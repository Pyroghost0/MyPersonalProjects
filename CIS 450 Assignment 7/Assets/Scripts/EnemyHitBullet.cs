/* Caleb Kahn
 * EnemyHitBullet
 * Assignment 7 (Hard)
 * Bullet that spawns to hit enemy if player hits black bullet
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBullet : MonoBehaviour
{
    private float timer = 0f;
    public bool hit = false;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    private void Start()
    {
        rigidbody.angularVelocity = 20f;
        rigidbody.velocity = new Vector2(transform.localPosition.y, -transform.localPosition.x).normalized * 8f;
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            transform.localScale = new Vector3(timer, timer, timer);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        transform.localScale = Vector3.one;
    }

    // Update is called once per frame
   void Update()
    {
        timer += Time.deltaTime;
        rigidbody.velocity += new Vector2(transform.localPosition.x, transform.localPosition.y).normalized * (-Time.deltaTime * timer * 36f);
        transform.rotation = Quaternion.Euler(0f, 0f, timer * 250f);
        //rigidbody.velocity += new Vector2(transform.localPosition.x, transform.localPosition.y) * -Time.deltaTime;
        //rigidbody.SetRotation(Quaternion.LookRotation(rigidbody.velocity));
        //GetComponent<Rigidbody2D>().MoveRotation(60f * Time.deltaTime);
        //transform.rotation = Quaternion.LookRotation(rigidbody.velocity);


        /*timer -= Time.deltaTime;//Negetive for oposite direction
        transform.localPosition = new Vector2(Mathf.Cos(timer), Mathf.Sin(timer)) * (1f + timer);*/
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }*/
}
