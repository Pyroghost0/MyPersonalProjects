/* Caleb Kahn
 * Assignment 6
 * Invisable powerup that makes the text turn invisable
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisablePowerup : EnemyPowerup
{
    private float opacity;
    private bool decrease;
    public ProtectPowerup protect;
    public SpeedPowerup speed;

    protected override void Awake()
    {
        base.Awake();
        id = 0;
        decrease = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (decrease) {
            opacity -= Time.deltaTime;
            if (opacity < 0) {
                text.color = new Color(1.0f, 1.0f, 1.0f, 0f);
                decrease = false;
                if (protect != null) {
                    protect.text.color = new Color(1.0f, 1.0f, 1.0f, 0f);
                    enemy.shield.GetComponent<Shield>().text.color = new Color(1.0f, 1.0f, 1.0f, 0f);
                }
                if (speed != null) {
                    speed.text.color = new Color(1.0f, 1.0f, 1.0f, 0f);
                }
            }
            else {
                text.color = new Color(1.0f, 1.0f, 1.0f, opacity);
                if (protect != null) {
                    protect.text.color = new Color(1.0f, 1.0f, 1.0f, opacity);
                    enemy.shield.GetComponent<Shield>().text.color = new Color(1.0f, 1.0f, 1.0f, opacity);
                }
                if (speed != null) {
                    speed.text.color = new Color(1.0f, 1.0f, 1.0f, opacity);
                }
            }
        }
        else {
            opacity += Time.deltaTime;
            if (opacity > 1) {
                text.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                decrease = true;
                if (protect != null) {
                    protect.text.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                    enemy.shield.GetComponent<Shield>().text.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                }
                if (speed != null) {
                    speed.text.color = new Color(1.0f, 1.0f, 1.0f, 1f);
                }
            }
            else {
                text.color = new Color(1.0f, 1.0f, 1.0f, opacity);
                if (protect != null)
                {
                    protect.text.color = new Color(1.0f, 1.0f, 1.0f, opacity);
                    enemy.shield.GetComponent<Shield>().text.color = new Color(1.0f, 1.0f, 1.0f, opacity);
                }
                if (speed != null)
                {
                    speed.text.color = new Color(1.0f, 1.0f, 1.0f, opacity);
                }
            }
        }
        if (attacked) {
            transform.Translate(Vector3.right * 20 * Time.deltaTime);
            if (transform.position.x >= 45f) {
                manager.powerUpAmount--;
                Destroy(this.gameObject);
            }
            if (transform.position.x >= origionalPos + 30f) {
                transform.position = new Vector3(origionalPos + 30f, transform.position.y, transform.position.z);
                attacked = false;
            }
        }
    }

    public override void Attacked()
    {
        timer = 0;
        enemy = null;
        protect = null;
        speed = null;
        origionalPos = transform.position.x;
        attacked = true;
        text.color = Color.white;
    }
}
