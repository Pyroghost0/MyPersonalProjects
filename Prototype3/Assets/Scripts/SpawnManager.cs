/* Caleb Kahn
 * Assignment 4
 * Spawns obsticles every 2 seconds
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject obstaclePrefab;

    private Vector3 spawnPosition = new Vector3(25, 0, 0);
    private float startDelay = 2f;
    private float repeatRate = 2f;

    private PlayerControler playerControlerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        playerControlerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (!playerControlerScript.gameOver) {
            Instantiate(obstaclePrefab, spawnPosition, obstaclePrefab.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
