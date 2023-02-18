/* Caleb Kahn
 * Spawner
 * Assignment 5 (Hard)
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
    //public int maxSlimesPerCycle = -1;
    private Transform player;
    private bool currentlySpawning = false;
    public float ranSpawnRange = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
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
        //while (numEnemies < maxEnemies && (maxSlimesPerCycle == -1 || slimesSpawned < maxSlimesPerCycle))
        while (numEnemies < maxEnemies)
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
    public GameObject SpawnEnemy()
    {
        GameObject enemySpawned = Instantiate(enemy, spawnPoint.transform.position + new Vector3(Random.Range(-ranSpawnRange, ranSpawnRange), Random.Range(-ranSpawnRange, ranSpawnRange), 0f), enemy.transform.rotation);
        enemySpawned.GetComponent<Enemy>().spawner = this;
        numEnemies++;
        //audioManager.SpawnSlime();
        return enemySpawned;
    }
}
