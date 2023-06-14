/* Caleb Kahn
 * MortarIngredient
 * Assignment 11 (Hard)
 * If crushed by pestle, it will turn into smaller versions of itself if applicable
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarIngredient : MonoBehaviour
{
    public Sprite originalSprite;
    public static Texture2D originalTexture;
    public SpriteRenderer spriteRenderer;
    //public SpriteMask spriteMask;
    //public static int orderNum = 0;
    public static int total = 0;
    public static int fullTotal = 0;
    public static int newestNum = 0;
    public int num = 0;
    public static int[,] assignedValues;
    private List<int> values = new List<int>();
    public PixelCollider2D pixelCollider;
    public int level = 0;
    public float damage = 0f;
    public float maxDamage = 100f;
    public GameObject ingredient;
    public GameObject powder;
    public static float startPixel;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody2D;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public AudioSource smashSound;

    void Start()
    {
        /*orderNum--;
        spriteRenderer.sortingOrder = orderNum;
        spriteMask.frontSortingOrder = orderNum;
        spriteMask.backSortingOrder = orderNum-1;*/
        if (level == 0)
        {
            total = 0;
            fullTotal = 0;
            newestNum = 0;
            originalTexture = originalSprite.texture;
            startPixel = originalTexture.width * -.0625f / 2f;
            assignedValues = new int[(int)originalSprite.rect.width, (int)originalSprite.rect.height];
            for (int i = 0; i < assignedValues.GetLength(0); i++)
            {
                for (int j = 0; j < assignedValues.GetLength(1); j++)
                {
                    assignedValues[i, j] = originalSprite.texture.GetPixel(i, j).a == 0 ? -1 : 0;
                    fullTotal += originalSprite.texture.GetPixel(i, j).a == 0 ? 0 : 1;
                }
            }
        }

        Texture2D texture = new Texture2D((int)(originalTexture.width), originalTexture.height, originalTexture.format, false);
        texture.filterMode = FilterMode.Point;
        Color[] colors = new Color[originalTexture.width * originalTexture.height];
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                if (assignedValues[i, j] == num)
                {
                    colors[(i * texture.width) + j] = originalTexture.GetPixel(i, j);
                    values.Add((i * texture.width) + j);
                }
            }
        }
        texture.SetPixels(colors);
        texture.Apply();
        Sprite sprite = Sprite.Create(texture,
               new Rect(0, 0, texture.width, texture.height),
               new Vector2(0.5f, 0.5f),
               16);
        spriteRenderer.sprite = sprite;
        pixelCollider.Regenerate();
        rigidbody2D.mass = values.Count * .1f;
        smashSound.volume = ((float)values.Count) / fullTotal;
        smashSound.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pestle"))
        {
            //Debug.Log(collision.relativeVelocity.magnitude);
            damage += collision.relativeVelocity.magnitude;
            if (damage >= maxDamage)
            {
                if (level < 4)
                {
                    int attemptAmount = (int)(values.Count * 1.5f / (5 - level));
                    for (int childNum = 0; childNum < 5 - level && values.Count > 0; childNum++)
                    {
                        List<int> attemptValues = new List<int>();
                        List<int> potentialValues = new List<int>();
                        Add(attemptValues, potentialValues, values[Random.Range(0, values.Count)]);
                        for (int attempt = 1; potentialValues.Count > 0 && attempt < attemptAmount; attempt++)
                        {
                            Add(attemptValues, potentialValues, potentialValues[Random.Range(0, potentialValues.Count)]);
                        }
                        if (attemptValues.Count > 3)
                        {
                            MortarIngredient child = Instantiate(ingredient, transform.position, transform.rotation).GetComponent<MortarIngredient>();
                            child.transform.parent = transform.parent;
                            child.level = level + 1;
                            child.maxDamage = maxDamage * .75f;
                            newestNum++;
                            child.num = newestNum;
                            foreach(int value in attemptValues)
                            {
                                assignedValues[value / originalTexture.width, value % originalTexture.width] = newestNum;
                            }
                        }
                        else
                        {
                            foreach (int value in attemptValues)
                            {
                                values.Add(value);
                            }
                        }
                    }
                }
                foreach (int value in values)
                {
                    total++;
                    if (total % 4 == 0)
                    {
                        Instantiate(powder, transform.position + (transform.up * (startPixel + (.0625f * (value / originalTexture.width)))) + (transform.right * (startPixel + (.0625f * (value % originalTexture.width)))), transform.rotation)
                            .GetComponent<Powder>().SetUp(total, originalTexture.GetPixel(value / originalTexture.width, value % originalTexture.height));
                    }
                }
                Destroy(gameObject);
            }
            else
            {
                smashSound.volume = Mathf.Min(Mathf.Pow((values.Count * rigidbody2D.velocity.magnitude) / fullTotal / 50f, .5f), .5f);
                smashSound.Play();
            }
        }
    }

    private void Add(List<int> attemptValues, List<int> potentialValues, int value)
    {
        attemptValues.Add(value);
        potentialValues.Remove(value);
        values.Remove(value);
        if (value / originalTexture.width != 0 && values.Contains(value - originalTexture.width))
        {
            potentialValues.Add(value - originalTexture.width);
        }
        if (value / originalTexture.width != originalTexture.width-1 && values.Contains(value + originalTexture.width))
        {
            potentialValues.Add(value + originalTexture.width);
        }
        if (value % originalTexture.width != 0 && values.Contains(value - 1))
        {
            potentialValues.Add(value - 1);
        }
        if (value % originalTexture.width != originalTexture.width - 1 && values.Contains(value + 1))
        {
            potentialValues.Add(value + 1);
        }
    }

    /*public float value = 1f;
    private float maxDamage = 35f;
    private GameObject[] children;
    public SpriteRenderer spriteRenderer;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    void Start()
    {
        if (value == -1)
        {
            GenerateValue();
        }
        rigidbody.mass = value;
        GetComponent<PixelCollider2D>().Regenerate();
    }

    public float GenerateValue()
    {
        children = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
        }
        if (children.Length == 0)
        {
            value = 1f;
            return value;
        }
        else
        {
            float sum = 0f;
            foreach (GameObject child in children)
            {
                sum += child.GetComponent<MortarIngredient>().GenerateValue();
            }
            value = sum;
            maxDamage = Mathf.Sqrt(sum) / children.Length;
            return value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Pestle") && children.Length > 0)
        {
            //Debug.Log(collision.relativeVelocity.magnitude);
            maxDamage -= collision.relativeVelocity.magnitude;
            if (maxDamage <= 0f)
            {
                tag = "Untagged";
                foreach (GameObject child in children)
                {
                    child.SetActive(true);
                    child.GetComponent<Rigidbody2D>().velocity = rigidbody.velocity;
                }
                rigidbody.simulated = false;
                GetComponent<Collider2D>().enabled = false;
                spriteRenderer.enabled = false;
            }
        }
    }*/
}
