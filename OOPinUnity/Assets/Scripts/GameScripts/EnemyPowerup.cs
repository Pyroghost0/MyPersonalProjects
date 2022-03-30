/* Caleb Kahn
 * Assignment 6
 * Parent class to all powerups
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyPowerup : TextElements
{
    private bool isFalling;
    public bool disabled;
    public bool attacked;
    protected float origionalPos;
    public EnemyMovement enemy;
    public int id;
    protected EndlessManager manager;
    protected float timer;
    protected TutorialManager tm;

    protected override void Awake()
    {
        base.Awake();
        isFalling = true;
        disabled = true;
        attacked = false;
        timer = 0;
        manager = FindObjectOfType<EndlessManager>();
        if (manager == null)
        {
            tm = FindObjectOfType<TutorialManager>();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        timer += Time.deltaTime;
        if (enemy == null && manager != null && manager.respawnTime <= timer)
        {
            manager.powerUpAmount--;
            Destroy(this.gameObject);
        }
        if (isFalling && id != 2)
        {
            transform.Translate(Vector3.down * 20 * Time.deltaTime);
            if (transform.position.y <= 5f)
            {
                transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
                isFalling = false;
            }
        }
        else if (isFalling)
        {
            transform.Translate(Vector3.left * 20 * Time.deltaTime);
            if (transform.position.y <= 5f)
            {
                transform.position = new Vector3(transform.position.x, 5f, transform.position.z);
                isFalling = false;
            }
        }
        if (tm != null)
        {
            if (tm.canKill && letter != string.Empty && Input.GetKeyDown(letter) && !enemy.shielded)
            {
                disabled = true;
                text.text = string.Empty;
                enemy.PowerUpDefeated(this);
            }
        }
        else if (letter != string.Empty && Input.GetKeyDown(letter) && !enemy.shielded) {
            disabled = true;
            text.text = string.Empty;
            enemy.PowerUpDefeated(this);
        }
    }

    public void Disable()
    {
        disabled = true;
        text.text = string.Empty;
        letter = string.Empty;
    }
    public abstract void Attacked();
}
