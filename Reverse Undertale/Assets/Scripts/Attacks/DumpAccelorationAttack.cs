/* Caleb Kahn
 * DumpAccelorationAttack
 * Assignment 7 (Hard)
 * Attack type where projectiles spawn accelorating projectiles
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpAccelorationAttack : Attack
{
    private AttackExecutor attackExecutor;

    public DumpAccelorationAttack(AttackExecutor executor)
    {
        attackExecutor = executor;
    }

    public void StartAttack()
    {
        attackExecutor.DumpAccelorationAttack();
    }
}

