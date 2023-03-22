/* Caleb Kahn
 * Bomb
 * Assignment 7 (Hard)
 * Bomb thrown from player that lands in a spot
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ThrownObject
{
    public GameObject explosionParticle;
    public GameObject explosionCollider;

    //Explodes
    protected override IEnumerator ObjectEffect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        explosionParticle.SetActive(true);
        explosionCollider.SetActive(true);
        yield return new WaitForSeconds(1f);
        explosionCollider.SetActive(false);
        yield return new WaitForSeconds(2f);
        //explosionParticle.SetActive(false);
        Destroy(gameObject);
    }
}
