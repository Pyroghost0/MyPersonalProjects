/* Caleb Kahn
 * PotIngredient
 * Assignment 11 (Hard)
 * slowly dissolves into the water
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotIngredient : MonoBehaviour
{
    public static float potRed = 1.25f;
    public static float potGreen = .5f;
    public static float potBlue = 1.25f;
    public float ingredientRed = 1f;
    public float ingredientGreen = 0f;
    public float ingredientBlue = 0f;
    private float size = 1f;
    public ingredient ingredientType;
    //private bool used = false;
    private bool inWater = false;
    private bool touchingWater = false;
    public Collider2D ingredientCollider;
    private Collider2D mouseCollider;
    private Collider2D potCollider;
    private bool holding = false;
    private SpriteRenderer waterRenderer;
    public SpriteRenderer ingredientRenderer;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public ParticleSystem particleSystem;
    private Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    // Start is called before the first frame update
    void Start()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        particleSystem.startColor = new Color(ingredientRed, ingredientGreen, ingredientBlue, .25f);
#pragma warning restore CS0618 // Type or member is obsolete
        mouseCollider = GameObject.FindGameObjectWithTag("Mouse").GetComponent<Collider2D>();
        potCollider = GameObject.FindGameObjectWithTag("Pot").GetComponent<Collider2D>();
        waterRenderer = GameObject.FindGameObjectWithTag("Pot Water").GetComponent<SpriteRenderer>();
        waterRenderer.color = potRed > potGreen && potRed > potBlue ? new Color(1f, potGreen / potRed, potBlue / potRed) : potGreen > potBlue ? new Color(potRed / potGreen, 1f, potBlue / potGreen) : new Color(potRed / potBlue, potGreen / potBlue, 1f);
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        StartCoroutine(Ingredient());
    }

    // Update is called once per frame
    /*void Update()
    {
        if (!used)
        {
            float temp = size;
            size -= rigidbody.velocity.magnitude * Time.deltaTime / 100f;
            if (size > 0f)
            {
                particleSystem.startColor = new Color(ingredientRed, ingredientGreen, ingredientBlue, size / 2f);
                transform.localScale = Vector3.one * size;
                rigidbody.mass = size;
                potRed += ingredientRed * rigidbody.velocity.magnitude * Time.deltaTime / 100f;
                potGreen += ingredientGreen * rigidbody.velocity.magnitude * Time.deltaTime / 100f;
                potBlue += ingredientBlue * rigidbody.velocity.magnitude * Time.deltaTime / 100f;
            }
            else
            {
                potRed += ingredientRed * temp;
                potGreen += ingredientGreen * temp;
                potBlue += ingredientBlue * temp;
                //Destroy(gameObject);
                particleSystem.Stop();
                ingredientRenderer.enabled = false;
                used = true;
                Debug.Log("Used");
            }
            waterRenderer.color = potRed > potGreen && potRed > potBlue ? new Color(1f, potGreen / potRed, potBlue / potRed) : potGreen > potBlue ? new Color(potRed / potGreen, 1f, potBlue / potGreen) : new Color(potRed / potBlue, potGreen / potBlue, 1f);
        }
    }*/

    void Update()
    {
        if (Time.timeScale == 1f && !inWater)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (ingredientCollider.IsTouching(mouseCollider))
                {
                    rigidbody.drag = 10f;
                    holding = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                rigidbody.drag = 4f;
                holding = false;
            }
        }
        else
        {
            rigidbody.drag = 4f;
            holding = false;
        }
    }

    IEnumerator Ingredient()
    {
        while (!inWater)
        {
            if (holding)
            {
                //Vector2 position = camera.ScreenToWorldPoint(Input.mousePosition);
                //mouse.position = position;
                if (!ingredientCollider.IsTouching(mouseCollider))
                {
                    rigidbody.AddForce((ingredientCollider.ClosestPoint(mouseCollider.transform.position) - ((Vector2)transform.position)) * 100f);
                }
            }
            else if (touchingWater && !ingredientCollider.IsTouching(potCollider)) {
                inWater = true;
            }
            yield return new WaitForFixedUpdate();
        }

        GameObject.FindGameObjectWithTag("Potion Maker").GetComponent<PotionMaker>().ingredientOrder.Add(ingredientType);
        ingredientRenderer.sortingOrder = 5;
        gameObject.layer = 6;
        float timer = 0f;
        while (timer < .5f)
        {
            transform.localScale = Vector3.one * (1.2f - (.4f * timer));
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        transform.localScale = Vector3.one;

        bool used = false;
        particleSystem.Play();
        while (!used)
        {
            rigidbody.AddForce((new Vector2(transform.position.y - waterRenderer.transform.position.y, waterRenderer.transform.position.x - transform.position.x).normalized *1.5f) + ((Vector2)(waterRenderer.transform.position - transform.position).normalized));
            float temp = size;
            float amount = Mathf.Clamp(rigidbody.velocity.magnitude - 2.5f, 0f, 20f) * Time.deltaTime / 60f;
            size -= amount;
            if (size > 0f)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                particleSystem.startColor = new Color(ingredientRed, ingredientGreen, ingredientBlue, size / 4f);
#pragma warning restore CS0618 // Type or member is obsolete
                transform.localScale = Vector3.one * size;
                rigidbody.mass = size;
                potRed += ingredientRed * amount;
                potGreen += ingredientGreen * amount;
                potBlue += ingredientBlue * amount;
            }
            else
            {
                potRed += ingredientRed * temp;
                potGreen += ingredientGreen * temp;
                potBlue += ingredientBlue * temp;
                //Destroy(gameObject);
                particleSystem.Stop();
                ingredientRenderer.enabled = false;
                used = true;
                //Debug.Log("Used");
            }
            waterRenderer.color = potRed > potGreen && potRed > potBlue ? new Color(1f, potGreen / potRed, potBlue / potRed) : potGreen > potBlue ? new Color(potRed / potGreen, 1f, potBlue / potGreen) : new Color(potRed / potBlue, potGreen / potBlue, 1f);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pot Water"))
        {
            touchingWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pot Water"))
        {
            touchingWater = false;
        }
    }
}
