/* Caleb Kahn
 * AttackComponent
 * Assignment 4 (Hard)
 * Abstract class for creating a series of attacks
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class AttackComponent
{
    [field: SerializeField] public virtual float totalDelay { get; } = 0f;
    [field: SerializeField] public virtual int activeAttackPower { get; } = 0;
    [field: SerializeField] public virtual bool attacking { set; get; } = false;
    [field: SerializeField] public virtual bool inAction { set; get; } = false;
    public abstract void Reset();
    public abstract void StartAttack();
    protected abstract IEnumerator AttackBehaivior();
}
