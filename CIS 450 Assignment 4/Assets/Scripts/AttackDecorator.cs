/* Caleb Kahn
 * AttackDecorator
 * Assignment 4 (Hard)
 * Decorates the attack action by specifying which attack (abstract class)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackDecorator : AttackComponent
{
    //public override abstract float totalDelay { get; set; }
    //public override abstract float delay { get; set; }
    //[field: SerializeField] public virtual float delay { set; get; } = 1f;
    public float delay = 0f;
    [field: SerializeField] public virtual float speed { set; get; } = 2f;
    public float nextSpeed = 1f;
    //[field: SerializeField] public virtual float prevSpeed { set; get; } = 0f;
    [field: SerializeField] public virtual float attackTime { set; get; } = .5f;
    public float nextAttackTime = 1f;
    //[field: SerializeField] public virtual float prevAttackTime { set; get; } = 0f;
    [field: SerializeField] public virtual int damage { set; get; } = 5;
    public float nextDamage = 1f;
    //[field: SerializeField] public virtual int prevDamage { set; get; } = 5;
    public bool yDirection = true;
}
