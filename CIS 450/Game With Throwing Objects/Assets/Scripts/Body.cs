/* Caleb Kahn
 * Body
 * Assignment 7 (Hard)
 * Body part that can be detrpyed by bullets
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public GameObject smokePrefab;
    private bool doubleDeath = false;

    //Sets body to head settings
    public void SetHead(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
        GetComponent<Rigidbody2D>().mass = .5f;
        GetComponent<CircleCollider2D>().radius = .25f;
    }

    //Destroys body when hit
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !doubleDeath)
        {
            doubleDeath = true;
            Instantiate(smokePrefab, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
