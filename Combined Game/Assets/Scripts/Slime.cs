using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Transform target;
    public int health = 2;
    public float speed = 1f;
    public float slowedSpeed = .75f;
    //public float followDistance = 10f;
    public float shootDistance = 5f;
    private bool slowed = false;
    public bool knockback = false;
    public Spawner spawner;
    public Animator animator;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
    public Collider2D collider;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public SpriteRenderer spriteRenderer;
    public GameObject deathCollider;
    public GameObject pickaxePrefab;
    public GameObject projectilePrefab;
    public bool pickaxeSlime = false;
    public EnemyColor enemyColor;
    public GameObject destroyedBodyPrefab;

    public AudioSource WalkSound;
    public AudioSource shootSound;
    public AudioSource deathSound;

    public

    //Sets up variables
    IEnumerator Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        while (true)
        {
            WalkSound.Play();
            while ((target.position - transform.position).magnitude > shootDistance)
            {
                if (!knockback)
                {
                    rigidbody.velocity = ((target.position - transform.position).normalized) * (slowed ? slowedSpeed : speed);
                    animator.SetFloat("X", rigidbody.velocity.normalized.x);
                    animator.SetFloat("Y", rigidbody.velocity.normalized.y);
                }
                yield return new WaitForFixedUpdate();
            }
            if (!knockback)
            {
                animator.SetFloat("X", rigidbody.velocity.normalized.x);
                animator.SetFloat("Y", rigidbody.velocity.normalized.y);
                animator.SetBool("Stop", true);
            }
            WalkSound.loop = false;
            while ((target.position - transform.position).magnitude <= shootDistance)
            {
                float timer = 0f;
                while (timer < .66f)
                {
                    if (!knockback)
                    {
                        if (rigidbody.velocity.magnitude > 0f)
                        {
                            rigidbody.velocity -= rigidbody.velocity.normalized * (Mathf.Min(Time.deltaTime * 2f, rigidbody.velocity.magnitude));
                        }
                    }
                    yield return new WaitForFixedUpdate();
                    timer += Time.deltaTime;
                }
                Vector2 dir = (target.position - transform.position).normalized;
                animator.SetFloat("X", dir.x);
                animator.SetFloat("Y", dir.y);
                animator.SetTrigger("Shoot");
                Vector3 pos = target.position;
                yield return new WaitForSeconds(5/6f);
                shootSound.Play();
                Vector2 delta = pos - transform.position;
                float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
                GameObject thrownObject = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0f, 0f, angle - 90f));
                thrownObject.GetComponent<ThrownObject>().time = (delta.magnitude + 1f) / 3.75f;
                thrownObject.GetComponent<ThrownObject>().velocity = delta / thrownObject.GetComponent<ThrownObject>().time;
                thrownObject.GetComponent<ThrownObject>().target = pos;
                timer = 0f;
                while (timer < .33f)
                {
                    if (!knockback)
                    {
                        if (rigidbody.velocity.magnitude > 0f)
                        {
                            rigidbody.velocity -= rigidbody.velocity.normalized * (Mathf.Min(Time.deltaTime * 2f, rigidbody.velocity.magnitude));
                        }
                    }
                    yield return new WaitForFixedUpdate();
                    timer += Time.deltaTime;
                }
            }
            if (!knockback)
            {
                animator.SetBool("Stop", false);
            }
            WalkSound.loop = true;
        }
    }

    //Take damage from contact
    //private void OnCollisionStay2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && health > 0)
        {
            Destroy(collision.gameObject);
            health--;
            if (health <= 0)
            {
                StopAllCoroutines();
                StartCoroutine(Death());
            }
            else
            {
                //StartCoroutine(IFrames());
                StartCoroutine(Knockback((transform.position - collision.transform.position).normalized, .5f));
            }
        }
        else if (collision.gameObject.CompareTag("Explosion") && health > 0)
        {
            if (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Bomb || (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Red && ((int)enemyColor != 3)) ||
                (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Yellow && ((int)enemyColor != 4)) || (collision.transform.parent.GetComponent<ThrownObject>().objectType == ItemType.Blue && ((int)enemyColor != 5)))
            {
                if (pickaxeSlime)
                {
                    GameObject pickaxe = Instantiate(pickaxePrefab, transform.position, transform.rotation);
                    pickaxe.GetComponent<Item>().itemType = ItemType.Pickaxe;
                    pickaxe.GetComponent<ParticleSystem>().Play();
                    pickaxe.transform.localScale = new Vector3(2f, 2f, 2f);
                }
#pragma warning disable CS0618 // Type or member is obsolete
                Instantiate(destroyedBodyPrefab, transform.position, transform.rotation).GetComponent<ParticleSystem>().startColor = enemyColor == EnemyColor.Green ? new Color(0f, 1f, 0f, .3f) : enemyColor == EnemyColor.Purple ? new Color(1f, 0f, 1f, .3f) : new Color(1f, .5f, 0f, .3f);
                Destroy(gameObject);
                /*health = 0;
                StopAllCoroutines();
                StartCoroutine(Death());*/
            }
        }
    }

    public IEnumerator Death()
    {
        rigidbody.velocity = Vector2.zero;
        collider.enabled = false;
        animator.SetTrigger("Death");
        deathSound.Play();
        if (spawner != null)
        {
            spawner.EnemyDeath();
        }
        yield return new WaitForSeconds(.6667f);
        if (pickaxeSlime)
        {
            GameObject pickaxe = Instantiate(pickaxePrefab, transform.position, transform.rotation);
            pickaxe.GetComponent<Item>().itemType = ItemType.Pickaxe;
            pickaxe.GetComponent<ParticleSystem>().Play();
            pickaxe.transform.localScale = new Vector3(2f, 2f, 2f);
        }
        yield return new WaitForSeconds(.1333f);
        deathCollider.SetActive(true);
        yield return new WaitForSeconds(2f);
        float timer = 0f;
        while(timer < 1f)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f-timer);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        Destroy(gameObject);
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Slow Enemy"))
        {
            slowed = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Slow Enemy"))
        {
            slowed = false;
        }
    }

    //Knockback
    IEnumerator Knockback(Vector3 dir, float power)
    {
        animator.SetFloat("X", 0f);
        animator.SetFloat("Y", 0f);
        animator.SetBool("Stop", true);
        knockback = true;
        float timer = 0f;
        dir *= power;
        while (timer < power)
        {
            rigidbody.velocity = dir * (1f - (timer / power));
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime * (1f + (rigidbody.velocity.magnitude * rigidbody.velocity.magnitude / 4f));
        }
        knockback = false;
        if ((target.position - transform.position).magnitude > shootDistance)
        {
            animator.SetBool("Stop", false);
        }
    }
}
