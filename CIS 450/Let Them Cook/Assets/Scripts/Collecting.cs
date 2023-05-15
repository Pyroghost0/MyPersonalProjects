/* Caleb Kahn
 * Collecting
 * Assignment 9 (Hard)
 * Minion state that tries to collect resources
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collecting : MinionState
{
    public Minion minion;
    public Transform target;
    private Rigidbody2D rb;

    public void Start()
    {
        minion = gameObject.GetComponent<Minion>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void StartCollecting()
    {
        StartCoroutine(CollectCoroutine());
    }

    public IEnumerator CollectCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        while (minion.currentState == this)
        {
            if (target == null)
            {
                GameObject[] resources = GameObject.FindGameObjectsWithTag("Resource");
                GameObject[] minions = GameObject.FindGameObjectsWithTag("Minion");
                Transform closest = null;
                for (int i = 0; i < resources.Length; i++)
                {
                    bool notUsed = true;
                    foreach(GameObject otherTarget in minions)
                    {
                        if (otherTarget.GetComponent<Collecting>().target == resources[i].transform)
                        {
                            notUsed = false;
                            break;
                        }
                    }
                    if (notUsed)
                    {
                        if (closest == null || (transform.position - resources[i].transform.position).magnitude < (transform.position - closest.position).magnitude)
                        {
                            closest = resources[i].transform;
                        }
                    }
                }
                if (closest != null)
                {
                    target = closest;
                    rb.velocity = (target.transform.position - transform.position).normalized * minion.speed;
                }
            }
            else
            {
                rb.velocity = (target.transform.position - transform.position).normalized * minion.speed;
            }
            yield return new WaitForFixedUpdate();
        }
        target = null;
    }

    public override void Return()
    {
        minion.Return();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Resource") && !minion.hasResource)
        {
            minion.hasResource = true;
            Destroy(collision.gameObject);
            Return();
        }
    }
}
