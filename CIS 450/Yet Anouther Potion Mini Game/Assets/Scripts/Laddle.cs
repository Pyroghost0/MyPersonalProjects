/* Caleb Kahn
 * Laddle
 * Assignment 11 (Hard)
 * Laddle follows the mouse if held
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laddle : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public Collider2D laddleCollider;
    public Collider2D mouseCollider;
    public Transform mouse;
    public Transform center;
    public Transform farPoint;
    public Transform laddleCenter;
    public Collider2D laddleCenterCollider;
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
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2((center.position - laddleCenter.position).y, (center.position - laddleCenter.position).x) * 57.2958f + 90f), Time.deltaTime * 20f);
            //Vector2 position = camera.ScreenToWorldPoint(Input.mousePosition);
            //mouse.position = position;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (laddleCollider.IsTouching(mouseCollider))
                {
                    holding = true;
                    laddleCenter.position = mouse.position;
                    laddleCenter.localPosition = new Vector2(0f, laddleCenter.localPosition.y);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                holding = false;
            }

            //Movement
            if (holding)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2((center.position - farPoint.position).y, (center.position - farPoint.position).x) * 57.2958f + 90f), Time.deltaTime * 50f);
                if (laddleCenterCollider.IsTouching(mouseCollider))
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
                    rigidbody.drag = 15f;
                    rigidbody.AddForce((laddleCenterCollider.ClosestPoint(mouse.position) - ((Vector2)laddleCenter.position)) * 1000f);
                }
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2((center.position - transform.position).y, (center.position - transform.position).x) * 57.2958f + 90f), Time.deltaTime * 3f);
            }
        }
        else
        {
            holding = false;
        }
    }
}
