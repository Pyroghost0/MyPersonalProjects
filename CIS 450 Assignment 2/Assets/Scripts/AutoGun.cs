/* Caleb Kahn
 * AutoGun
 * Assignment 2 (Hard)
 * This gun type shoots bullets automatically while the player holds down the shoot button
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class AutoGun : Gun
{
    private float cooldown = 0;

    //Fires the gun at mouse position with specified bullet type every .2 seconds
    protected override IEnumerator Fire()
    {
        while (Input.GetKey(KeyCode.Mouse0) && this.enabled)
        {
            if (cooldown == 0f)
            {
                if (camera == null)
                {
                    camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                    //bulletPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Bullet.prefab", typeof(GameObject));
                    bulletPrefab = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().bulletReference;
                    bulletClass = Random.value < .5f ? (GetComponent<SpinBullet>() == null ? gameObject.AddComponent<SpinBullet>() : GetComponent<SpinBullet>()) : (GetComponent<StraightBullet>() == null ? gameObject.AddComponent<StraightBullet>() : GetComponent<StraightBullet>());
                }
                Ray r = camera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(r, out RaycastHit hit))
                {
                    Vector3 pos = hit.point;
                    pos.z = 0;
                    bulletClass.Shoot(GameObject.Instantiate(bulletPrefab, pos, bulletPrefab.transform.rotation));
                    cooldown = .2f;
                    StartCoroutine(Cooldown());
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    //Cooldown for gun between shooting gun
    IEnumerator Cooldown()
    {
        while (cooldown >= Time.deltaTime)
        {
            cooldown -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        cooldown = 0f;
    }
}
