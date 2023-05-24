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
    Ground,//Redo
    Extract,//Slight
    Stir,//Debug?
    AddIngredient,
    Fire,
    Measure//Make
}

public enum ingredient
{
    Orangeberry,
    Voidshroom,
    TreeHeart,
    CookedHeart,
    SeedSkinApple,
    AppleSeeds,
    IceFlower,
    GroundUpIceFlower,
    TearGem,
    Rock,
    GroundUpRock,
    Sulfer,
    GroundUpSulfer,
    Charcoal
}

public class PotionMaker : MonoBehaviour
{
    public TextMeshProUGUI instrcutions;
    //private static ingredient[] ingredientList1 = {ingredient.Voidshroom, ingredient.GroundUpIceFlower };
    //private static ingredient[] ingredientList2 = {ingredient.Orangeberry, ingredient.AppleSeeds, ingredient.Orangeberry };
    public static ItemType goal = ItemType.Red;
    public PotionInstruction[] potionInstructions;/* = { new PotionInstruction(instruction.Ground, ingredient.IceFlower, 2f),
        new PotionInstruction(instruction.Extract, ingredient.IceFlower),
        new PotionInstruction(instruction.AddIngredient, ingredientList1), new PotionInstruction(instruction.Stir, ingredientList1, 3f),
        new PotionInstruction(instruction.AddIngredient, ingredientList2), new PotionInstruction(instruction.Stir, ingredientList2, 2f)};*/
    public Dictionary<ingredient, string> ingredientString = new Dictionary<ingredient, string>() { {ingredient.Orangeberry, "Orangeberry" }, {ingredient.Voidshroom, "Voidshroom" }, {ingredient.TreeHeart, "Tree Heart" },
        {ingredient.CookedHeart, "Cooked Tree Heart" }, { ingredient.SeedSkinApple, "Skin Seed Apple" }, { ingredient.AppleSeeds, "Apple Seeds" }, { ingredient.IceFlower, "Ice Flower" }, 
        { ingredient.GroundUpIceFlower, "Ground Up Ice Flower" }, { ingredient.TearGem, "Tear Gem" }, { ingredient.Rock, "Rock" }, { ingredient.GroundUpRock, "Ground Up Rock" }, { ingredient.Sulfer, "Sulfer" },
        { ingredient.GroundUpSulfer, "Ground Up Sulfer" }, { ingredient.Charcoal, "Charcoal" } };

    public GameObject button;
    public TextMeshProUGUI buttonText;
    private bool buttonClick = false;
    private bool timerOn = false;
    private float timer = 0f;
    public TextMeshProUGUI timerText;
    //public TextMeshProUGUI resultText;
    public List<ingredient> ingredientOrder;
    public GameObject mortarAndPestalPrefab;
    public GameObject applePrefab;
    public GameObject potPrefab;
    public GameObject firePrefab;
    public GameObject measurePrefab;
    public GameObject[] ingredientPrefabs;
    public Transform[] ingredientSpawnPositions;
    private bool activePot = false;

    void Update()
    {
        if (timerOn)
        {
            timer += Time.deltaTime;
            timerText.text = "Time: " + ((int)timer) + "s";
        }
    }

    /*public void Start()
    {
        MakePotion();
    }*/

