using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public ingredient ingredient;
    public SpriteRenderer spriteRenderer;
    private Coroutine dustCoroutine;
    private Vector2 previousVelosity;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody2D;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public void SetUp(int order, Color color, Bottle bottle)
    {
        spriteRenderer.sortingOrder = order;
        spriteRenderer.color = color;
        ingredient = bottle.ingredient;
        rigidbody2D.velocity = new Vector2((bottle.rigidbody.velocity.x / 4f) + Mathf.Pow(Random.Range(-1f, 1f), 2), Random.Range(-2f, 0f));
    }

    // Start is called before the first frame update
    void Start()
    {
        dustCoroutine = StartCoroutine(DustCoroutine());
    }

    IEnumerator DustCoroutine()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            previousVelosity = rigidbody2D.velocity;
            if (transform.position.y < -5f)
            {
                Debug.Log("Destroyying Dust");
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Stop Dust") && gameObject.layer != 0)
        {
            //Debug.Log(rigidbody2D.velocity.magnitude);
            //Debug.Log(previousVelosity.magnitude);
            if (previousVelosity.magnitude < 2f)
            {
                //Debug.Log(transform.position);
                //transform.position += (Vector3)(rigidbody2D.velocity.normalized * collision.GetContact(0).separation);
                //transform.position = collision.GetContact(0).point;
                transform.position = collision.GetContact(0).point - (previousVelosity.normalized * .03125f);
                //Debug.Log(transform.position);
                StopCoroutine(dustCoroutine);
                rigidbody2D.bodyType = RigidbodyType2D.Static;
                gameObject.layer = 0;
                tag = "Stop Dust";
            }
        }
    }
}
