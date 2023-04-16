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
    public float value = 1f;
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
    }
}
