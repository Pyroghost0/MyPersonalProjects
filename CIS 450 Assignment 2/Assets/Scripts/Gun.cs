/* Caleb Kahn
 * Gun
 * Assignment 2 (Hard)
 * Abstract class where the player can aim and shoot a random bullet type
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public abstract class Gun : MonoBehaviour
{
    public BulletBehaivior bulletClass;
    public GameObject bulletPrefab;
    public Camera camera;

    //Puts values in variables
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //bulletPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Bullet.prefab", typeof(GameObject));
        bulletPrefab = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().bulletReference;
        bulletClass = Random.value < .5f ? (GetComponent<SpinBullet>() == null ? gameObject.AddComponent<SpinBullet>() : GetComponent<SpinBullet>()) : (GetComponent<StraightBullet>() == null ? gameObject.AddComponent<StraightBullet>() : GetComponent<StraightBullet>());
        GameController gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        gc.bulletImage.sprite = bulletClass is SpinBullet ? gc.spinBulletSprite : gc.straightBulletSprite;
    }

    //Fires gun via mouse
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Fire());
        }
    }

    //Fires gun if switched to
    private void OnEnable()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartCoroutine(Fire());
        }
    }

    protected abstract IEnumerator Fire();
}
