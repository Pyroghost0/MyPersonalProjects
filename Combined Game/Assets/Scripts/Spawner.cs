/* Caleb Kahn
 * Spawner
 * Assignment 6 (Hard)
 * Spawns enemies
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawnPoint;
    public int numEnemies = 0;
    public int maxEnemies = 5;
    public float averageRespawnTime = 2f;
    public float spawnDistence = 20f;
    public int totalSpawned = 0;
    public int maxEnemiesPerCycle = -1;
    private Transform player;
    private bool currentlySpawning = false;
    public float ranSpawnRange = 1.5f;

    //public bool offsetSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        maxEnemies = (int) Mathf.Pow(1.1f, Player.inventoryProgress[22]);
        maxEnemiesPerCycle = (int) Mathf.Pow(1.15f, Player.inventoryProgress[22]);
        averageRespawnTime = 200 / (Player.inventoryProgress[22] + 10);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(StartSpawning());
    }

    //Enemy died
    public void EnemyDeath()
    {
        numEnemies--;
        if (!currentlySpawning)
        {
            StartCoroutine(StartSpawning());
        }
    }

    //Spawns enemies randomly
    IEnumerator StartSpawning()
    {
        currentlySpawning = true;
        while (numEnemies < maxEnemies && (maxEnemiesPerCycle == -1 || totalSpawned < maxEnemiesPerCycle))
        //while (numEnemies < maxEnemies)
        {
            yield return new WaitForSeconds(Random.Range(averageRespawnTime / 4, averageRespawnTime * 3 / 4));
            if (Mathf.Abs((player.position - transform.position).magnitude) < spawnDistence)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(Random.Range(averageRespawnTime / 4, averageRespawnTime * 3 / 4));
        }
        currentlySpawning = false;
    }

    //Restarts spawner
    public void RestartSpawner()
    {
        numEnemies = 0;
        StopAllCoroutines();
        StartCoroutine(StartSpawning());
    }

    //Spawns Enemy
    public void SpawnEnemy()
    {
        /*if (offsetSpawn)
        {
            GameObject enemySpawned = Instantiate(enemy, spawnPoint.transform.position + new Vector3(Random.Range(-ranSpawnRange, ranSpawnRange), Random.Range(-ranSpawnRange, ranSpawnRange), 0f), enemy.transform.rotation);
            enemySpawned.GetComponent<Enemy>().SetOffset();
            enemySpawned.GetComponent<Enemy>().spawner = this;
            offsetSpawn = false;
            numEnemies++;
        }
        else
        {*/
        totalSpawned++;
            GameObject enemySpawned = Instantiate(enemy, spawnPoint.transform.position + new Vector3(Random.Range(-ranSpawnRange, ranSpawnRange), Random.Range(-ranSpawnRange, ranSpawnRange), 0f), enemy.transform.rotation);
            enemySpawned.GetComponent<Enemy>().spawner = this;
            numEnemies++;
        //}
    }
}
