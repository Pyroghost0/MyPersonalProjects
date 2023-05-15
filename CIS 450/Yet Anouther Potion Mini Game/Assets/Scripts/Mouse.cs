/* Caleb Kahn
 * Mouse
 * Assignment 11 (Hard)
 * For extraction mini-game, the in-game mouse collider will move to it's position
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
