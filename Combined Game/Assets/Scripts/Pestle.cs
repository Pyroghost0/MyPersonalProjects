/* Caleb Kahn
 * Pestle
 * Assignment 11 (Hard)
 * Pestle follows the mouse if held
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pestle : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public Collider2D pestleCollider;
    public Collider2D mouseCollider;
    public Transform mouse;
    private bool holding = false;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1f)
        {
            Vector2 position = camera.ScreenToWorldPoint(Input.mousePosition);
            mouse.position = position;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (pestleCollider.IsTouching(mouseCollider))
                {
                    holding = true;
                    rigidbody.gravityScale = 0f;
                }
            }
            else if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                holding = false;
                rigidbody.gravityScale = 1f;
            }

            //Movement
            if (holding)
            {
                if (pestleCollider.IsTouching(mouseCollider))
                {
                    rigidbody.drag = 50f;
                    /*Vector2 init = rigidbody.velocity.normalized;
                    rigidbody.AddForce(-rigidbody.velocity.normalized * 1000f * Time.deltaTime);
                    Debug.Log(rigidbody.velocity.normalized == init);
                    if ((rigidbody.velocity.normalized - init).magnitude > .1f)
                    {
                        Debug.Log("Zeroing");
                        rigidbody.velocity = Vector2.zero;
                    }*/
                }
                else
                {
                    rigidbody.drag = 10f;
                    rigidbody.AddForce((pestleCollider.ClosestPoint(position) - ((Vector2)transform.position)) * 300f);
                }
            }
            else
            {
                rigidbody.drag = 0f;
            }
        }
        else
        {
            holding = false;
            rigidbody.gravityScale = 1f;
        }
    }
}
