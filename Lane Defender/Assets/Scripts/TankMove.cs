using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour
{
    public float movementSpeed = 1.5f;
    public float shootCooldownTime = .5f;
    private bool shooting;
    private float timer = 0f;
    public GameObject bullet;
    public GameObject explosion;
    public Transform bulletSpawnPosition;
    public AudioSource shoot;
    public AudioSource hit;
    public AudioSource death;

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            timer += Time.deltaTime;
            if (timer >= shootCooldownTime)
            {
                shooting = false;
                timer = 0f;
            }
        }
        if (Input.GetKey(KeyCode.Space) && !shooting)
        {
            shoot.Play();
            Instantiate(bullet, bulletSpawnPosition.position, transform.rotation);
            StartCoroutine(Explode(Instantiate(explosion, bulletSpawnPosition.position, transform.rotation)));
            shooting = true;
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
            if (transform.position.y > -.5f)
            {
                transform.position = new Vector3(-8f, -.5f, 0f);
            }
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            transform.Translate(Vector3.down * Time.deltaTime * movementSpeed);
            if (transform.position.y < -4.5f)
            {
                transform.position = new Vector3(-8f, -4.5f, 0f);
            }
        }
    }

    public void ExplodeMethod(GameObject explodeObject)
    {
        StartCoroutine(Explode(explodeObject));
    }

    IEnumerator Explode(GameObject explosionObject)
    {
        yield return new WaitForSeconds(.5f);
        Destroy(explosionObject);
    }
}
