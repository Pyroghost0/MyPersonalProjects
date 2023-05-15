/* Caleb Kahn
 * PrepareIngredients
 * Assignment 11 (Hard)
 * Controls the grounding and extracting function of the ingredients
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareIngredients : MonoBehaviour
{
    public PotionMaker potionMaker;
    public GameObject mortarAndPestalPrefab;
    public GameObject applePrefab;

    public void Extract()
    {
        StartCoroutine(ExtractCoroutine());
    }

    IEnumerator ExtractCoroutine()
    {
        GameObject apple = Instantiate(applePrefab);
        GameObject[] seeds = GameObject.FindGameObjectsWithTag("Seed");
        int[] reds = { Random.Range(0, 3), Random.Range(3, 6), Random.Range(6, 9), Random.Range(9, 12), Random.Range(12, 15) };
        for (int i = 0; i < seeds.Length; i++)
        {
            if (reds[i / 3] == i)
            {
                seeds[i].GetComponent<Seed>().SetRed();
            }
            else
            {
                seeds[i].GetComponent<Seed>().SetBrown();
            }
        }
        yield return new WaitUntil(() => potionMaker.buttonClick);
        float gameResult = 1f;
        for (int i = 0; i < seeds.Length; i++)
        {
            if (seeds[i] == null)
            {
                if (reds[i / 3] == i)//If red was taken
                {
                    gameResult -= .2f;
                }
            }
            else if (reds[i / 3] != i)//If brown still exists
            {
                gameResult -= .2f;
            }
        }
        Debug.Log(gameResult);
        potionMaker.result += gameResult;
        if (gameResult > .8f)
        {
            potionMaker.notes.Add("You picked out all the correct seeds in step " + potionMaker.step);
        }
        else if (gameResult < .2f)
        {
            potionMaker.notes.Add("You missed quite a few seeds in step " + potionMaker.step);
        }
        Destroy(apple);
    }

    public void Ground(PotionInstruction potionInstruction)
    {
        StartCoroutine(GroundCoroutine(potionInstruction));
    }

    IEnumerator GroundCoroutine(PotionInstruction potionInstruction)
    {
        GameObject mortarAndPestal = Instantiate(mortarAndPestalPrefab);
        yield return new WaitUntil(() => potionMaker.buttonClick);
        float gameResult = 1f - Mathf.Abs(potionInstruction.idealResult - (GameObject.FindGameObjectsWithTag("Ingredient").Length / 25f));
        Debug.Log(gameResult);
        potionMaker.result += gameResult;
        if (gameResult > .8f)
        {
            potionMaker.notes.Add("Good job grounding ingredients in step " + potionMaker.step);
        }
        else if (gameResult < .2f)
        {
            potionMaker.notes.Add("Your grounding could use some work in step " + potionMaker.step);
        }
        Destroy(mortarAndPestal);
    }
}
