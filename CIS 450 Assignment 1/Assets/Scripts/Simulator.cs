/* Caleb Kahn
 * Simulator
 * Assignment 1 (Easy)
 * Simulates the enemies' actions
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public Enemy[] enemies = { new Bat(), new Bat(), new Goblin(), new Goblin(), new Robot() };
    public Attack[] attakers = { new Bat(), new Bat(), new Goblin(), new Goblin(), new Robot() };
    public Goblin goblin = new Goblin();
    public Bat bat = new Bat();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("***1***\n");
        goblin.Move();
        bat.Move();
        bat.Attack(goblin);
        goblin.Attack(bat);
        bat.Attack(goblin);
        goblin.Move();

        Debug.Log("***2***\n");
        foreach (Enemy enemy in enemies)
        {
            enemy.Move();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Move();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (Attack attaker in attakers)
            {
                attaker.Attack(goblin);
            }
        }
    }
}
