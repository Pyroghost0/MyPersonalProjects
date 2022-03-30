/* Caleb Kahn
 * Assignment 6
 * Speed Powerup that makes enemy go twice as fast
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerup : EnemyPowerup
{
    protected override void Awake()
    {
        base.Awake();
        id = 2;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (manager == null && transform.position.x >= 30f)
        {
            enemy.movementSpeed = 0f;
            tm.speedDemenstration = true;
            tm.continueText.gameObject.SetActive(true);
        }
        if (attacked)
        {
            transform.Translate(Vector3.back * 8 * Time.deltaTime);
            if (transform.position.z <= -20f)
            {
                manager.powerUpAmount--;
                Destroy(this.gameObject);
            }
            if (transform.position.z <= origionalPos - 12f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, origionalPos - 12);
                attacked = false;
            }
        }
    }

    public override void Attacked()
    {
        timer = 0;
        enemy = null;
        origionalPos = transform.position.z;
        attacked = true;
        text.color = Color.white;
    }
}
