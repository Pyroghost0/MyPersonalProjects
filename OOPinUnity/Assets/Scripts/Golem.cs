/* Caleb Kahn
 * Assignment 6
 * Example child class of enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Enemy
{
    protected int damage;
    
    protected override void Awake()
    {
        base.Awake();
        health = 120;
        //GameManager.score = 5;
        //GameManager.instance.score+=2;
        GameManager.Instance.score += 2;
    }

    protected override void Attack(int amount)
    {
        Debug.Log("Golem attacks");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TakeDamage(int amount)
    {
        Debug.Log("You took " + amount + " damage");
    }
}
