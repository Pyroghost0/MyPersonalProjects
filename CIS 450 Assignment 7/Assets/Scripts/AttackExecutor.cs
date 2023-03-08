/* Caleb Kahn
 * AttackExecutor
 * Assignment 7 (Hard)
 * Reciever that exicutes the attack patterns
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackExecutor : MonoBehaviour
{
    public GameObject blackProjectile;
    public Sprite[] projectileSprites;
    public Sprite[] specialProjectileSprites;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float DiagnalAttack()
    {
        StartCoroutine(DiagnalAttackCoroutine());
        return 7f;
    }

    IEnumerator DiagnalAttackCoroutine()
    {
        Vector2 velocity = new Vector2(0f, -2f);
        yield return new WaitForSeconds(1f);
        if (Random.value < .5f)
        {
            Vector2 spawnPosition = new Vector2(-3f, 2f);
            int ranNotSpawn = Random.Range(2, 7);
            for (int i = 0; i < 8; i++)
            {
                if (i > 1)//Top black
                {
                    SpawnProjectile(spawnPosition + new Vector2(-2f, 0f), velocity);
                }
                if (i > 0 && i < 7)//Middle red
                {
                    if (i == ranNotSpawn)
                    {
                        SpawnProjectile(spawnPosition + new Vector2(-1f, 0f), velocity);
                    }
                    else
                    {
                        SpawnProjectile(spawnPosition + new Vector2(-1f, 0f), velocity, false);
                    }
                }
                if (i < 6)//Bottom black
                {
                    SpawnProjectile(spawnPosition, velocity);
                }
                yield return new WaitForSeconds(1f);
                spawnPosition += new Vector2(1f, 0f);
            }
        }
        else
        {
            Vector2 spawnPosition = new Vector2(3f, 2f);
            int ranNotSpawn = Random.Range(2, 7);
            for (int i = 0; i < 8; i++)
            {
                if (i > 1)//Top black
                {
                    SpawnProjectile(spawnPosition + new Vector2(2f, 0f), velocity);
                }
                if (i > 0 && i < 7)//Middle red
                {
                    if (i == ranNotSpawn)
                    {
                        SpawnProjectile(spawnPosition + new Vector2(1f, 0f), velocity);
                    }
                    else
                    {
                        SpawnProjectile(spawnPosition + new Vector2(1f, 0f), velocity, false);
                    }
                }
                if (i < 6)//Bottom black
                {
                    SpawnProjectile(spawnPosition, velocity);
                }
                yield return new WaitForSeconds(1f);
                spawnPosition += new Vector2(-1f, 0f);
            }
        }
    }

    public float RainbowAttack()
    {
        StartCoroutine(DiagnalAttackCoroutine());
        return 5f;
    }

    private void SpawnProjectile(Vector2 spawnPosition, Vector2 direction, bool black = true, int spriteNum = -1, bool lookDir = false, float turnAmount = 0f, float followAmount = 0f, float followTime = 2f, float xAccel = 0f, float yAccel = 0f)//Either follow player or accelorate or none
    {
        GameObject projectile = projectile = Instantiate(blackProjectile, spawnPosition, lookDir ? Quaternion.Euler(0, 0f, Mathf.Atan2(direction.y, direction.x) * 57.2958f - 90f) : transform.rotation);
        if (!black)
        {
            projectile.GetComponent<SpriteRenderer>().color = Color.red;
            projectile.GetComponent<Projectile>().blackBullet = false;
        }
        //projectile.GetComponent<SpriteRenderer>().sprite = projectileSprites[spriteNum == -1 ? Random.Range(0, projectileSprites.Length) | spriteNum];
        projectile.GetComponent<Rigidbody2D>().velocity = direction;
        if (followAmount != 0f)//Can have negetive values
        {
            projectile.GetComponent<Projectile>().FollowPlayer(followAmount, followTime);
            if (turnAmount != 0f)
            {
                projectile.GetComponent<Projectile>().Turn(turnAmount);
            }
            else if (lookDir)
            {
                projectile.GetComponent<Projectile>().Look();
            }
        }
        else if (xAccel != 0f || yAccel != 0f)//Can have negetive values
        {
            projectile.GetComponent<Projectile>().Accelorate(new Vector2(xAccel, yAccel));
            if (lookDir)
            {
                projectile.GetComponent<Projectile>().Look();
            }
        }
        else if (turnAmount != 0f)
        {
            projectile.GetComponent<Projectile>().Turn(turnAmount);
        }
    }
}
