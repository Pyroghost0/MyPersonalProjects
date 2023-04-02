/* Caleb Kahn
 * Returning
 * Assignment 9 (Hard)
 * Moves back to house
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Returning : MinionState
{
    public Minion minion;
    private Rigidbody2D rb;

    public void Start()
    {
        minion = gameObject.GetComponent<Minion>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void StartCollecting()
    {
        minion.Collect();
    }

    public override void Return()
    {
        StartCoroutine(ReturnCoroutine());
    }

    public IEnumerator ReturnCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        Vector3 target = (Vector3) GameObject.FindGameObjectWithTag("Home").GetComponent<Collider2D>().ClosestPoint(transform.position);
        while (minion.currentState == this)
        {
            rb.velocity = (target - transform.position).normalized * minion.speed;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Resource") && !minion.hasResource)
        {
            minion.hasResource = true;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Home"))
        {
            if (minion.hasResource)
            {
                minion.hasResource = false;
                collision.GetComponent<House>().AddPhase();
            }
            StartCollecting();
        }
    }
}
