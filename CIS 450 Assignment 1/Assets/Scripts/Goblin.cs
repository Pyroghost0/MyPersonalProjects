/* Caleb Kahn
 * Goblin
 * Assignment 1 (Easy)
 * Goblin Enemy that runs away of scared
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    public int fearLevel = 1;

    public Goblin()
    {
        speed = 2f;
        health = 10;
        name = "Goblin";
    }

    public override void Move()
    {
        if (fearLevel < 0)
        {
            Debug.Log(name + " ran away " + speed + " meters.");
        }
        else
        {
            Debug.Log(name + " moved " + speed + " meters.");
        }
    }

    public override void Hit(int damage)
    {
        bool alive = health > 0;
        health -= damage;
        fearLevel--;
        Debug.Log(name + " was hit for " + damage + " damage.");
        if (health <= 0 && alive)
        {
            Debug.Log(name + " died.");
        }
        else if (health <= 0)
        {
            Debug.Log("But " + name + " was already dead, why are you hiiting it?");
        }
    }
}
