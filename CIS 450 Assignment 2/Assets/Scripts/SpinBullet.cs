/* Caleb Kahn
 * SpinBullet
 * Assignment 2 (Hard)
 * This bullet type goes in a circle
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBullet : MonoBehaviour, BulletBehaivior
{
    //Applies velocity for a spin bullet
    public void Shoot(GameObject bullet)
    {
        bullet.name = "Spin Bullet";
        bullet.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 30f);
        StartCoroutine(Spin(bullet));
    }

    //Spins the bullet
    IEnumerator Spin(GameObject bullet)
    {
        float degrees = Random.Range(0f, 6.283f);
        float initX = bullet.transform.position.x;
        float initY = bullet.transform.position.y;
        while (bullet != null)
        {
            bullet.transform.position = new Vector3(initX + Mathf.Cos(degrees) * .75f, initY + Mathf.Sin(degrees) * .75f, bullet.transform.position.z);
            degrees += Time.deltaTime * 6.283f;
            yield return new WaitForFixedUpdate();
        }
    }
}
