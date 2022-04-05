/* Caleb Kahn
 * Assignment 7
 * Allows for player to be controled and manages player behavior
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    private Rigidbody playerRB;
    public float speed;
    private float forwardInput;

    private GameObject focalPoint;

    public bool hasPowerUp;
    private float powerupStrength = 15f;

    public GameObject powerupIndicator;
    public Text gameOverText;

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        focalPoint = GameObject.FindGameObjectWithTag("FocalPoint");
    }

    // Update is called once per frame
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        //playerRB.AddForce(Vector3.forward * speed * forwardInput /** Time.deltaTime*/);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -.5f, 0);
        if (transform.position.y < -10)
        {
            gameOverText.text = "You Lose! Press R to Restart!";
            GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>().lost = true;
            Time.timeScale = 0f;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        playerRB.AddForce(focalPoint.transform.forward * speed * forwardInput * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup")) {
            hasPowerUp = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized;
            enemyRigidBody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        powerupIndicator.gameObject.SetActive(false);
    }
}
