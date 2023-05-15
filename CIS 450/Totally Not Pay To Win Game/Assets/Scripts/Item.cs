/* Caleb Kahn
 * Item
 * Assignment 5 (Hard)
 * Abstract item that can be collected by player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public float floatCycleTime = .8f;
    public float floatCycleDistence = .2f;
    protected float curentYpos = 0f;
    protected float timer = 0f;
    protected bool doublePickUp = false;

    //Floats
    void Update()
    {
        if (!doublePickUp)
        {
            timer += Time.deltaTime;
            float distence = floatCycleDistence * -Mathf.Sin(timer / floatCycleTime * Mathf.PI);
            transform.position += new Vector3(0f, curentYpos - distence, 0f);
            curentYpos = distence;
            if (timer > floatCycleTime * Mathf.PI * 2f)
            {
                timer -= floatCycleTime * Mathf.PI * 2f;
            }
        }
    }

    protected abstract void OnTriggerStay2D(Collider2D collision);

    //Goes to player and shrinks
    protected IEnumerator CollectionAnimation()
    {
        doublePickUp = true;
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        Vector3 distance = transform.localPosition * -2f;
        timer = 0f;
        while (timer < .5f)
        {
            transform.localScale = Vector3.one * ((.5f - timer) / .5f);
            yield return new WaitForFixedUpdate();
            timer+=Time.deltaTime;
            transform.position += distance * Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
