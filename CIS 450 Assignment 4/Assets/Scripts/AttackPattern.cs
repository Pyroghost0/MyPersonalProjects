/* Caleb Kahn
 * AttackPattern
 * Assignment 4 (Hard)
 * Attacks in specified attack type based on previous attack
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Empty = 0,
    HeavyAttack = 1,
    NormalAttack = 2,
    LightAttack = 3,
    Charge = 4,
    QuickCharge = 5
}

public class AttackPattern : AttackDecorator
{
    private float _speed;
    private float _attackTime;
    private int _damage;
    private bool _attacking = false;
    AttackComponent prevAttackComponent;
    AttackComponent nextAttackComponent;

    //Sets up attack pattern
    public AttackPattern(AttackComponent ac, AttackType attackType, bool yDir, int dir = 1)
    {
        prevAttackComponent = ac;
        if (prevAttackComponent is AttackPattern)
        {
            ((AttackPattern)ac).nextAttackComponent = this;
        }
        if (attackType == AttackType.HeavyAttack)
        {
            _speed = 4f * dir;
            nextSpeed = .6f;
            _attackTime = .6f;
            nextAttackTime = .75f;
            _damage = 8;
            nextDamage = .75f;
            delay = .75f;
        }
        else if (attackType == AttackType.NormalAttack)
        {
            _speed = 3f * dir;
            _attackTime = .5f;
            _damage = 4;
            delay = .45f;
        }
        else if (attackType == AttackType.LightAttack)
        {
            _speed = 2.75f * dir;
            nextSpeed = 1.1f;
            _attackTime = .4f;
            nextAttackTime = 1.1f;
            _damage = 3;
            nextDamage = 1.2f;
            delay = .15f;
        }
        else if (attackType == AttackType.Charge)
        {
            _speed = 0f;
            nextSpeed = 1.4f;
            _attackTime = 0f;
            nextAttackTime = 1.4f;
            nextDamage = 2f;
            delay = 1.5f;
        }
        else if (attackType == AttackType.QuickCharge)
        {
            _speed = 0f;
            nextSpeed = 1.2f;
            _attackTime = 0f;
            nextAttackTime = 1.2f;
            nextDamage = 1.4f;
            delay = .5f;
        }
        yDirection = yDir;
    }

    public override float speed
    {
        get
        {
            return (prevAttackComponent is AttackPattern ? ((AttackPattern)prevAttackComponent).nextSpeed : 1f) * _speed;
        }
        set
        {
            _speed = value;
        }
    }

    public override float attackTime
    {
        get
        {
            return (prevAttackComponent is AttackPattern ? ((AttackPattern)prevAttackComponent).nextAttackTime : 1f) * _attackTime;
        }
        set
        {
            _attackTime = value;
        }
    }

    public override int damage
    {
        get
        {
            return (int)((prevAttackComponent is AttackPattern ? ((AttackPattern)prevAttackComponent).nextDamage : 1f) * _damage);
        }
        set
        {
            _damage = value;
        }
    }

    public override float totalDelay
    {
        get
        {
            return prevAttackComponent.totalDelay + delay + attackTime;
        }
    }

    public override bool attacking
    {
        get
        {
            return prevAttackComponent.attacking || _attacking;
        }
        set
        {
            _attacking = value;
        }
    }

    public override bool inAction
    {
        get
        {
            return prevAttackComponent.inAction;
        }
    }

    public override int activeAttackPower
    {
        get
        {
            return (nextAttackComponent != null && nextAttackComponent.attacking ? nextAttackComponent.activeAttackPower : (attacking ? damage : 0));
        }
    }

    //Resets variables
    public override void Reset()
    {
        prevAttackComponent.Reset();
        attacking = false;
    }

    //Starts attack animation
    public override void StartAttack()
    {
        prevAttackComponent.StartAttack();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().InstantiateCoroutine(AttackBehaivior());
    }

    //Attack movement
    protected override IEnumerator AttackBehaivior()
    {
        yield return new WaitForSeconds(totalDelay - delay - attackTime);
        attacking = true;
        yield return new WaitForFixedUpdate();
        Rigidbody2D rigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        Animator animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        float timer = 0f;
        if (yDirection)
        {
            animator.SetFloat("X", 0f);
            animator.SetFloat("Y", speed > 0f ? 1f : -1f);
        }
        else
        {
            animator.SetFloat("X", speed > 0f ? 1f : -1f);
            animator.SetFloat("Y", 0f);
        }
        while (timer < attackTime)
        {
            if (yDirection)
            {
                animator.SetFloat("Y", speed > 0f ? 1f - timer / attackTime : -1f + timer / attackTime);
            }
            else
            {
                animator.SetFloat("X", speed > 0f ? 1f - timer / attackTime : -1f + timer / attackTime);
            }
            rigidbody.velocity = yDirection ? new Vector2(0f, speed * (1f - (timer / attackTime))) : new Vector2(speed * (1f - (timer / attackTime)), 0f);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        attacking = false;
        yield return new WaitForSeconds(delay);
        /*if (yDirection)
        {
            animator.SetFloat("Y", speed > 0f ? .2f : -.2f);
        }
        else
        {
            animator.SetFloat("X", speed > 0f ? .2f : -.2f);
        }
        rigidbody.velocity = Vector2.zero;
        attacking = false;
        if (delay < .2f)
        {
            yield return new WaitForSeconds(.2f);
            animator.SetFloat("X", 0f);
            animator.SetFloat("Y", 0f);
            yield return new WaitForSeconds(delay-.2f);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }*/
    }
}
