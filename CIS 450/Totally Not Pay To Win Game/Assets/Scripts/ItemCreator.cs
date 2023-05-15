/* Caleb Kahn
 * ItemCreator
 * Assignment 5 (Hard)
 * Creates Item Objects
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public GameObject keyPrefab;
    public GameObject commonPrefab;
    public GameObject moneyPrefab;

    //Spawns item
    public void SpawnItem(bool gacha, Vector3 spawnPosition, int moneyAmount, KeyType keyType = KeyType.None)
    {
        GameObject.FindGameObjectWithTag("ItemCreator").GetComponent<ItemFactory>().SpawnItem(gacha, spawnPosition, moneyAmount, keyType, this);
    }
}
