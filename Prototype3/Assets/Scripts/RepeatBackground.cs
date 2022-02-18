/* Caleb Kahn
 * Assignment 4
 * Repeats the background so that it is endless
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{

    private Vector3 startPosition;
    private float repeatWidth;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        repeatWidth = transform.GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < startPosition.x - repeatWidth) {
            transform.position = startPosition;
        }
    }
}
