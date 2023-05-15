/* Caleb Kahn
 * RainbowAttack
 * Assignment 7 (Hard)
 * Attack type where projectiles spawn random projectile from above
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpRandomAttack : Attack
{
    private AttackExecutor attackExecutor;

    public DumpRandomAttack(AttackExecutor executor)
    {
        attackExecutor = executor;
    }

    public void StartAttack()
    {
        attackExecutor.DumpRandomAttack();
    }
}
