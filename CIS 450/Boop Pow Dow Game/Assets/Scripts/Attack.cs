/* Caleb Kahn
 * Attack
 * Assignment 4 (Hard)
 * Attacks using the decorators initialised from this class
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : AttackComponent
{
    //Resets variables
    public override void Reset()
    {
        inAction = false;
    }

    //Starts attack
    public override void StartAttack()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().InstantiateCoroutine(AttackBehaivior());
    }

    //Determines if attack is active
    protected override IEnumerator AttackBehaivior()
    {
        inAction = true;
        yield return new WaitForSeconds(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().attack.totalDelay);
        inAction = false;
    }
}
