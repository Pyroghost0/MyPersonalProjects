/* Caleb Kahn
 * PotionMaker
 * Assignment 11 (Hard)
 * Guides the potion making process
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum instruction
{
    Ground,//Redo
    Extract,
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

    public GameObject raycast;
    public GameObject button;
    public TextMeshProUGUI buttonText;
    private bool buttonClick = false;
    private bool timerOn = false;
    private float timer = 0f;
    public TextMeshProUGUI timerText;
    //public TextMeshProUGUI resultText;
    public List<ingredient> ingredientOrder;
    public GameObject[] mortarIngredientPrefabs;
    public GameObject mortarAndPestalPrefab;
    public GameObject applePrefab;
    public GameObject potPrefab;
    public GameObject firePrefab;
    public GameObject measurePrefab;
    public GameObject[] ingredientPrefabs;
    public Transform[] ingredientSpawnPositions;
    private bool activePot = false;

    public GameObject resultMenu;
    public TextMeshProUGUI timerResultText;
    public Image[] stars;
    public Sprite[] starSprites;
    public Image resultPotion;
    public Sprite[] resultPotionSprites;
    public TextMeshProUGUI resultPotionText;
    public TextMeshProUGUI notesText;
    public GameObject endButton;

    void Update()
    {
        if (timerOn && Time.timeScale == 1f)
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
        if (goal == ItemType.Free)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.E));
            goal = Input.GetKeyDown(KeyCode.R) ? ItemType.Red : Input.GetKeyDown(KeyCode.Y) ? ItemType.Yellow : Input.GetKeyDown(KeyCode.B) ? ItemType.Blue : ItemType.Bomb;
            //instrcutions.text = "Make a " + (goal == ItemType.Red ? "red potion" : goal == ItemType.Yellow ? "yellow potion" : goal == ItemType.Blue ? "blue potion" : "bomb");
            //yield return new WaitUntil(() => Input.GetKey(KeyCode.Space));
        }
        MakePotion();
    }

    public void MakePotion()
    {
        if (goal == ItemType.Red)
        {
            ingredient[] ingredientList = { ingredient.CookedHeart, ingredient.Orangeberry, ingredient.Voidshroom };
            potionInstructions = new [] { new PotionInstruction(instruction.Fire, ingredient.TreeHeart, 2f),
        new PotionInstruction(instruction.AddIngredient, ingredientList), new PotionInstruction(instruction.Stir, ingredientList, 3f) };
            resultPotion.sprite = resultPotionSprites[0];
        }
        else if (goal == ItemType.Yellow)
        {
            ingredient[] ingredientList = { ingredient.AppleSeeds, ingredient.Orangeberry, ingredient.Sulfer };
            potionInstructions = new[] { new PotionInstruction(instruction.Extract, ingredient.SeedSkinApple, 2f),
        new PotionInstruction(instruction.AddIngredient, ingredientList), new PotionInstruction(instruction.Stir, ingredientList, 3f) };
            resultPotion.sprite = resultPotionSprites[1];
        }
        else if (goal == ItemType.Blue)
        {
            ingredient[] ingredientList = { ingredient.GroundUpIceFlower, ingredient.Voidshroom, ingredient.TearGem };
            potionInstructions = new[] { new PotionInstruction(instruction.Ground, ingredient.IceFlower, 2f),
        new PotionInstruction(instruction.AddIngredient, ingredientList), new PotionInstruction(instruction.Stir, ingredientList, 2f) };
            resultPotion.sprite = resultPotionSprites[2];
        }
        else// if (goal == ItemType.Bomb)
        {
            ingredient[] ingredientList = { ingredient.GroundUpRock, ingredient.Charcoal, ingredient.GroundUpSulfer };
            potionInstructions = new[] { new PotionInstruction(instruction.Ground, ingredient.Rock, 3f),
        new PotionInstruction(instruction.Ground, ingredient.Sulfer, 3f), new PotionInstruction(instruction.Measure, ingredientList) };
            resultPotion.sprite = resultPotionSprites[3];
        }
        StartCoroutine(MakePotionCoroutine());
    }

    IEnumerator MakePotionCoroutine()
    {
        //Setup
        ShowInstructions();
        instrcutions.text = instrcutions.text + "\n\nPress the \"Start\" button when you are ready.";
        yield return new WaitUntil(() => buttonClick);
        buttonClick = false;
        timerOn = true;
        buttonText.text = "Next";
        float result = 1f;
        List<string> notes = new List<string>();
        int step = 0;

        //Minigames
        foreach (PotionInstruction potionInstruction in potionInstructions)
        {
            step++;
            if (potionInstruction.instruction == instruction.Ground)
            {
                string newText = "Grind up the ";
                for (int i = 0; i < potionInstruction.ingredients.Length-1; i++)
                {
                    newText += ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
                }
                newText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length-1]].ToLower();
                newText += potionInstruction.idealResult == 1f ? " into big chunks." : potionInstruction.idealResult == 2f ? " into medium chunks." : " into a powder.";
                instrcutions.text = newText + "\n\nPress the \"Next\" button when you are done.";
                GameObject mortarAndPestal = Instantiate(mortarAndPestalPrefab);
                Instantiate(mortarIngredientPrefabs[(int)potionInstruction.ingredients[0]]).transform.parent = mortarAndPestal.transform;
                yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                buttonClick = false;
                Debug.Log(MortarIngredient.total);
                Debug.Log(MortarIngredient.fullTotal);
                float gameResult = Mathf.Max(1f - Mathf.Abs(potionInstruction.idealResult - (MortarIngredient.total * 3f / MortarIngredient.fullTotal)), 0f); ;
                Debug.Log(gameResult);
                result *= gameResult;
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
                //Destroy(mortarAndPestal);
                GameObject[] powders = GameObject.FindGameObjectsWithTag("Falling Dust");
                foreach (GameObject powder in powders)
                {
                    powder.transform.parent = mortarAndPestal.transform;
                }
                GameObject.FindGameObjectWithTag("Pestle").GetComponent<Pestle>().enabled = false;
                GameObject.FindGameObjectWithTag("Pestle").GetComponent<Collider2D>().enabled = false;
                GameObject.FindGameObjectWithTag("Pestle").GetComponent<Rigidbody2D>().simulated = false;
                StartCoroutine(Hide(mortarAndPestal.transform));
                yield return new WaitForSeconds(1.5f);
            }
            else if (potionInstruction.instruction == instruction.Extract)
            {
                string newText = "Extract all of the brown seeds from the apple, and leave the red ones.";// + ingredientString[potionInstruction.ingredients[0]].ToLower();
                instrcutions.text = newText + "\n\nPress the \"Next\" button when you are done.";
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
                //button.SetActive(false);
                SpriteRenderer appleSprite = GameObject.FindGameObjectWithTag("Apple").GetComponent<SpriteRenderer>();
                float tempTimer = 0f;
                while (tempTimer < 1f)
                {
                    appleSprite.color = new Color(1f, 1f, 1f, 1f - tempTimer);
                    tempTimer += Time.deltaTime / 1.5f;
                    yield return new WaitForFixedUpdate();
                }
                appleSprite.color = Color.clear;
                yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                buttonClick = false;
                //yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Seed").Length == 0);
                //button.SetActive(true);
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
                gameResult = Mathf.Max(0f, gameResult);
                Debug.Log(gameResult);
                result *= gameResult;
                if (gameResult > .8f)
                {
                    //notes.Add("You picked out all the correct seeds in step " + step);
                    notes.Add("You got out all the right seeds from the apple");
                }
                else if (gameResult < .8f)
                {
                    //notes.Add("You missed quite a few seeds in step " + step);
                    notes.Add("The didn't get all the right seeds from the apple");
                }
                //Destroy(apple);
                StartCoroutine(Hide(GameObject.FindGameObjectWithTag("Apple").transform.parent));
                yield return new WaitForSeconds(1.5f);
            }
            else if (potionInstruction.instruction == instruction.Stir)
            {
                string newText = "Stir the ";
                for (int i = 0; i < potionInstruction.ingredients.Length - 1; i++)
                {
                    newText += ingredientString[potionInstruction.ingredients[i]].ToLower() + " & ";
                }
                newText += ingredientString[potionInstruction.ingredients[potionInstruction.ingredients.Length - 1]].ToLower();
                newText += potionInstruction.idealResult == 1f ? " until lightly mixed together." : potionInstruction.idealResult == 2f ? " until mostly mixed together." : " until entirely mixed together.";
                instrcutions.text = newText + "\n\nPress the \"Next\" button when you are done.";
                float initAmount = PotIngredient.potBlue + PotIngredient.potGreen + PotIngredient.potRed;
                //GameObject mortarAndPestal = Instantiate(mortarAndPestalPrefab);
                yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                buttonClick = false;
                //float gameResult = 1f - Mathf.Abs(potionInstruction.idealResult - (GameObject.FindGameObjectsWithTag("Ingredient").Length / 10f));
                float gameResult = 0;
                GameObject[] ingredients = GameObject.FindGameObjectsWithTag("Ingredient");
                for (int i = 0; i < 3; i++)
                {
                    gameResult += i < ingredients.Length ? 1f - ingredients[i].GetComponent<PotIngredient>().size : 1f;
                    //Debug.Log(ingredients[i].name + ": "+ ingredients[i].GetComponent<PotIngredient>().size );
                    //Debug.Log(i < ingredients.Length ? 1f - ingredients[i].GetComponent<PotIngredient>().size : 1f);
                }
                gameResult = Mathf.Max(1f - Mathf.Abs(potionInstruction.idealResult - gameResult), 0f);
                Debug.Log(gameResult);
                result *= gameResult;
                if (gameResult > .8f)
                {
                    //notes.Add("Your strring in " + step + " was well done");
                    notes.Add("You stirred how you were supposed to");
                }
                else if (gameResult < .2f)
                {
                    //notes.Add("You didn't quite stir well enough in step " + step);
                    notes.Add("You didn't quite stir the ingredients right");
                }
                GameObject.FindGameObjectWithTag("Laddle").GetComponent<Laddle>().enabled = false;
                StartCoroutine(Hide(GameObject.FindGameObjectWithTag("Pot Water").transform.parent));
                foreach (GameObject ingredient in ingredients)
                {
                    ingredient.GetComponent<ParticleSystem>().Stop();
                    ingredient.GetComponent<ParticleSystem>().Clear();
                    StartCoroutine(Hide(ingredient.transform));
                }
                yield return new WaitForSeconds(1.5f);
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
                    if (goal == ItemType.Red)
                    {
                        PotIngredient.potRed = .5f;
                        PotIngredient.potGreen = .125f;
                        PotIngredient.potBlue = .5f;
                    }
                    else if (goal == ItemType.Yellow)
                    {
                        PotIngredient.potRed = .5f;
                        PotIngredient.potGreen = .25f;
                        PotIngredient.potBlue = .25f;
                    }
                    else if (goal == ItemType.Blue)
                    {
                        PotIngredient.potRed = .125f;
                        PotIngredient.potGreen = .5f;
                        PotIngredient.potBlue = .5f;
                    }
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
                //Destroy(fire);
                float gameResult = GameObject.FindGameObjectWithTag("Heart").GetComponent<TreeHeart>().cookedAmount > 1f ? 2 - GameObject.FindGameObjectWithTag("Heart").GetComponent<TreeHeart>().cookedAmount : GameObject.FindGameObjectWithTag("Heart").GetComponent<TreeHeart>().cookedAmount;
                Debug.Log(gameResult);
                result *= gameResult;
                if (gameResult > .8f)
                {
                    notes.Add("You baked the tree heart very well");
                }
                else if (gameResult < .5f)
                {
                    notes.Add("Your didn't quite bake the tree heart how you were supposed to");
                }
                GameObject.FindGameObjectWithTag("Heart").GetComponent<TreeHeart>().enabled = false;
                StartCoroutine(Hide(fire.transform));
                yield return new WaitForSeconds(1.5f);
            }
            else if (potionInstruction.instruction == instruction.Measure)
            {
                string newText = "Measure the crushed rock, charcoal powder, and crushed sulfer to a 75:15:10 ratio respectively.";
                instrcutions.text = newText + "\n\nPress the \"Next\" button when you are done.";
                GameObject measure = Instantiate(measurePrefab);
                yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
                buttonClick = false;
                //Destroy(measure);
                GameObject[] dusts = GameObject.FindGameObjectsWithTag("Stop Dust");//Ignore falling ones
                int rocks = 0;
                int charcoal = 0;
                int sulfer = 0;
                foreach (GameObject dust in dusts)
                {
                    if (dust.GetComponent<Dust>() != null)
                    {
                        if (dust.GetComponent<Dust>().ingredient == ingredient.GroundUpRock)
                        {
                            rocks++;
                        }
                        else if (dust.GetComponent<Dust>().ingredient == ingredient.Charcoal)
                        {
                            charcoal++;
                        }
                        else
                        {
                            sulfer++;
                        }
                        dust.transform.parent = measure.transform;
                    }
                }
                int total = rocks + charcoal + sulfer;
                Debug.Log("Rocks: " + rocks + ", Charcoal " + charcoal + ", Sulfer: " + sulfer);
                float gameResult = Mathf.Sqrt( Mathf.Max( Mathf.Min( Mathf.Min( ((rocks*100f) / (total*75f)) > 1f ? 2f - ((rocks * 100f) / (total * 75f)) : ((rocks * 100f) / (total * 75f)), 
                    ((charcoal * 100f) / (total * 15f)) > 1f ? 2f - ((charcoal * 100f) / (total * 15f)) : ((charcoal * 100f) / (total * 15f))), ((sulfer * 100f) / (total * 10f)) > 1f ? 2f - ((sulfer * 100f) / (total * 10f)) : ((sulfer * 100f) / (total * 10f))), 0f));
                Debug.Log(gameResult);
                result *= gameResult;
                if (gameResult > .8f)
                {
                    notes.Add("You measured the ingredients into a good ratio");
                }
                else if (gameResult < .5f)
                {
                    notes.Add("You didn't quite measure the ingredients well enough");
                }
                dusts = GameObject.FindGameObjectsWithTag("Falling Dust");
                foreach (GameObject dust in dusts)
                {
                    dust.transform.parent = measure.transform;
                }
                foreach (Bottle bottle in Bottle.bottles)
                {
                    bottle.enabled = false;
                }
                StartCoroutine(Hide(measure.transform));
                yield return new WaitForSeconds(1.5f);
            }
            //yield return new WaitUntil(() => buttonClick);//Disable and move to next instruction
            //buttonClick = false;
            yield return new WaitForFixedUpdate();
        }

        //Result
        if (goal == ItemType.Bomb)
        {
            result *= 2f / 3f;
        }
        button.SetActive(false);
        resultMenu.SetActive(true);
        ShowInstructions();
        timerOn = false;
        timerResultText.text = "Time: " + ((int)timer) + "s";
        float timeResult = Mathf.Min(10f / timer, .5f) + 1f;
        Debug.Log("Result: " + result + ", Time: " + timeResult + ", Final: " + (result * timeResult));
        int star = (int)Mathf.Min(result * 12f, 10f);
        Debug.Log("Star: " + star);
        for (int i = 0; i < star; i++)
        {
            resultPotionText.text = "x" + ((int)((i + 1) / 10f * UpgradeStations.potionAmount));
            for (int j = 0; j < 4; j++)
            {
                stars[i / 2].sprite = starSprites[((i % 2) * 4) + j];
                yield return new WaitForSeconds(.08f);
            }
        }
        yield return new WaitForSeconds(.5f);
        if (star != 10)
        {
            Color color = timerResultText.color;
            float tempTimer = 0f;
            while (tempTimer < 1f)
            {
                timerResultText.color = new Color(Mathf.Min(color.r - tempTimer, 0f), Mathf.Min(color.g - tempTimer, 0f), Mathf.Min(color.b - tempTimer, 0f), 1f - tempTimer);
                timerResultText.transform.localScale = Vector3.one * (1 + tempTimer);
                timerResultText.rectTransform.anchoredPosition = new Vector2(-150f * tempTimer - 150f, -100f * tempTimer - 25f);
                tempTimer += Time.deltaTime / 1.5f;
                yield return new WaitForFixedUpdate();
            }
            timerResultText.gameObject.SetActive(false);
            result *= timeResult;
            int temp = star;
            star = (int)Mathf.Min(result * 12f, 10f);
            Debug.Log("Star: " + star);
            for (int i = temp; i < star; i++)
            {
                resultPotionText.text = "x" + ((int)((i + 1) / 10f * UpgradeStations.potionAmount));
                for (int j = 0; j < 4; j++)
                {
                    stars[i / 2].sprite = starSprites[((i % 2) * 4) + j];
                    yield return new WaitForSeconds(.08f);
                }
            }
            yield return new WaitForSeconds(.5f);
        }
        notesText.text = notes.Count > 0 ? notes[Random.Range(0, notes.Count)] : "Average all around";
        endButton.SetActive(true);
        Player.inventoryProgress[goal == ItemType.Red ? 3 : goal == ItemType.Yellow ? 4 : goal == ItemType.Blue ? 5 : 2] += (ushort)(star / 10f * UpgradeStations.potionAmount);
    }

    public void Back()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().LoadFromPotion();
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
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().shortButtonSound.Play();
        buttonClick = true;
    }

    public void ShowInstructions()
    {
        string startText = "";
        foreach (PotionInstruction potionInstruction in potionInstructions)
        {
            if (potionInstruction.instruction == instruction.Ground)
            {
                startText += "-Grind up the ";
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
        instrcutions.text = startText;
    }

    IEnumerator Hide(Transform scene)
    {
        raycast.SetActive(true);
        float timer = 0f;
        while(timer < 1.5f)
        {
            scene.position +=  new Vector3(200f * Mathf.Pow(timer, 3f) * Time.deltaTime, 0f);
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Destroy(scene.gameObject);
        raycast.SetActive(false);
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
