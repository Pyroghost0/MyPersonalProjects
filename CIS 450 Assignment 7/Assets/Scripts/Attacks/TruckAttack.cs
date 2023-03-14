/* Caleb Kahn
 * TruckAttack
 * Assignment 7 (Hard)
 * Attack type where trucks come in from both sides
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckAttack : Attack
{
    private AttackExecutor attackExecutor;

    public TruckAttack(AttackExecutor executor)
    {
        attackExecutor = executor;
    }

    public void StartAttack()
    {
        attackExecutor.TruckAttack();
    }
}
