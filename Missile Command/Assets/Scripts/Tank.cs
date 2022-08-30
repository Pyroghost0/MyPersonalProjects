using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public Transform spawnPosition;
    public GameObject bulletPrefab;

    // Update is called once per frame
    void Update()
    {
        Vector3 position = camera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        Quaternion rotation = Quaternion.Euler(0, 0f, Mathf.Atan2((position - transform.position).y, (position - transform.position).x) * 57.2958f - 90f);
        transform.rotation = rotation;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Bullet bullet = Instantiate(bulletPrefab, spawnPosition.position, rotation).GetComponent<Bullet>();
            bullet.goal = position;
        }
        if (Input.GetKey(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
