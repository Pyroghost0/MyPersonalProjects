/* Caleb Kahn
 * Creator
 * Assignment 6 (Hard)
 * Abstract class that when called creats a spawnable object
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Creator : MonoBehaviour
{
    public abstract void SpawnObject(Transform referencePosition);
}
