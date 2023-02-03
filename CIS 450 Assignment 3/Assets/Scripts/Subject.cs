/* Caleb Kahn
 * Subject
 * Assignment 3 (Hard)
 * Interface that sends updates to observers
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Subject
{
    public void AddObserver(Observer observer);
    public void RemoveObserver(Observer observer);
    public void UpdateObservers(bool tileOn, bool moved);
}
