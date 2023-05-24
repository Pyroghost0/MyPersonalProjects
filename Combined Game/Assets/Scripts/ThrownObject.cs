/* Caleb Kahn
 * ThrownObject
 * Assignment 6 (Hard)
 * Object thrown from player that lands in a spot
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    public ItemType objectType;
    public GameObject explosionParticle;
    public GameObject explosionCollider;
    public Vector3 target;
    public Vector3 velocity;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Throw());
    }

    IEnumerator Throw()
    {
        float turnRadius = Random.Range(-180f, 180f);
        float timer = 0f;
        Vector3 initScale = transform.localScale;
        float maxScale = (time * .5f) + 1f;
        while (timer < time)
        {
            transform.localScale = initScale * (maxScale - (Mathf.Abs(time/2f - timer) * .5f));
            transform.Rotate(0f, 0f, turnRadius * Time.deltaTime);
            transform.position += velocity * Time.deltaTime;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        transform.localScale = initScale;
        transform.position = target;
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
