/* Caleb Kahn
 * Bat
 * Assignment 1 (Easy)
 * Bat Enemy that flys
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy
{
    public float ySpeed = 1f;

    public Bat()
    {
        speed = 1.5f;
        health = 5;
        name = "Bat";
    }

    public override void Move()
    {
        Debug.Log(name + " moved " + speed + " meters and " + ySpeed + " meters up/down.");
    }
}
