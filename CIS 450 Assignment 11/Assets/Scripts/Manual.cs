/* Caleb Kahn
 * Manual
 * Assignment 11 (Hard)
 * Shows the instructions
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manual : MonoBehaviour
{
    public TextMeshProUGUI instrcutions;
    public PotionMaker potionMaker;

    public void ShowInstructions()
    {
        string startText = "";
        foreach (PotionInstruction potionInstruction in potionMaker.potionInstructions)
        {
            if (potionInstruction.instruction == instruction.Ground)
            {
                startText += "-Grind up ";
                for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    startText += potionMaker.ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
                }
                startText += potionMaker.ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower() + "\n";
            }
            else if (potionInstruction.instruction == instruction.Extract)
            {
                startText += "-Extract seeds from the apple\n";// + ingredientString[potionInstruction.ingredients[0]].ToLower();
            }
            else if (potionInstruction.instruction == instruction.Stir)
            {
                startText += "-Stir the pot\n";
                /*for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    startText += ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
                }
                startText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower() + " in the pot\n";*/
            }
            else if (potionInstruction.instruction == instruction.AddIngredient)
            {
                startText += "-Add ingredients to the pot\n";
                /*for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    startText += ingredientString[potionInstruction.ingredients[i]].ToLower() + ", ";
                }
                startText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower() + "to the pot\n";*/
            }
        }
        instrcutions.text = startText + "\n\nPress the \"Start\" button when you are ready.";
    }

    public void NextStep(PotionInstruction potionInstruction)
    {
        if (potionInstruction.instruction == instruction.Ground)
        {
            string newText = "Grind ";
            for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
            {
                newText += potionMaker.ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
            }
            newText += potionMaker.ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower();
            newText += potionInstruction.idealResult == 1f ? " lightly into big chunks." : potionInstruction.idealResult == 2f ? " into medium chunks." : " into a powder.";
            instrcutions.text = newText + "\n\nClick and drag the pestle with your mouse.\n\nPress the \"Next\" button when you are done.";
        }
        else if (potionInstruction.instruction == instruction.Extract)
        {
            string newText = "Extract all of the brown seeds from the apple.";// + ingredientString[potionInstruction.ingredients[0]].ToLower();
            instrcutions.text = newText + "\n\nClick and drag the seeds with your mouse.\n\nPress the \"Next\" button when you are done.";
        }
        else if (potionInstruction.instruction == instruction.Stir)
        {
            string newText = "Stir the ";
            for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
            {
                newText += potionMaker.ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
            }
            newText += potionMaker.ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower();
            newText += potionInstruction.idealResult == 1f ? " until lightly mixed." : potionInstruction.idealResult == 2f ? " until moderately mixed." : " until entirely mixed together.";
            instrcutions.text = newText + "\n\nClick and drag the mixing stick with your mouse.\n\nPress the \"Next\" button when you are done.";
        }
        else if (potionInstruction.instruction == instruction.AddIngredient)
        {
            string newText = "Add ";
            for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
            {
                newText += potionMaker.ingredientString[potionInstruction.ingredients[i]].ToLower() + " then ";
            }
            newText += potionMaker.ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower();
            newText += potionInstruction.ingredients.Length == 1 ? " into the pot." : " into the pot in order.";
            instrcutions.text = newText + "\n\nClick and drag the ingredients with your mouse.\n\nPress the \"Next\" button when you are done.";
        }
    }
}
