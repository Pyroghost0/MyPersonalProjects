/* Caleb Kahn
 * ChargeGun
 * Assignment 2 (Hard)
 * This gun type charges before it fires the bullet
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeGun : Gun
{
    //Fires the gun at mouse position with specified bullet type by charging
    protected override IEnumerator Fire()
    {
        float timer = 0f;
        while (Input.GetKey(KeyCode.Mouse0) && this.enabled)
        {
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        if (timer > 2.5f)
        {
            timer = 2.5f;
        }
        Ray r = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out RaycastHit hit))
        {
            Vector3 pos = hit.point;
            pos.z = 0;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, pos, bulletPrefab.transform.rotation);
            bullet.transform.localScale = Mathf.Sqrt(Mathf.Pow(2.8f * timer, 2f)/3f) * Vector3.one;
            //Debug.Log(bullet.transform.localScale.magnitude + "\n" + timer);
            bulletClass.Shoot(bullet);
        }
    }
}
