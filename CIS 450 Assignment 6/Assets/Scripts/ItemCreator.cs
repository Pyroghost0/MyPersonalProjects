/* Caleb Kahn
 * ItemCreator
 * Assignment 6 (Hard)
 * Creates Item Objects
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : Creator
{
    //public Sprite[] itemSprites;
    public GameObject itemPrefab;

    //Spawns item
    public override void SpawnObject(Transform referencePosition)
    {
        Instantiate(itemPrefab, referencePosition.position, transform.rotation);
    }
}
