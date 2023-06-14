using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    public string title;
    [TextArea(4, 10)] public string description;
    public bool isAxe = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (isAxe)
            {
                Player.gatherEfficiency[1] = 1f;
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Tutorial(title, description, !isAxe);
            Destroy(gameObject);
        }
    }
}
