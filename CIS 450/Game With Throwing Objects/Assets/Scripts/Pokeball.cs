/* Caleb Kahn
 * Pokeball
 * Assignment 7 (Hard)
 * Pokeball thrown from player that lands in a spot
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : ThrownObject
{
    public GameObject smokeParticle;

    //Captures enemy if nearby
    protected override IEnumerator ObjectEffect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        smokeParticle.SetActive(true);
        bool used = false;
        GameObject[] snorelaxes = GameObject.FindGameObjectsWithTag("Snorelax");
        if (snorelaxes.Length > 0)
        {
            GameObject closest = snorelaxes[0];
            for (int i = 1; i < snorelaxes.Length; i++)
            {
                if ((snorelaxes[i].transform.position - transform.position).magnitude < (closest.transform.position - transform.position).magnitude)
                {
                    closest = snorelaxes[i];
                }
            }
            //Debug.Log((closest.transform.position - transform.position).magnitude);
            if ((closest.transform.position - transform.position).magnitude < 1.5f)
            {
                used = true;
                StartCoroutine(HideSnorelax(closest.GetComponent<SpriteRenderer>()));
            }
        }
        if (!used)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 0)
            {
                GameObject closest = enemies[0];
                for (int i = 1; i < enemies.Length; i++)
                {
                    if ((enemies[i].transform.position - transform.position).magnitude < (closest.transform.position - transform.position).magnitude)
                    {
                        closest = enemies[i];
                    }
                }
                Debug.Log((closest.transform.position - transform.position).magnitude);
                if ((closest.transform.position - transform.position).magnitude < 1f)
                {
                    StartCoroutine(TransformEnemy(closest));
                }
            }
        }
        yield return new WaitForSeconds(1.5f);
        //explosionParticle.SetActive(false);
        Destroy(gameObject);
    }

    //Hides the Snorelax slowly
    IEnumerator HideSnorelax(SpriteRenderer sprite)
    {
        sprite.GetComponent<Collider2D>().enabled = false;
        float timer = 0f;
        while (timer < 1f)
        {
            sprite.color = new Color(1f, 1f, 1f, 1f - timer);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        Destroy(sprite.gameObject);
    }

    //Transforms enemy to ally
    IEnumerator TransformEnemy(GameObject enemy)
    {
        enemy.tag = "Ally";
        enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        enemy.GetComponent<SpriteRenderer>().color = new Color(.5f, 1f, 1f);
        enemy.GetComponent<Enemy>().knockback = true;
        enemy.GetComponent<Enemy>().captured = true;
        enemy.GetComponent<Enemy>().target = null;
        yield return new WaitForSeconds(1f);
        enemy.GetComponent<Enemy>().knockback = false;
    }
}
