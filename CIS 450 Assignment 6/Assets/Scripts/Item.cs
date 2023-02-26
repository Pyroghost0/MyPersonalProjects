/* Caleb Kahn
 * Item
 * Assignment 6 (Hard)
 * Item that can be collected by player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum ItemType
{
    Empty = 0,
    Key = 1,
}*/

public class Item : SpawnableObject
{
    //public ItemType itemType;
    public float floatCycleTime = .8f;
    public float floatCycleDistence = .2f;
    private float curentYpos = 0f;
    private float timer = 0f;
    private bool doublePickUp = false;

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

    //Gives item to player on contact
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !doublePickUp)
        {
            doublePickUp = true;
            collision.GetComponent<Player>().hasKey = true;
            collision.GetComponent<Player>().key.SetActive(true);
            StartCoroutine(CollectionAnimation());
            /*if (itemType == ItemType.Key)
            {
                doublePickUp = true;
                collision.GetComponent<Player>().hasKey = true;
                StartCoroutine(CollectionAnimation());
                //Destroy(gameObject);
            }*/
        }
    }

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
