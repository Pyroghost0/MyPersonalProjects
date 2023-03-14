/* Caleb Kahn
 * RainbowAttack
 * Assignment 7 (Hard)
 * Attack type where projectiles spawn in a rainbow pattern
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowAttack : Attack
{
    private AttackExecutor attackExecutor;

    public RainbowAttack(AttackExecutor executor)
    {
        attackExecutor = executor;
    }

    public void StartAttack()
    {
        attackExecutor.RainbowAttack();
    }
}
