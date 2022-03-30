/* Caleb Kahn
 * Assignment 6
 * Enemy behavior and some parts of the powerups
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : TextElements
{
    public bool shielded;
    public ProtectPowerup protect;
    public SpeedPowerup speed;
    public InvisablePowerup invisable;
    public float movementSpeed;
    public GameObject shield;
    private EndlessManager manager;
    private TutorialManager tm;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        manager = FindObjectOfType<EndlessManager>();
        if (manager != null)
        {
            movementSpeed = manager.movementSpeed;
            NewLetter();
        }
        else
        {
            tm = FindObjectOfType<TutorialManager>();
            movementSpeed = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 amount = Vector3.right * movementSpeed * Time.deltaTime;
        if (speed != null)
        {
            amount *= 2;
            speed.gameObject.transform.Translate(Vector3.down * 2 * movementSpeed * Time.deltaTime);
        }
        if (protect != null)
        {
            protect.gameObject.transform.Translate(amount);
        }
        if (invisable != null)
        {
            invisable.gameObject.transform.Translate(amount);
        }
        transform.Translate(amount);
        if (transform.position.x >= 53f)
        {
            manager.EndGame();
        }
        if (letter != string.Empty && Input.GetKeyDown(letter) && !shielded)
        {
            if (manager != null)
            {
                manager.enemyAmount--;
                Destroy(this.gameObject);
            }
            else if (tm.stage == 3)
            {
                tm.stage++;
                tm.continueText.text = "Press Any Arrow Key To Continue";
                transform.position = new Vector3(-30f, 5f, 0f);
                tm.speed.gameObject.SetActive(true);
                tm.protect.gameObject.SetActive(true);
                tm.invisable.gameObject.SetActive(true);
                tm.mainText.text = "The Things In The Middle Are Powerups For The Enemy";
            }
            else if (tm.canKill)
            {
                tm.stage++;
                tm.continueText.text = "Press Any Arrow Key To Continue";
                tm.mainText.text = "Notice How Powerups Need To Be Removed First";
                Destroy(this.gameObject);
            }
        }
    }

    private void LetterReset()
    {
        letter = string.Empty;
        text.text = letter;
        if (speed != null)
        {
            speed.disabled = false;
            speed.NewLetter();
        }
        if (invisable != null)
        {
            invisable.disabled = false;
            invisable.NewLetter();
        }
        if (protect != null)
        {
            protect.disabled = false;
            protect.NewLetter();
            shielded = true;
            shield.SetActive(true);
            shield.GetComponent<Shield>().NewLetter();
            protect.text.text = string.Empty;
            if (speed != null)
            {
                speed.text.text = string.Empty;
            }
            if (invisable != null)
            {
                invisable.text.text = string.Empty;
            }
        }
        if (speed == null && invisable == null && protect == null)
        {
            NewLetter();
        }
    }

    public void PowerUpDefeated(EnemyPowerup enemyPowerup)
    {
        enemyPowerup.Disable();
        if ((protect == null || protect.disabled) && (speed == null || speed.disabled) && (invisable == null || invisable.disabled))
        {
            if (invisable != null && protect != null && protect.id == enemyPowerup.id)
            {
                invisable.protect = null;
            }
            else if (invisable != null && speed != null && speed.id == enemyPowerup.id)
            {
                invisable.speed = null;
            }
            if (enemyPowerup.id == 0)
            {
                invisable = null;
            }
            else if (enemyPowerup.id == 1)
            {
                protect = null;
            }
            else
            {
                speed = null;
            }
            enemyPowerup.Attacked();
            LetterReset();
        }
        if (protect == null && speed == null && invisable == null)
        {
            NewLetter();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Protect") && protect == null && other.GetComponent<ProtectPowerup>().enemy == null && !other.GetComponent<ProtectPowerup>().attacked)
        {
            protect = other.GetComponent<ProtectPowerup>();
            other.transform.position = new Vector3(transform.position.x-3f, transform.position.y, transform.position.z+3f);
            protect.enemy = this;
            if (invisable != null)
            {
                invisable.protect = protect;
            }
            LetterReset();
        }
        else if (other.CompareTag("Speed") && speed == null && other.GetComponent<SpeedPowerup>().enemy == null && !other.GetComponent<SpeedPowerup>().attacked)
        {
            speed = other.GetComponent<SpeedPowerup>();
            other.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            speed.enemy = this;
            if (invisable != null)
            {
                invisable.speed = speed;
            }
            LetterReset();
        }
        else if (other.CompareTag("Invisable") && invisable == null && other.GetComponent<InvisablePowerup>().enemy == null && !other.GetComponent<InvisablePowerup>().attacked)
        {
            invisable = other.GetComponent<InvisablePowerup>();
            other.transform.position = new Vector3(transform.position.x+3, transform.position.y, transform.position.z);
            invisable.enemy = this;
            invisable.speed = speed;
            invisable.protect = protect;
            LetterReset();
        }
        else if (other.CompareTag("Enemy"))
        {
            int thisNumPowerUps = 0;
            if (speed != null)
            {
                thisNumPowerUps++;
            }
            if (protect != null)
            {
                thisNumPowerUps++;
            }
            if (invisable != null)
            {
                thisNumPowerUps++;
            }
            int theirNumPowerUps = 0;
            EnemyMovement them = other.GetComponent<EnemyMovement>();
            if (them.speed != null)
            {
                theirNumPowerUps++;
            }
            if (them.protect != null)
            {
                theirNumPowerUps++;
            }
            if (them.invisable != null)
            {
                theirNumPowerUps++;
            }
            if ((thisNumPowerUps < theirNumPowerUps) || (thisNumPowerUps == theirNumPowerUps && them.transform.position.x > transform.position.x))
            {
                if (speed != null)
                {
                    speed.Disable();
                    speed.Attacked();
                }
                if (protect != null) {
                    protect.Disable();
                    protect.Attacked();
                }
                if (invisable != null)
                {
                    invisable.Disable();
                    invisable.Attacked();
                }
                manager.enemyAmount--;
                Destroy(this.gameObject);
            }
            else
            {
                if (them.speed != null)
                {
                    them.speed.Disable();
                    them.speed.Attacked();
                }
                if (them.protect != null)
                {
                    them.protect.Disable();
                    them.protect.Attacked();
                }
                if (them.invisable != null)
                {
                    them.invisable.Disable();
                    them.invisable.Attacked();
                }
                manager.enemyAmount--;
                Destroy(them.gameObject);
            }
        }
    }
}
