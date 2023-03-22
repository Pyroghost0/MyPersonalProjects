/* Caleb Kahn
 * Item
 * Assignment 7 (Hard)
 * Item that can be collected by player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Empty = 0,
    Key = 1,
    Bomb = 2,
    Pokeball = 3
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public float floatCycleTime = .8f;
    public float floatCycleDistence = .2f;
    private float curentYpos = 0f;
    private float timer = 0f;
    private bool pickedUp = false;

    //Floats
    void Update()
    {
        if (!pickedUp)
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
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !pickedUp)
        {
            if (itemType == ItemType.Key)
            {
                pickedUp = true;
                collision.GetComponent<Player>().key.SetActive(true);
                collision.GetComponent<Player>().hasKey = true;
                StartCoroutine(CollectionAnimation());
            }
            else
            {
                if (collision.GetComponent<Player>().inventory == ItemType.Empty)
                {
                    pickedUp = true;
                    collision.GetComponent<Player>().inventory = itemType;
                    collision.GetComponent<Player>().inventorySprite.sprite = collision.GetComponent<Player>().itemSprites[(int)itemType];
                    StartCoroutine(CollectionAnimation());
                }
            }
        }
    }

    //Goes to player and shrinks
    protected IEnumerator CollectionAnimation()
    {
        pickedUp = true;
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        Vector3 distance = transform.localPosition * -2f;
        float startScale = transform.localScale.x;
        timer = 0f;
        while (timer < .5f)
        {
            transform.localScale = Vector3.one * startScale * ((.5f - timer) / .5f);
            yield return new WaitForFixedUpdate();
            timer+=Time.deltaTime;
            transform.position += distance * Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
