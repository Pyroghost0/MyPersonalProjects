/* Caleb Kahn
 * Player
 * Assignment 9 (Hard)
 * Player that moves acconding to controls and shoots
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float speed = 2.2f;
    public bool hasKey = false;
    public Animator animator;
    public bool bulletCooldown = false;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Updates movement and attacks
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        animator.SetFloat("X", x);
        animator.SetFloat("Y", y);
        float magnitude = Mathf.Sqrt(x * x + y * y);
        if (magnitude != 0)
        {
            x *= Mathf.Abs(x) / magnitude;
            y *= Mathf.Abs(y) / magnitude;
        }
        rigidbody.velocity = new Vector2(x, y) * speed;
        if (Input.GetKey(KeyCode.Mouse0) && !bulletCooldown)
        {
            Vector3 position = camera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0f;
            Quaternion rotation = Quaternion.Euler(0, 0f, Mathf.Atan2((position - transform.position).y, (position - transform.position).x) * 57.2958f - 90f);
            //Instantiate(bulletPrefab, transform.position, rotation);
            Instantiate(bulletPrefab, transform.position, rotation).GetComponent<Rigidbody2D>().velocity = (new Vector2( position.x - transform.position.x, position.y - transform.position.y)).normalized * 20f;
            //Bullet bullet = Instantiate(bulletPrefab, transform.position, rotation).GetComponent<Bullet>();
            //bullet.goal = position;
            StartCoroutine(Cooldown());
        }
    }

    //Cooldown fpr shooting the gun
    IEnumerator Cooldown()
    {
        bulletCooldown = true;
        yield return new WaitForSeconds(.4f);
        bulletCooldown = false;
    }
}
