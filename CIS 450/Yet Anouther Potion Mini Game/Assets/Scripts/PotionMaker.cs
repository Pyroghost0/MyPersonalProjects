/* Caleb Kahn
 * PotionMaker
 * Assignment 11 (Hard)
 * Guides the potion making process
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum instruction
{
    Ground,
    Extract,
    Stir,
    AddIngredient
}

public enum ingredient
{
    IceFlower,
    GroundUpIceFlower,
    SkinSeedApple,
    AppleSeeds,
    Orangeberry,
    Voidshroom
}

public class PotionMaker : MonoBehaviour
{
    private static ingredient[] ingredientList1 = {ingredient.Voidshroom, ingredient.GroundUpIceFlower };
    private static ingredient[] ingredientList2 = {ingredient.Orangeberry, ingredient.AppleSeeds, ingredient.Orangeberry };
    public PotionInstruction[] potionInstructions = { new PotionInstruction(instruction.Ground, ingredient.IceFlower, 2f),
        new PotionInstruction(instruction.Extract, ingredient.IceFlower),
        new PotionInstruction(instruction.AddIngredient, ingredientList1), new PotionInstruction(instruction.Stir, ingredientList1, 3f),
        new PotionInstruction(instruction.AddIngredient, ingredientList2), new PotionInstruction(instruction.Stir, ingredientList2, 2f)};
    public Dictionary<ingredient, string> ingredientString = new Dictionary<ingredient, string>() { { ingredient.IceFlower, "Ice Flower" }, { ingredient.GroundUpIceFlower, "Ground Up Ice Flower" },
        { ingredient.SkinSeedApple, "Skin Seed Apple" }, { ingredient.AppleSeeds, "Apple Seeds" }, {ingredient.Orangeberry, "Orangeberry" }, {ingredient.Voidshroom, "Voidshroom" } };

    public GameObject button;
    public TextMeshProUGUI buttonText;
    public bool buttonClick = false;
    private bool timerOn = false;
    private float timer = 0f;
    public TextMeshProUGUI timerText;
    //public TextMeshProUGUI resultText;

    public Manual manual;
    public PrepareIngredients prepareIngredients;
    public Pot pot;
    public int step = 0;
    public float result = 0f;
    public List<string> notes = new List<string>();

    void Update()
    {
        if (timerOn)
        {
            timer += Time.deltaTime;
            timerText.text = ((int)timer) + "s";
        }
    }

    public void Start()
    {
        MakePotion();
    }

    public void MakePotion()
    {
        StartCoroutine(MakePotionCoroutine());
    }

    IEnumerator MakePotionCoroutine()
    {
        //Setup
        manual.ShowInstructions();
        yield return new WaitUntil(() => buttonClick);
        buttonClick = false;
        timerOn = true;
        buttonText.text = "Next";

        //Minigames
        foreach (PotionInstruction potionInstruction in potionInstructions)
        {
            step++;
            Debug.Log("Step " + step);
            if (potionInstruction.instruction == instruction.Ground)
            {
                prepareIngredients.Ground(potionInstruction);
            }
            else if (potionInstruction.instruction == instruction.Extract)
            {
                prepareIngredients.Extract();
            }
            else if (potionInstruction.instruction == instruction.Stir)
            {
                pot.Stir(potionInstruction);
            }
            else if (potionInstruction.instruction == instruction.AddIngredient)
            {
                pot.AddIngredients(potionInstruction);
            }
            manual.NextStep(potionInstruction);
            yield return new WaitUntil(() => buttonClick);
            buttonClick = false;
            yield return new WaitForFixedUpdate();
        }

        //Result
        float timeResult = 60f / timer;
        Debug.Log("Result: " + result + ", Time: " + timeResult + ", Final: " + (result * timeResult));
        result *= timeResult;
        if (timer > 90f)
        {
            notes.Add("Slow potion making");
        }
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().resultDescriptionText.text = notes.Count > 0f ? notes[Random.Range(0, notes.Count)] : "Average job all around";
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().EndGame(result > 1f);
    }


    public void NextButton()
    {
        buttonClick = true;
    }
}

public class PotionInstruction
{
    public instruction instruction;
    public ingredient[] ingredients;
    public float idealResult;

    public PotionInstruction(instruction inst, ingredient ing, float result = 0f)
    {
        instruction = inst;
        ingredient[] ings = { ing };
        ingredients = ings;
        idealResult = result;
    }

    public PotionInstruction(instruction inst, ingredient[] ings, float result = 0f)
    {
        instruction = inst;
        ingredients = ings;
        idealResult = result;
    }
}
