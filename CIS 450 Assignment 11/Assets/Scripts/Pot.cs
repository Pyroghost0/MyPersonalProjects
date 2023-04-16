/* Caleb Kahn
 * Pot
 * Assignment 11 (Hard)
 * Controlls pot ingredients and add them
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public List<ingredient> ingredientOrder;
    public PotionMaker potionMaker;
    public GameObject potPrefab;
    public GameObject[] ingredientPrefabs;
    public Transform[] ingredientSpawnPositions;
    public bool activePot = false;

    public void AddIngredients(PotionInstruction potionInstruction)
    {
        StartCoroutine(AddIngredientsCoroutine(potionInstruction));
    }

    IEnumerator AddIngredientsCoroutine(PotionInstruction potionInstruction)
    {
        //button.SetActive(false);
        if (!activePot)
        {
            activePot = true;
            Instantiate(potPrefab);
        }
        ingredientOrder = new List<ingredient>();
        for (int i = 0; i < potionInstruction.ingredients.Length; i++)
        {
            Instantiate(ingredientPrefabs[(int)potionInstruction.ingredients[i]], ingredientSpawnPositions[i]);
            //yield return new WaitUntil(() => buttonClick);
        }
        yield return new WaitUntil(() => potionMaker.buttonClick);
        GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
        foreach (GameObject ingredient in ingredients)
        {
            if (ingredient.layer == 7)
            {
                Destroy(ingredient);
            }
        }
        //button.SetActive(true);
        float gameResult = 1f;
        for (int i = 0; i < potionInstruction.ingredients.Length || i < ingredientOrder.Count; i++)
        {
            if (i >= potionInstruction.ingredients.Length || i >= ingredientOrder.Count || potionInstruction.ingredients[i] != ingredientOrder[i])
            {
                gameResult -= 1f / 3f;
            }
        }
        Debug.Log(gameResult);
        potionMaker.result += gameResult;
        if (gameResult != 1f)
        {
            potionMaker.notes.Add("Your put the ingredients in the pot in the wrong order for step " + potionMaker.step);
        }
    }

    public void Stir(PotionInstruction potionInstruction)
    {
        StartCoroutine(StirCoroutine(potionInstruction));
    }

    IEnumerator StirCoroutine(PotionInstruction potionInstruction)
    {
        float initAmount = PotIngredient.potBlue + PotIngredient.potGreen + PotIngredient.potRed;
        yield return new WaitUntil(() => potionMaker.buttonClick);//Disable and move to next instruction
        //float gameResult = 1f - Mathf.Abs(potionInstruction.idealResult - (GameObject.FindGameObjectsWithTag("Ingredient").Length / 10f));
        float gameResult = 1f - Mathf.Abs(potionInstruction.idealResult - (((PotIngredient.potBlue + PotIngredient.potGreen + PotIngredient.potRed) - initAmount) / potionInstruction.ingredients.Length) * 3f );
        Debug.Log(gameResult);
        potionMaker.result += gameResult;
        if (gameResult > .8f)
        {
            potionMaker.notes.Add("Your strring in " + potionMaker.step + " was well done");
        }
        else if (gameResult < .2f)
        {
            potionMaker.notes.Add("Your didn't quite stir well enough in step " + potionMaker.step);
        }
    }
}
