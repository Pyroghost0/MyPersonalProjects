/* Caleb Kahn
 * Body
 * Assignment 6 (Hard)
 * Body part that can be detrpyed by bullets
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public GameObject smokePrefab;
    public SpriteRenderer spriteRenderer;
    public EnemyColor enemyColor;
    public Sprite[] sprites;
    private bool doubleDeath = false;

    IEnumerator Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (spriteRenderer.sprite == sprites[i])
            {
                enemyColor = (EnemyColor)(i / 8);
            }
        }
        yield return new WaitForSeconds(60f);
        DestroyBody();
    }

    //Sets body to head settings
    public void SetHead(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        GetComponent<Rigidbody2D>().mass = .5f;
        GetComponent<CircleCollider2D>().radius = .25f;
    }

    //Destroys body when hit
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !doubleDeath)
        {
            Destroy(collision.gameObject);
            DestroyBody();
        }
        else if (collision.CompareTag("Explosion") && !doubleDeath)
        {
            if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Bomb)
            {
                doubleDeath = true;
                Destroy(gameObject);
            }
            else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Red && enemyColor == EnemyColor.Red)
            {
                doubleDeath = true;
                Destroy(gameObject);
            }
            else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Yellow && enemyColor == EnemyColor.Yellow)
            {
                doubleDeath = true;
                Destroy(gameObject);
            }
            else if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Blue && enemyColor == EnemyColor.Blue)
            {
                doubleDeath = true;
                Destroy(gameObject);
            }
        }
    }

    public void DestroyBody()
    {
        doubleDeath = true;
#pragma warning disable CS0618 // Type or member is obsolete
        Instantiate(smokePrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>().startColor = enemyColor == EnemyColor.Red ? new Color(0.2641509f, 0.1240628f, 0.05401868f) : enemyColor == EnemyColor.Yellow ? new Color(0.2627451f, 0.1336386f, 0.05490196f) : new Color(0.2175974f, 0.05490196f, 0.2627451f);
#pragma warning restore CS0618 // Type or member is obsolete
        Destroy(gameObject);
    }
}