    IEnumerator Start()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.E));
        goal = Input.GetKeyDown(KeyCode.R) ? ItemType.Red : Input.GetKeyDown(KeyCode.Y) ? ItemType.Yellow : Input.GetKeyDown(KeyCode.B) ? ItemType.Blue : ItemType.Bomb;
        MakePotion();
    }

    public void MakePotion()
    {
        if (goal == ItemType.Red)
        {
            ingredient[] ingredientList = { ingredient.CookedHeart, ingredient.Orangeberry, ingredient.Voidshroom };
            potionInstructions = new [] { new PotionInstruction(instruction.Fire, ingredient.TreeHeart, 2f),
        new PotionInstruction(instruction.AddIngredient, ingredientList), new PotionInstruction(instruction.Stir, ingredientList, 3f) };
        }
        else if (goal == ItemType.Yellow)
        {
            ingredient[] ingredientList = { ingredient.AppleSeeds, ingredient.Orangeberry, ingredient.Sulfer };
            potionInstructions = new[] { new PotionInstruction(instruction.Extract, ingredient.SeedSkinApple, 2f),
        new PotionInstruction(instruction.AddIngredient, ingredientList), new PotionInstruction(instruction.Stir, ingredientList, 3f) };
        }
        else if (goal == ItemType.Blue)
        {
            ingredient[] ingredientList = { ingredient.GroundUpIceFlower, ingredient.Voidshroom, ingredient.TearGem };
            potionInstructions = new[] { new PotionInstruction(instruction.Ground, ingredient.IceFlower, 2f),
        new PotionInstruction(instruction.AddIngredient, ingredientList), new PotionInstruction(instruction.Stir, ingredientList, 3f) };
        }
        else// if (goal == ItemType.Bomb)
        {
            ingredient[] ingredientList = { ingredient.GroundUpRock, ingredient.Charcoal, ingredient.GroundUpSulfer };
            potionInstructions = new[] { new PotionInstruction(instruction.Ground, ingredient.Rock, 2f),
        new PotionInstruction(instruction.Ground, ingredient.Sulfer, 2f), new PotionInstruction(instruction.Stir, ingredientList, 3f) };
        }
        StartCoroutine(MakePotionCoroutine());
    }

    IEnumerator MakePotionCoroutine()
    {
        //Setup
        string startText = "";
        foreach (PotionInstruction potionInstruction in potionInstructions)
        {
            if (potionInstruction.instruction == instruction.Ground)
            {
                startText += "-Ground up the ";
                /*for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    startText += ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
                }*/
                startText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower() + "\n";
            }
            else if (potionInstruction.instruction == instruction.Extract)
            {
                startText += "-Extract seeds from the seed skin apple\n";// + ingredientString[potionInstruction.ingredients[0]].ToLower();
            }
            else if (potionInstruction.instruction == instruction.Stir)
            {
                startText += "-Stir the pot to mix the ingredients together\n";
                /*for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    startText += ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
                }
                startText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower() + " in the pot\n";*/
        }
            else if (potionInstruction.instruction == instruction.AddIngredient)
            {
                startText += "-Add the ingredients to the pot\n";
                /*for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    startText += ingredientString[potionInstruction.ingredients[i]].ToLower() + ", ";
                }
                startText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower() + "to the pot\n";*/
            }
            else if (potionInstruction.instruction == instruction.Fire)
            {
                startText += "-Bake the tree heart until maximum juiciness\n";// + ingredientString[potionInstruction.ingredients[0]].ToLower();
            }
            else if (potionInstruction.instruction == instruction.Measure)
            {
                startText += "-Measure the ingredients to a 75:15:10 ratio\n";// + ingredientString[potionInstruction.ingredients[0]].ToLower();
            }
        }
        instrcutions.text = startText + "\n\nPress the \"Start\" button when you are ready.";
        yield return new WaitUntil(() => buttonClick);
        buttonClick = false;
        timerOn = true;
        buttonText.text = "Next";
        float result = 0f;
        List<string> notes = new List<string>();
        int step = 0;

        //Minigames
        foreach (PotionInstruction potionInstruction in potionInstructions)
        {
            step++;
            if (potionInstruction.instruction == instruction.Ground)
            {
                string newText = "Ground up the ";
                for (int i = 0; i < potionInstruction.ingredients.Length-1; i++)
                {
                    newText += ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
                }
                newText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length-1]].ToLower();
                newText += potionInstruction.idealResult == 1f ? " into big chunks." : potionInstruction.idealResult == 2f ? " into medium chunks." : " into a powder.";
                instrcutions.text = newText + "\n\nPress the \"Next\" button when you are done.";
                GameObject mortarAndPestal = Instantiate(mortarAndPestalPrefab);
                yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                buttonClick = false;
                float gameResult = 1f - Mathf.Abs(potionInstruction.idealResult - (GameObject.FindGameObjectsWithTag("Ingredient").Length / 10f));
                Debug.Log(gameResult);
                result += gameResult;
                if (gameResult > .8f)
                {
                    //notes.Add("Good job grounding ingredients in step " + step);
                    notes.Add("The " + potionInstruction.ingredients[0] + " was well grounded");
                }
                else if (gameResult < .2f)
                {
                    //notes.Add("Your grounding could use some work in step " + step);
                    notes.Add("When grounding the " + potionInstruction.ingredients[0] + ", you didn't make it the right size");
                }
                Destroy(mortarAndPestal);
            }
            else if (potionInstruction.instruction == instruction.Extract)
            {
                string newText = "Extract all of the brown seeds from the apple without dirtying them.";// + ingredientString[potionInstruction.ingredients[0]].ToLower();
                instrcutions.text = newText;// + "\n\nPress the \"Next\" button when you are done.";
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
                button.SetActive(false);
                //yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Seed").Length == 0);
                button.SetActive(true);
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
                result += gameResult;
                if (gameResult > .8f)
                {
                    //notes.Add("You picked out all the correct seeds in step " + step);
                    notes.Add("You got out all the seeds from the apple in good condition");
                }
                else if (gameResult < .2f)
                {
                    //notes.Add("You missed quite a few seeds in step " + step);
                    notes.Add("The seeds weren't in a good condition after extracting them");
                }
                Destroy(apple);
            }
            else if (potionInstruction.instruction == instruction.Stir)
            {
                string newText = "Stir the ";
                for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    newText += ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
                }
                newText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower();
                newText += potionInstruction.idealResult == 1f ? " until lightly mixed." : potionInstruction.idealResult == 2f ? " until moderately mixed." : " until entirely mixed together.";
                instrcutions.text = newText + "\n\nClick and drag the mixing stick with your mouse.\n\nPress the \"Next\" button when you are done.";
                float initAmount = PotIngredient.potBlue + PotIngredient.potGreen + PotIngredient.potRed;
                //GameObject mortarAndPestal = Instantiate(mortarAndPestalPrefab);
                yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                buttonClick = false;
                //float gameResult = 1f - Mathf.Abs(potionInstruction.idealResult - (GameObject.FindGameObjectsWithTag("Ingredient").Length / 10f));
                float gameResult = 1f - Mathf.Abs(potionInstruction.idealResult - ((PotIngredient.potBlue + PotIngredient.potGreen + PotIngredient.potRed) - initAmount) / potionInstruction.ingredients.Length) / 3f;
                Debug.Log(gameResult);
                result += gameResult;
                if (gameResult > .8f)
                {
                    //notes.Add("Your strring in " + step + " was well done");
                    notes.Add("Your strring was well done");
                }
                else if (gameResult < .2f)
                {
                    //notes.Add("You didn't quite stir well enough in step " + step);
                    notes.Add("You didn't quite stir well enough");
                }
            }
            else if (potionInstruction.instruction == instruction.AddIngredient)
            {
                /*string newText = "Add ";
                for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    newText += ingredientString[potionInstruction.ingredients[i]].ToLower() + " then ";
                }
                newText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower();
                newText += potionInstruction.ingredients.Length == 1 ? " into the pot." : " into the pot in order.";
                instrcutions.text = newText + "\n\nClick and drag the ingredients with your mouse.\n\nPress the \"Next\" button when you are done.";*/
                string newText = "Add ";
                for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    newText += ingredientString[potionInstruction.ingredients[i]].ToLower() + ", ";
                }
                newText += (potionInstruction.ingredients.Length == 1 ? "" : "and " ) + ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower() + " into the pot." ;
                instrcutions.text = newText;// + "\n\nClick and drag the ingredients with your mouse.\n\nPress the \"Next\" button when you are done.";
                //button.SetActive(false);
                if (!activePot)
                {
                    activePot = true;
                    Instantiate(potPrefab);
                }
                //ingredientOrder = new List<ingredient>();
                for (int i = 0; i < potionInstruction.ingredients.Length; i++)
                {
                    Instantiate(ingredientPrefabs[(int)potionInstruction.ingredients[i]], ingredientSpawnPositions[i]);
                    //yield return new WaitUntil(() => buttonClick);
                }
                button.SetActive(false);
                yield return new WaitUntil(() => AllInPot());
                button.SetActive(true);
                /*yield return new WaitUntil(() => buttonClick);
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
                result += gameResult;
                if (gameResult != 1f)
                {
                    notes.Add("Your put the ingredients in the pot in the wrong order for step " + step);
                }*/
                Debug.Log("Done");
            }
            else if (potionInstruction.instruction == instruction.Fire)
            {
                string newText = "Bake the tree heart until it has maximum juiciness.";
                instrcutions.text = newText + "\n\nPress the \"Next\" button when you are done.";
                GameObject fire = Instantiate(firePrefab);
                yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                buttonClick = false;
                Destroy(fire);
                float gameResult = 1f - GameObject.FindGameObjectWithTag("Heart").GetComponent<TreeHeart>().cookedAmount;
                Debug.Log(gameResult);
                result += gameResult;
                if (gameResult > .8f)
                {
                    notes.Add("You baked the tree heart very well");
                }
                else if (gameResult < .5f)
                {
                    notes.Add("Your didn't quite bake the tree heart how you were supposed to");
                }
            }
            else if (potionInstruction.instruction == instruction.Measure)
            {
                string newText = "Measure the crushed rock, charcoal powder, and crushed sulfer to a 75:15:10 ratio respectively.";
                instrcutions.text = newText + "\n\nPress the \"Next\" button when you are done.";
                GameObject measure = Instantiate(measurePrefab);
                yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                buttonClick = false;
                Destroy(measure);
                float gameResult = 1f - GameObject.FindGameObjectWithTag("Heart").GetComponent<TreeHeart>().cookedAmount;
                Debug.Log(gameResult);
                result += gameResult;
                if (gameResult > .8f)
                {
                    notes.Add("You baked the tree heart very well");
                }
                else if (gameResult < .5f)
                {
                    notes.Add("Your didn't quite bake the tree heart how you were supposed to");
                }
            }
            //yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
            //buttonClick = false;
            yield return new WaitForFixedUpdate();
        }

        //Result
        float timeResult = timer / 60f;
        Debug.Log("Result: " + result + ", Time: " + timeResult + ", Final: " + (result * timeResult));
        result *= timeResult;
        buttonText.text = "Done";
        Debug.Log("Done");
    }

    public bool AllInPot()
    {
        GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
        foreach(GameObject ingredient in ingredients)
        {
            if (!ingredient.GetComponent<PotIngredient>().inWater)
            {
                return false;
            }
        }
        return true;
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
