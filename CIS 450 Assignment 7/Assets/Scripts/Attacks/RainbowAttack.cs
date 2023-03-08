/* Caleb Kahn
 * RainbowAttack
 * Assignment 7 (Hard)
 * Attack type where projectiles spawn in a rainbow pattern
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowAttack : MonoBehaviour
{
    private AttackExecutor attackExecutor;

    public RainbowAttack(AttackExecutor executor)
    {
        attackExecutor = executor;
    }

    public float StartAttack()
    {
        return attackExecutor.RainbowAttack();
    }
}
