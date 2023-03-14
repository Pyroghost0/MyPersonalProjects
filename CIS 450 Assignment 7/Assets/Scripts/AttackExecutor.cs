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
    public bool isAttacking = false;
    public float multiplier = 1f;
    public GameObject normalProjectile;
    public Sprite[] projectileSprites;
    public GameObject[] specialProjectiles;

    public Transform backTruck;
    public Transform frontTruck;
    public GameObject leftPortal;
    public GameObject rightPortal;
    public Animator canAnimator;
    public Animator binAnimator;
    public GameObject clouds;
    public Transform[] cloudTransforms;
    public GameObject truckPortals;
    public Transform[] portalTransforms;

    public void DiagnalAttack()
    {
        StartCoroutine(DiagnalAttackCoroutine());
    }

    IEnumerator DiagnalAttackCoroutine()
    {
        isAttacking = true;
        float startMultiplier = multiplier;
        Vector2 velocity = new Vector2(0f, -2.5f * startMultiplier);
        if (Random.value < .5f)
        {//Left -> right
            StartCoroutine(MoveTruck(true));
            yield return new WaitForSeconds(.5f / startMultiplier);
            Vector2 spawnPosition = new Vector2(2.875f, 2.5f);
            int ranNotSpawn = Random.Range(2, 6);
            for (int i = 0; i < 7; i++)
            {
                if (i > 1)//Top black
                {
                    SpawnProjectile(spawnPosition + new Vector2(-2f, 0f), velocity);
                }
                if (i > 0 && i < 6)//Middle red
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
                if (i < 5)//Bottom black
                {
                    SpawnProjectile(spawnPosition, velocity);
                }
                yield return new WaitForSeconds(1f / startMultiplier);
                spawnPosition += new Vector2(1f, 0f);
            }
        }
        else
        {
            StartCoroutine(MoveTruck(false));
            yield return new WaitForSeconds(.5f / startMultiplier);
            Vector2 spawnPosition = new Vector2(6.875f, 2.5f);
            int ranNotSpawn = Random.Range(2, 6);
            for (int i = 0; i < 7; i++)
            {
                if (i > 1)//Top black
                {
                    SpawnProjectile(spawnPosition + new Vector2(2f, 0f), velocity);
                }
                if (i > 0 && i < 6)//Middle red
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
                if (i < 5)//Bottom black
                {
                    SpawnProjectile(spawnPosition, velocity);
                }
                yield return new WaitForSeconds(1f / startMultiplier);
                spawnPosition += new Vector2(-1f, 0f);
            }
        }
        yield return new WaitForSeconds(1f / startMultiplier);
        isAttacking = false;
    }

    IEnumerator MoveTruck(bool leftToRight)
    {
        float startMultiplier = multiplier;
        leftPortal.SetActive(true);
        rightPortal.SetActive(true);
        Color color = new Color(Random.Range(.75f, 1f), Random.Range(.75f, 1f), Random.Range(.75f, 1f), 0f);
        float timer = 0f;
        while (timer < .25f / startMultiplier)
        {
            leftPortal.GetComponent<SpriteRenderer>().color = color;
            rightPortal.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            color.a = timer * 4f * startMultiplier;
        }
        color.a = 1f;
        leftPortal.GetComponent<SpriteRenderer>().color = color;
        rightPortal.GetComponent<SpriteRenderer>().color = color;

        //Truck Size = 3.67647
        //Truck Distance = 9.426
        //Real Distance = 5.75 (7.75 - 2)
        //Time = 7.5 + .5 portals
        //Sprite Size = 2.45098
        if (leftToRight)
        {
            backTruck.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            frontTruck.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else
        {
            backTruck.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
            frontTruck.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }
        //SpriteRenderer truck = leftToRight ? frontTruck.GetComponent<SpriteRenderer>() : backTruck.GetComponent<SpriteRenderer>();
        SpriteRenderer truck = frontTruck.GetComponent<SpriteRenderer>();
        truck.gameObject.SetActive(true);
        timer = 0f;
        while (timer < 2.9252f / startMultiplier)
        {
            truck.size = new Vector2(timer * startMultiplier * .83788f, truck.size.y);
            frontTruck.position = leftToRight ? new Vector3(2f + (timer * startMultiplier * 1.2568f), 2.5f, 0f) : new Vector3(7.75f - (timer * startMultiplier * 1.2568f), 2.5f, 0f);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        truck.size = new Vector2(truck.size.y, truck.size.y);

        while (timer < 4.5748f / startMultiplier)
        {
            frontTruck.position = leftToRight ? new Vector3(2f + (timer * startMultiplier * 1.2568f), 2.5f, 0f) : new Vector3(7.75f - (timer * startMultiplier * 1.2568f), 2.5f, 0f);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        truck.gameObject.SetActive(false);
        truck = backTruck.GetComponent<SpriteRenderer>();
        truck.gameObject.SetActive(true);

        while (timer < 7.5f / startMultiplier)
        {
            truck.size = new Vector2((7.5f - timer * startMultiplier) * .83788f, truck.size.y);
            backTruck.position = leftToRight ? new Vector3(-1.67647f + (timer * startMultiplier * 1.2568f), 2.5f, 0f) : new Vector3(11.42647f - (timer * startMultiplier * 1.2568f), 2.5f, 0f);
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        truck.gameObject.SetActive(false);

        while (timer < 7.75f / startMultiplier)
        {
            leftPortal.GetComponent<SpriteRenderer>().color = color;
            rightPortal.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            color.a -= Time.deltaTime * 4f * startMultiplier;
        }
        leftPortal.SetActive(false);
        rightPortal.SetActive(false);
    }

        public void RainbowAttack()
    {
        StartCoroutine(RainbowAttackCoroutine());
    }

    IEnumerator RainbowAttackCoroutine()
    {
        isAttacking = true;
        float startMultiplier = multiplier;
        canAnimator.SetTrigger("Up");
        canAnimator.SetFloat("Multiplier", startMultiplier);
        yield return new WaitForSeconds(.5f / startMultiplier);
        int total = Random.Range(9, 13);
        int redSpawnNum = Random.Range(0, 2);
        float x = 1.75f * startMultiplier;
        for (int i = 0; i < total; i++)
        {
            bool leftSide = Random.value < .5f;
            //float x = Random.Range(1.5f, 2f) * startMultiplier;
            float y = Random.Range(1f, 4f) * startMultiplier;
            SpawnProjectile(new Vector2(1f, -4f), new Vector2(x, y), i % 2 != redSpawnNum || !leftSide, -1, true, 0f, 0f, 0f, 0f, -y * startMultiplier/2f);
            SpawnProjectile(new Vector2(8.75f, -4f), new Vector2(-x, y), i % 2 != redSpawnNum || leftSide, -1, true, 0f, 0f, 0f, 0f, -y * startMultiplier/2f);
            canAnimator.SetTrigger("Shoot");
            yield return new WaitForSeconds(Random.Range(.8f, .9f) / startMultiplier);
        }
        canAnimator.SetTrigger("Down");
        yield return new WaitForSeconds(1.5f / startMultiplier);
        isAttacking = false;
    }

    public void DumpRandomAttack()
    {
        StartCoroutine(DumpRandomAttackCoroutine());
    }

    IEnumerator DumpRandomAttackCoroutine()
    {
        isAttacking = true;
        float startMultiplier = multiplier;
        binAnimator.SetTrigger("Start");
        binAnimator.SetFloat("Multiplier", startMultiplier);
        yield return new WaitForSeconds(.5f / startMultiplier);
        Vector2 velocity = new Vector2(0f, -2.5f * startMultiplier);
        int ranNotSpawn = Random.Range(2, 6);
        int total = Random.Range(22, 30);
        int redSpawnNum = Random.Range(0, 3);
        for (int i = 0; i < total; i++)
        {
            SpawnProjectile(new Vector2(Random.Range(2.5f, 7.25f), 2f), velocity, i % 3 != redSpawnNum, -1, false, Random.Range(-135f, 135f));
            yield return new WaitForSeconds(Random.Range(.3f, .425f) / startMultiplier);
        }
        binAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1f / startMultiplier);
        isAttacking = false;
    }

    public void DumpAccelorationAttack()
    {
        StartCoroutine(DumpAccelorationAttackCoroutine());
    }

    IEnumerator DumpAccelorationAttackCoroutine()
    {
        isAttacking = true;
        float startMultiplier = multiplier;
        binAnimator.SetTrigger("Start");
        binAnimator.SetFloat("Multiplier", startMultiplier);
        yield return new WaitForSeconds(.5f / startMultiplier);
        Vector2 velocity = new Vector2(0f, -1.5f * startMultiplier);
        int ranNotSpawn = Random.Range(2, 6);
        int total = Random.Range(22, 30);
        int redSpawnNum = Random.Range(0, 3);
        for (int i = 0; i < total; i++)
        {
            SpawnProjectile(new Vector2(Random.Range(2.5f, 7.25f), 2f), velocity, i % 3 != redSpawnNum, 0, false, 0f, 0f, 0f, 0f, -multiplier * 1.75f);
            yield return new WaitForSeconds(Random.Range(.5f, .65f) / startMultiplier);
        }
        binAnimator.SetTrigger("End");
        yield return new WaitForSeconds(1f / startMultiplier);
        isAttacking = false;
    }

    public void FollowAttack()
    {
        StartCoroutine(FollowAttackCoroutine());
    }

    IEnumerator FollowAttackCoroutine()
    {
        isAttacking = true;
        float startMultiplier = multiplier;
        clouds.SetActive(true);
        float timer = 0f;
        float[] transformAmounts = new float[cloudTransforms.Length];
        for (int i = 0; i < cloudTransforms.Length; i++)
        {
            transformAmounts[i] = Random.Range(.6f, 1f);
            cloudTransforms[i].localScale = Vector3.zero;
        }
        while (timer < 1f / startMultiplier)
        {
            for (int i = 0; i < cloudTransforms.Length; i++)
            {
                cloudTransforms[i].localScale += Vector3.one * startMultiplier * transformAmounts[i] * Time.deltaTime;
            }
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }

        Vector2 velocity = new Vector2(0f, -1.5f * startMultiplier);
        int ranNotSpawn = Random.Range(2, 6);
        int redSpawnNum = Random.Range(0, 3);
        for (int i = 0; i < 15; i++)
        {
            if (i % 5 == redSpawnNum)
            {
                SpawnProjectile(new Vector2(Random.Range(3.5f, 6.25f), -5.6f), new Vector2(0f, 1.5f * startMultiplier), false, 2, true, 0f, 2f * startMultiplier, 5f);//Spawn Fly
            }
            else
            {
                SpawnProjectile(new Vector2(Random.Range(3.5f, 6.25f), 2f), new Vector2(0f, -2f * startMultiplier), true, 1, false, 0f, -1f * startMultiplier, 3f);//Spawn Umbrella
            }
            yield return new WaitForSeconds(Random.Range(.5f, .65f) / startMultiplier);
        }

        timer = 0f;
        for (int i = 0; i < cloudTransforms.Length; i++)
        {
            transformAmounts[i] = cloudTransforms[i].localScale.x;
        }
        while (timer < 1f / startMultiplier)
        {
            for (int i = 0; i < cloudTransforms.Length; i++)
            {
                cloudTransforms[i].localScale -= Vector3.one * startMultiplier * transformAmounts[i] * Time.deltaTime;
            }
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
        }
        clouds.SetActive(false);
        yield return new WaitForSeconds(1f / startMultiplier);
        isAttacking = false;
    }

    public void TruckAttack()
    {
        StartCoroutine(TruckAttackCoroutine());
    }

    IEnumerator TruckAttackCoroutine()
    {
        isAttacking = true;
        float startMultiplier = multiplier;
        float xVelocity = 2f * startMultiplier;
        int total = Random.Range(7, 10);
        truckPortals.SetActive(true);
        Color[] colors = { new Color(Random.Range(.75f, 1f), Random.Range(.75f, 1f), Random.Range(.75f, 1f), 0f), new Color(Random.Range(.75f, 1f), Random.Range(.75f, 1f), Random.Range(.75f, 1f), 0f), new Color(Random.Range(.75f, 1f), Random.Range(.75f, 1f), Random.Range(.75f, 1f), 0f) };
        float timer = 0f;
        while (timer < .25f / startMultiplier)
        {
            for (int i = 0; i < 3; i++)
            {
                portalTransforms[i*2].GetComponent<SpriteRenderer>().color = colors[i];
                portalTransforms[i*2+1].GetComponent<SpriteRenderer>().color = colors[i];
            }
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            for (int i = 0; i < 3; i++)
            {
                colors[i].a = timer * 4f * startMultiplier;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            colors[i].a = 1f;
            portalTransforms[i * 2].GetComponent<SpriteRenderer>().color = colors[i];
            portalTransforms[i * 2 + 1].GetComponent<SpriteRenderer>().color = colors[i];
        }

        int num = 0;
        while (num < total)
        {
            int redSpawnNum = Random.Range(0, 3);
            bool[] left = { Random.value < .5f, Random.value < .5f, Random.value < .5f };
            do
            {
                num++;
                redSpawnNum = (redSpawnNum + Random.Range(1, 3)) % 3;
                for (int j = 0; j < 3; j++)
                {
                    if (left[j])
                    {//Left
                        SpawnProjectile(new Vector2(portalTransforms[j * 2].transform.position.x - 1.2255f, portalTransforms[j * 2].transform.position.y), new Vector2(multiplier * 2.5f, 0f), j != redSpawnNum, 3);
                    }
                    else
                    {
                        SpawnProjectile(new Vector2(portalTransforms[j * 2 + 1].transform.position.x + 1.2255f, portalTransforms[j * 2].transform.position.y), new Vector2(multiplier * -2.5f, 0f), j != redSpawnNum, 3);
                    }
                }
                yield return new WaitForSeconds(2f / startMultiplier);
            } while (Random.value < .66f && num < total);
            yield return new WaitForSeconds(2.25f / startMultiplier);
        }

        timer = 0f;
        while (timer < .25f / startMultiplier)
        {
            for (int i = 0; i < 3; i++)
            {
                portalTransforms[i * 2].GetComponent<SpriteRenderer>().color = colors[i];
                portalTransforms[i * 2 + 1].GetComponent<SpriteRenderer>().color = colors[i];
            }
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            for (int i = 0; i < 3; i++)
            {
                colors[i].a -= Time.deltaTime * 4f * startMultiplier;
            }
        }
        truckPortals.SetActive(false);
        isAttacking = false;
    }

    private void SpawnProjectile(Vector2 spawnPosition, Vector2 direction, bool black = true, int projectileNum = -1, bool lookDir = false, float turnAmount = 0f, float followAmount = 0f, float followTime = 2f, float xAccel = 0f, float yAccel = 0f)//Either follow player or accelorate or none
    {
        GameObject projectile = projectileNum == -1 ? projectile = Instantiate(normalProjectile, spawnPosition, lookDir ? Quaternion.Euler(0, 0f, Mathf.Atan2(direction.y, direction.x) * 57.2958f - 90f) : Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)))     :     Instantiate(specialProjectiles[projectileNum], spawnPosition, lookDir ? Quaternion.Euler(0, 0f, Mathf.Atan2(direction.y, direction.x) * 57.2958f - 90f) : transform.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = direction;
        if (projectileNum == -1)
        {
            projectile.GetComponent<SpriteRenderer>().sprite = projectileSprites[Random.Range(0, projectileSprites.Length)];
            if (!black)
            {
                projectile.GetComponent<SpriteRenderer>().color = Color.red;
                projectile.GetComponent<Projectile>().blackBullet = false;
            }
        }
        else if (projectileNum == 3)
        {
            if (!black)
            {
                projectile.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                projectile.transform.GetChild(1).GetComponent<SpriteRenderer>().color = Color.red;
                projectile.GetComponent<Projectile>().blackBullet = false;
            }
            projectile.GetComponent<Projectile>().Truck();
        }
        else if (!black)
        {
            projectile.GetComponent<SpriteRenderer>().color = Color.red;
            projectile.GetComponent<Projectile>().blackBullet = false;
        }
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
