/* Caleb Kahn
 * SpawnableObject
 * Assignment 6 (Hard)
 * Creates a spawnable object from an enemy
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpawnableObject : MonoBehaviour
{
    protected abstract void OnTriggerStay2D(Collider2D collision);
}
