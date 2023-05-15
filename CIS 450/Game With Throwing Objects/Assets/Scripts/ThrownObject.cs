/* Caleb Kahn
 * ThrownObject
 * Assignment 7 (Hard)
 * Object thrown from player that lands in a spot
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrownObject : MonoBehaviour
{
    public Vector3 target;
    public Vector3 velocity;
    public float time;

    //Initiates Throw
    void Start()
    {
        StartCoroutine(Throw());
    }

    //Throws item to spot
    private IEnumerator Throw()
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
        StartCoroutine(ObjectEffect());
    }

    protected abstract IEnumerator ObjectEffect();
}
