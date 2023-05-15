/* Caleb Kahn
 * StraightBullet
 * Assignment 2 (Hard)
 * This bullet type goes in a straight line
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : MonoBehaviour, BulletBehaivior
{
    //Applies velocity for a straight bullet
    public void Shoot(GameObject bullet)
    {
        bullet.name = "Straight Bullet";
        bullet.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 50f);
    }
}
