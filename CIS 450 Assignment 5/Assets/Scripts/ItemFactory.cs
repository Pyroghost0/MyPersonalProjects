/* Caleb Kahn
 * ItemFactory
 * Assignment 5 (Hard)
 * Created Item Objects
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    //Spawns specified item
    public void SpawnItem(bool gacha, Vector3 spawnPosition, int moneyAmount, KeyType keyType, ItemCreator itemCreator)
    {
        if (gacha)
        {
            if (Random.value > .8f)
            {
                GameObject key = Instantiate(itemCreator.keyPrefab, spawnPosition, itemCreator.transform.rotation);
                key.GetComponent<Key>().keyType = keyType;
                key.GetComponent<SpriteRenderer>().color = keyType == KeyType.Yellow ? Color.yellow : keyType == KeyType.Blue ? Color.blue : Color.red;
            }
            else
            {
                Instantiate(itemCreator.commonPrefab, spawnPosition, itemCreator.transform.rotation);
            }
        }
        else
        {
            Instantiate(itemCreator.moneyPrefab, spawnPosition, itemCreator.transform.rotation).GetComponent<Money>().moneyAmount = moneyAmount;
        }
    }
}
