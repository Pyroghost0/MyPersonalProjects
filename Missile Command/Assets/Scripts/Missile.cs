using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 10f;
    public Vector3 goal;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0f, Mathf.Atan2((goal - transform.position).y, (goal - transform.position).x) * 57.2958f - 90f);
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
        StartCoroutine(Explosion());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Building"))
        {
            GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>().buildings.Remove(collision.transform);
            if (GameObject.FindGameObjectWithTag("Spawn Manager").GetComponent<SpawnManager>().buildings.Count == 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
            Destroy(collision.gameObject);
            StopAllCoroutines();
            StartCoroutine(Explosion());
        }
    }

    IEnumerator Explosion()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        animator.SetTrigger("Explosion");
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
    }
}
