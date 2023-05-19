using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StationType { 
    Potion,
    Heal,
    Damage,
    UnfoundDefence,
    Defence,
}

public class UpgradeStations : MonoBehaviour, InteractableObject
{
    public StationType stationType;
    public GameObject popup;

    void Start()
    {
        if (stationType == StationType.Potion)
        {
            if ((Player.inventoryProgress[20] / 1) % 2 == 0)
            {
                gameObject.SetActive(false);
            }
        }
        else if (stationType == StationType.Heal)
        {
            if ((Player.inventoryProgress[20] / 2) % 2 == 0)
            {
                gameObject.SetActive(false);
            }
        }
        else if (stationType == StationType.Damage)
        {
            if ((Player.inventoryProgress[20] / 4) % 2 == 0)
            {
                gameObject.SetActive(false);
            }
        }
        else if (stationType == StationType.UnfoundDefence)
        {
            if ((Player.inventoryProgress[20] / 8) % 2 == 1)
            {
                Destroy(gameObject);
            }
        }
        else /*if (stationType == StationType.Defence)*/
        {
            if ((Player.inventoryProgress[20] / 8) % 2 == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Cancel()
    {
        if (stationType == StationType.Potion)
        {

        }
        else if (stationType == StationType.Heal)
        {

        }
        else if (stationType == StationType.Damage)
        {

        }
        else if (stationType == StationType.UnfoundDefence)
        {
            //Player.inventoryProgress[20] += 8;
            Destroy(gameObject);
        }
        else /*if (stationType == StationType.Defence)*/
        {

        }
        Time.timeScale = 1f;
        popup.SetActive(false);
    }

    public void Interact()
    {
        if (!popup.activeSelf)
        {
            if (stationType == StationType.Potion)
            {

            }
            else if (stationType == StationType.Heal)
            {

            }
            else if (stationType == StationType.Damage)
            {

            }
            else if (stationType == StationType.UnfoundDefence)
            {
                //Player.inventoryProgress[20] = 1;
                //Destroy(gameObject);
            }
            else /*if (stationType == StationType.Defence)*/
            {

            }
            popup.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
