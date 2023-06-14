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
    public SpriteRenderer spriteRenderer;
    public GameObject[] enemies;
    public GameObject spawnPoint;
    public int numEnemies = 0;
    public int maxEnemies = 5;
    public float averageRespawnTime = 2f;
    public float spawnDistence = 20f;
    public float closeSpawnDistance = 1f;
    public int totalSpawned = 0;
    public int maxEnemiesPerCycle = -1;
    private Transform player;
    private bool currentlySpawning = false;
    public float ranSpawnRange = 1.5f;

    //public bool offsetSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        maxEnemies = (int) Mathf.Min(Mathf.Pow(1.1f, Player.inventoryProgress[22]), 3f);
        maxEnemiesPerCycle = (int) Random.Range(1f, 1.5f * Mathf.Log10(Mathf.Pow(Player.inventoryProgress[22], 2)) + 1f );
        averageRespawnTime = 100 / (Mathf.Pow(Player.inventoryProgress[22], .75f) + 10);
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
            yield return new WaitUntil(() => ((player.position - transform.position).magnitude < spawnDistence));
            yield return new WaitUntil(() => (player.position - transform.position).magnitude >= closeSpawnDistance);
            SpawnEnemy();
            if (totalSpawned == maxEnemiesPerCycle)
            {
                spriteRenderer.color = new Color(.5f, .5f, .5f);
                break;
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
        GameObject enemySpawned = Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoint.transform.position + new Vector3(Random.Range(-ranSpawnRange, ranSpawnRange), Random.Range(-ranSpawnRange, ranSpawnRange), 0f), transform.rotation);
        if (enemySpawned.GetComponent<Enemy>() == null)
        {
            enemySpawned.GetComponent<Slime>().spawner = this;
        }
        else
        {
            enemySpawned.GetComponent<Enemy>().spawner = this;
        }
        numEnemies++;
        //}
    }
}
