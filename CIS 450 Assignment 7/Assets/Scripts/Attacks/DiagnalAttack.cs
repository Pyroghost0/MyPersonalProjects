/* Caleb Kahn
 * DiagnalAttack
 * Assignment 7 (Hard)
 * Attack type where projectiles spawn in a diagnal pattern
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagnalAttack : Attack
{
    private AttackExecutor attackExecutor;
    //Stack<Vector3> positionHistory = new Stack<Vector3>();

    public DiagnalAttack(AttackExecutor executor)
    {
        attackExecutor = executor;
    }

    public void StartAttack()
    {
        //positionHistory.Push(moveObject.GetCurrentPosition());
        //return attackExecutor.DiagnalAttack();
        attackExecutor.DiagnalAttack();
    }

    /*public void Undo()
    {
        //You could consider Undoing the move left with a move to the right...
        //moveObject.MoveRight();

        //Instead, we will assign the Vector3 position in our positionHistory stack to our gameObject
        if (positionHistory.Count != 0)
        {
            moveObject.gameObject.transform.position = positionHistory.Pop();
        }

    }*/
}
