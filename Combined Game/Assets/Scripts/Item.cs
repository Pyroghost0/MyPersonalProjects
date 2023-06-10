/* Caleb Kahn
 * Item
 * Assignment 6 (Hard)
 * Item that can be collected by player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Free = 0,
    Key = 1,
    Bomb = 2,
    Red = 3,
    Yellow = 4,
    Blue = 5,
    Wood = 6,
    Rock = 7,
    SeedSkinApple = 8,
    TreeHeart = 9,
    Sulfer = 10,
    Voidshroom = 11,
    IceFlower = 12,
    Orangeberry = 13,
    TearGem = 14,
    Charcoal = 15,
    Pickaxe
}

public class Item : MonoBehaviour
{
    //public ItemType itemType { get; set; }
    private ItemType type;
    public ItemType itemType { 
        get
        {
            return type;
        }
        set
        {
            spriteRenderer.sprite = itemSprites[(int)value];
            type = value;
        }
    }
    public Sprite[] itemSprites;
    public SpriteRenderer spriteRenderer;
    public float floatCycleTime = .8f;
    public float floatCycleDistence = .2f;
    private float curentYpos = 0f;
    private float timer = 0f;
    private bool pickedUp = false;

    void Start()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 10f);
    }

    //Floats
    void Update()
    {
        if (!pickedUp)
        {
            timer += Time.deltaTime;
            float distence = floatCycleDistence * -Mathf.Sin(timer / floatCycleTime * Mathf.PI);
            transform.position += new Vector3(0f, curentYpos - distence, 0f);
            curentYpos = distence;
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
                GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
                foreach (GameObject goal in goals)
                {
                    goal.transform.GetChild(0).gameObject.SetActive(true);
                }
                StartCoroutine(CollectionAnimation());
            }
            else// if (itemType == ItemType.Bomb)
            {
                //if (collision.GetComponent<Player>().inventory == ItemType.Empty)
                //{
                    pickedUp = true;
                    //collision.GetComponent<Player>().inventory = itemType;
                    //collision.GetComponent<Player>().inventorySprite.sprite = collision.GetComponent<Player>().itemSprites[(int)itemType];
                    StartCoroutine(CollectionAnimation());
                //}
            }
        }
    }

    //Goes to player and shrinks
    protected IEnumerator CollectionAnimation()
    {
        spriteRenderer.sortingOrder += 100;
        pickedUp = true;
        transform.SetParent(GameObject.FindGameObjectWithTag("Player").transform);
        Vector3 distance = transform.localPosition * -1.5f;
        float startScale = transform.localScale.x;
        timer = 0f;
        while (timer < .75f)
        {
            transform.localScale = Vector3.one * startScale * ((.75f - timer) / .75f);
            yield return new WaitForFixedUpdate();
            timer+=Time.deltaTime;
            transform.position += distance * Time.deltaTime;
        }
        if (itemType == ItemType.Pickaxe)
        {
            Player.inventoryProgress[18] += 16;
            Player.hasPickaxe = true;
            Destroy(gameObject);
        }
        else
        {
            Player.inventoryProgress[(int)itemType]++;
            if ((int)itemType <= 5)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpdateItemAmount();
            }
            else if ((int)itemType >= 2)
            {
                PlayerInventory.instance.UpdateItem((int)itemType);
            }
        }
        Destroy(gameObject);
    }
}
