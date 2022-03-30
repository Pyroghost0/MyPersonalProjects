/* Caleb Kahn
 * Assignment 6
 * Protect powerup that gives the enemy a shield
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectPowerup : EnemyPowerup
{
    protected override void Awake()
    {
        base.Awake();
        id = 1;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (attacked)
        {
            transform.Translate(Vector3.forward * 8 * Time.deltaTime);
            if (transform.position.z >= 20f)
            {
                manager.powerUpAmount--;
                Destroy(this.gameObject);
            }
            if (transform.position.z >= origionalPos + 12f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, origionalPos+12);
                attacked = false;
            }
        }
    }

    /*public override void NewLetter()
    {
        base.NewLetter();

    }*/

    public override void Attacked()
    {
        timer = 0;
        enemy = null;
        origionalPos = transform.position.z;
        attacked = true;
        text.color = Color.white;
    }
}
