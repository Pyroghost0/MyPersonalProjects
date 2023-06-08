using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powder : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Coroutine dustCoroutine;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody2D;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public void SetUp(int order, Color color)
    {
        spriteRenderer.sortingOrder = order;
        spriteRenderer.color = color;
        //StartCoroutine(PowderCoroutine());
    }

    /*IEnumerator PowderCoroutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (rigidbody2D.velocity.magnitude < .5f)
            {
                if (rigidbody2D.IsTouchingLayers(13))
                {
                    //rigidbody2D.bodyType = RigidbodyType2D.Static;
                    rigidbody2D.velocity = Vector2.zero;
                    rigidbody2D.Sleep();
                    gameObject.layer = 13;
                    yield break;
                }
            }
        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer >= 14)
        {
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            gameObject.layer = 14;
        }
    }
}
