/* Caleb Kahn
 * Seed
 * Assignment 11 (Hard)
 * Seed follows mouse if held
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public SpriteRenderer spriteRenderer;
    public Collider2D seedCollider;
    public Collider2D mouseCollider;
    private bool holding = false;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void SetRed()
    {
        spriteRenderer.color = new Color(Random.Range(.8f, 1f), Random.Range(0f, .2f), Random.Range(0f, .2f));
    }

    public void SetBrown()
    {
        spriteRenderer.color = new Color(Random.Range(.8f, 1f), Random.Range(.35f, .55f), Random.Range(0f, .1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Vector2 position = camera.ScreenToWorldPoint(Input.mousePosition);
                //mouse.position = position;
                if (seedCollider.IsTouching(mouseCollider))
                {
                    holding = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                holding = false;
            }

            //Movement
            if (holding)
            {
                Vector2 position = camera.ScreenToWorldPoint(Input.mousePosition);
                //mouse.position = position;
                if (!seedCollider.IsTouching(mouseCollider))
                {
                    rigidbody.AddForce((seedCollider.ClosestPoint(position) - ((Vector2)transform.position)) * 30f);
                }
            }
        }
        else
        {
            holding = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Apple"))
        {
            Destroy(gameObject);
        }
    }
}
