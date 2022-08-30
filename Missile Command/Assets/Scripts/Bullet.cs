using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public CircleCollider2D collider;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public Animator animator;
    public Vector3 goal;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        Vector3 movement = (goal - transform.position).normalized * speed;
        float goalTimer = (goal - transform.position).magnitude / speed;
        float timer = 0f;
        while (timer < goalTimer)
        {
            yield return new WaitForFixedUpdate();
            transform.position += movement * Time.deltaTime;
            timer += Time.deltaTime;
        }
        transform.position = goal;
        collider.enabled = true;
        animator.SetTrigger("Explosion");
        yield return new WaitForSeconds(.08333f);
        collider.radius = .8f;
        yield return new WaitForSeconds(.08333f);
        collider.radius = 1f;
        yield return new WaitForSeconds(.16666f);
        collider.radius = .8f;
        yield return new WaitForSeconds(.16333f);
        Destroy(gameObject);
    }
}
