/* Caleb Kahn
 * BodyCreator
 * Assignment 6 (Hard)
 * Creates a objects of the body
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCreator : Creator
{
    public Sprite[] bodySprites;
    public Sprite[] enemySprites;
    public GameObject bodyPrefab;

    //Spawns 2 body objects where enemy was
    public override void SpawnObject(Transform referencePosition)
    {
        Sprite enemySprite = referencePosition.GetComponent<SpriteRenderer>().sprite;
        if (enemySprite == enemySprites[0] || enemySprite == enemySprites[1] || enemySprite == enemySprites[2])
        {
            Instantiate(bodyPrefab, referencePosition.position + new Vector3(0f, .125f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[0];
            Instantiate(bodyPrefab, referencePosition.position + new Vector3(0f, -.2f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[1]);
        }
        else if (enemySprite == enemySprites[3] || enemySprite == enemySprites[4] || enemySprite == enemySprites[5])
        {
            Instantiate(bodyPrefab, referencePosition.position + new Vector3(.16f, 0f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[2];
            Instantiate(bodyPrefab, referencePosition.position + new Vector3(-.24f, 0f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[3]);
        }
        else if (enemySprite == enemySprites[6] || enemySprite == enemySprites[7] || enemySprite == enemySprites[8])
        {
            Instantiate(bodyPrefab, referencePosition.position + new Vector3(0f, -.13f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[4];
            Instantiate(bodyPrefab, referencePosition.position + new Vector3(0f, .25f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[5]);
        }
        else// if (enemySprite == enemySprites[9] || enemySprite == enemySprites[10] || enemySprite == enemySprites[11])
        {
            Instantiate(bodyPrefab, referencePosition.position + new Vector3(-.16f, 0f, 0f), transform.rotation).GetComponent<SpriteRenderer>().sprite = bodySprites[6];
            Instantiate(bodyPrefab, referencePosition.position + new Vector3(.24f, 0f, 0f), transform.rotation).GetComponent<Body>().SetHead(bodySprites[7]);
        }
    }
}
