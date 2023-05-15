/* Caleb Kahn
 * MinionState
 * Assignment 9 (Hard)
 * State that decides the behaivior of the minion
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinionState : MonoBehaviour
{
    public abstract void StartCollecting();
    public abstract void Return();
}
