/* Caleb Kahn
 * Player
 * Assignment 3 (Hard)
 * Subject player that moves acconding to controls and changes tilemap color
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour, Subject
{
    public List<Observer> observers = new List<Observer>();
    public Tilemap tilemap;
    public float tileValue;
    public bool failed = false;
    public Animator animator;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    //Updates movement and tilemap color
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        animator.SetFloat("X", x);
        animator.SetFloat("Y", y);
        float magnitude = Mathf.Sqrt(x * x + y * y);
        if (magnitude != 0)
        {
            x *= Mathf.Abs(x) / magnitude;
            y *= Mathf.Abs(y) / magnitude;
        }
        rigidbody.velocity = new Vector2(x, y) * 2.2f;
        if (tileValue == 0f && rigidbody.velocity.magnitude > 0f)
        {
            failed = true;
            UpdateObservers(true, true);
        }
        else if (tileValue > 0f)
        {
            tileValue -= (rigidbody.velocity.magnitude/1.5f + 1f) * Time.deltaTime;
            if (tileValue > 2f)
            {
                tilemap.color = new Color((8 - (tileValue - 2)) / 8, (tileValue - 2) / 8, 0f);
            }
            else if (tileValue > 0f)
            {
                tilemap.color = (int)(tileValue / .5f) % 2 == 0? Color.red : Color.white;
            }
            else
            {
                tileValue = 0f;
                tilemap.color = Color.black;
                UpdateObservers(false, failed);
                StartCoroutine(WaitReset());
            }
        }
    }

    //Resets map after a few seconds
    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        UpdateObservers(true, failed);
        tileValue = 10f;
    }

    //Adds observers to obervers list
    public void AddObserver(Observer observer)
    {
        observers.Add(observer);
    }

    //Removes observers from obervers list
    public void RemoveObserver(Observer observer)
    {
        observers.Remove(observer);
    }

    //Updates observers state
    public void UpdateObservers(bool tileOn, bool moved)
    {
        foreach (Observer observer in observers)
        {
            observer.Change(tileOn, moved);
        }
    }

    //Determines if dead/won game
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resultText.text = "You Lose";
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resultText.text = "You Win";
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame();
        }
    }
}
