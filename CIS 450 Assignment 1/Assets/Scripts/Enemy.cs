/* Caleb Kahn
 * Enemy
 * Assignment 1 (Easy)
 * Abstract class for movable enemies
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Killable, Attack
{
    public float speed;
    protected int health;
    public string name;

    public abstract void Move();

    public void Attack(Enemy target)
    {
        Debug.Log(name + " attacks " + target.name + " for " + 2 + " damage.");
        target.Hit(2);
    }

    public virtual void Hit(int damage)
    {
        bool alive = health > 0;
        health -= damage;
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
