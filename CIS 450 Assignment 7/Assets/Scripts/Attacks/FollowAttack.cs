/* Caleb Kahn
 * FollowAttack
 * Assignment 7 (Hard)
 * Attack type where projectiles follow player or run away from player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAttack : Attack
{
    private AttackExecutor attackExecutor;

    public FollowAttack(AttackExecutor executor)
    {
        attackExecutor = executor;
    }

    public void StartAttack()
    {
        attackExecutor.FollowAttack();
    }
}
