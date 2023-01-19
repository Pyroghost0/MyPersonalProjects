/* Caleb Kahn
 * Robot
 * Assignment 1 (Easy)
 * Robot Enemy that is a typical enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : Enemy
{
    public Robot()
    {
        speed = 2.5f;
        health = 15;
        name = "Robot";
    }

    public override void Move()
    {
        Debug.Log(name + " rolled " + speed + " meters.");
    }
}
