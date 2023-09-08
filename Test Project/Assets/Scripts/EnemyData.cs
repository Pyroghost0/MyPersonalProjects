using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//To change icon for this script and children go to top right where script icon is, click arrow -> change -> find art

[CreateAssetMenu(fileName = "EnemyData_", menuName = "UnitData/Monster")]//The / in menu name will make a subfolder
public class EnemyData : ScriptableObject
{
    [Header("General (Header)")]
    [SerializeField] private string privateName = "...";
    public string _name => privateName;//Can see the name variables through this (Can't see private variables in enemy class)
    public EnemyType enemyType;
    public bool attackingEnemy;
    public int damage;
    [Seporator()]
    [Range(0, 100)]
    public int speed;
    [SerializeField] [Range(0, 100)] private int health;
    public EnemyAbility[] enemyAbilities;

    [Header("Other Stuff")]
    [Tooltip("Range the enemy can see the player in")] public float playerSightRangeTOOLTIP;
    [TextArea(3, 10)] public string descriptionTextArea;
}
