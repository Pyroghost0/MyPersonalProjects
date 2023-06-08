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
    private bool holding = false;
    public Collider2D pestleCollider;
    public Collider2D mouseCollider;
    public Collider2D holdCollider;
    public Transform mouse;
    public Transform hold;
    public Transform lookPos;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1f)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2((center.position - laddleCenter.position).y, (center.position - laddleCenter.position).x) * 57.2958f + 90f), Time.deltaTime * 20f);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (pestleCollider.IsTouching(mouseCollider))
                {
                    rigidbody.gravityScale = 0f;
                    holding = true;
                    hold.position = mouse.position;
                    hold.localPosition = new Vector2(0f, hold.localPosition.y);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                holding = false;
            }

            //Movement
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2(lookPos.position.y - transform.position.y, lookPos.position.x - transform.position.x) * 57.2958f + 90f), Time.deltaTime * 10f);
            if (holding)
            {
                //rigidbody.angularVelocity += Time.deltaTime;// (Mathf.Atan2(lookPos.position.y - transform.position.y, lookPos.position.x - transform.position.x) * 57.2958f + 90f) * Time.deltaTime * 10f;
                if (holdCollider.IsTouching(mouseCollider))
                {
                    rigidbody.drag = 75f;
                }
                else
                {
                    rigidbody.drag = 15f;
                    rigidbody.AddForce((holdCollider.ClosestPoint(mouse.position) - ((Vector2)hold.position)) * 1500f);
                }
            }
            else
            {
                rigidbody.gravityScale = 10f;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2((center.position - transform.position).y, (center.position - transform.position).x) * 57.2958f + 90f), Time.deltaTime * 3f);
            }
        }
        else
        {
            rigidbody.gravityScale = 10f;
            holding = false;
        }
    }
}
