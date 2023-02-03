/* Caleb Kahn
 * Observer
 * Assignment 3 (Hard)
 * Interface that gets updates from subjects
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer
{
    public void Change(bool tileOn, bool moved);
}
