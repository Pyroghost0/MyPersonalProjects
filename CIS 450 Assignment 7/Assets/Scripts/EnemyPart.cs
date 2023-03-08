/* Caleb Kahn
 * EnemyPart
 * Assignment 7 (Hard)
 * reacts the same way the enemy does when hit
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPart : MonoBehaviour
{
    public GameObject hitParticle;
    public Enemy enemy;

    //Take damage from contact
    //private void OnCollisionStay2D(Collision2D collision)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !collision.GetComponent<EnemyHitBullet>().hit)
        {
            //hitAudio.Play();
            collision.GetComponent<EnemyHitBullet>().hit = true;
            Instantiate(hitParticle, collision.transform.position, Quaternion.Euler(Mathf.Atan2(-collision.transform.localPosition.y, collision.transform.localPosition.x) * 57.2958f, 90f, -90f));
            Destroy(collision.gameObject);
            enemy.Hit();
        }
    }
}
