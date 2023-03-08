/* Caleb Kahn
 * Attacker
 * Assignment 7 (Hard)
 * Invokeer that starts attacks once they finish
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    public AttackExecutor attackExecutor;
    private Attack[] attacks;
    //private Attack moveLeft;

    //A stack of commands to keep track of the history of commands
    //public Stack<Command> commandHistory;

    // Initialize commands and our stack of commands using Awake or Start
    void Start()
    {
        attacks = new Attack[1];
        attacks[0] = new DiagnalAttack(attackExecutor);
        //moveLeft = new DiagnalAttack(attackExecutor);
        //commandHistory = new Stack<Command>();
        StartCoroutine(ConstantAttack());
    }

    IEnumerator ConstantAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(StartAttack());
            yield return new WaitForSeconds(1f);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            moveLeft.Execute();
            commandHistory.Push(moveLeft);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (commandHistory.Count != 0)
            {
                //pop the last command off our stack
                Command lastCommand = commandHistory.Pop();

                //call Undo() on the last command
                lastCommand.Undo();
            }
        }
    }*/

    public float StartAttack(int num = -1)
    {
        if (num == -1)
        {
            num = Random.Range(0, attacks.Length);
        }
        return attacks[num].StartAttack();
    }


}
