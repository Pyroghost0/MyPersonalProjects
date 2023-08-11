using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum enemyType { 
    Empty = 0,
    Skeleton = 1,
    Buddy = 2
}

public class Enemy : MonoBehaviour
{
    /*[Header("General (Header)")]
    [SerializeField] private string privateName;
    public enemyType enemyType;
    public int damage;
    [Range(0, 100)]
    public int speed;
    [SerializeField] [Range(0, 100)] private int health;

    [Header("Other Stuff")]
    [Tooltip("Range the enemy can see the player in")] public float playerSightRangeTOOLTIP;
    [TextArea(3, 10)] public string descriptionTextArea;*/
    public EnemyData enemyData;
    public float sightRange = 5f;
    public Transform[] patrolPoints;
    public Mesh arrowMesh;//Resources.Load<Mesh>("ArrowMesh")
    public Mesh cubeMesh;

    private void Start()
    {
        Debug.Log(enemyData._name);
    }

    //OnDrawGizmos will show when not selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, .25f);
        Gizmos.DrawSphere(transform.position, sightRange);
    }

    private void OnDrawGizmos()
    {
        //Points drawn first are drawn over
        for (int i = 0; i < patrolPoints.Length - 1; i++)
        {
            Gizmos.color = Color.cyan;
            //Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            Gizmos.DrawMesh(cubeMesh, patrolPoints[i].position + (patrolPoints[i + 1].position - patrolPoints[i].position) / 2f,
                Quaternion.LookRotation(patrolPoints[i].position - patrolPoints[i + 1].position), new Vector3(.25f, .25f, (patrolPoints[i + 1].position - patrolPoints[i].position).magnitude));
        }
        DrawPoints();
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (i != 0)
            {
                Gizmos.color = Color.green;
                Quaternion rotation = Quaternion.LookRotation(patrolPoints[i].position - patrolPoints[i - 1].position) * Quaternion.Euler(-90f, -90f, 0f);
                Vector3 position = patrolPoints[i].position + (patrolPoints[i - 1].position - patrolPoints[i].position).normalized;
                Gizmos.DrawMesh(arrowMesh, position, rotation, Vector3.one * .2f);
            }
            if (i != patrolPoints.Length-1)
            {
                Gizmos.color = Color.green;
                Quaternion rotation = Quaternion.LookRotation(patrolPoints[i].position - patrolPoints[i + 1].position) * Quaternion.Euler(-90f, -90f, 0f);
                Vector3 position = patrolPoints[i].position + (patrolPoints[i + 1].position - patrolPoints[i].position).normalized;
                Gizmos.DrawMesh(arrowMesh, position, rotation, Vector3.one * .2f);
            }
        }
    }

    public static float Difficulty(int health, int speed, int damage, float range)
    {
        return ((health * speed) + (damage * range)) / 2500f;
    }

    private void DrawPoints()
    {
        foreach (Transform point in patrolPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(point.position, .25f);
        }
    }
}

[System.Serializable]
public class EnemyAbility
{
    public string _name;
    public int damage;
}
