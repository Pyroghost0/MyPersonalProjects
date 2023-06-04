using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    //Wood = 6,
    //Charcoal = 15

    public static Dictionary<ItemType, string> itemName = new Dictionary<ItemType, string>() { { ItemType.Wood, "Wood"}, { ItemType.Rock, "Rock" }, { ItemType.SeedSkinApple, "Seedskin Apple" }, { ItemType.TreeHeart, "Tree Heart" }, 
        { ItemType.Sulfer, "Sulfer" }, { ItemType.Voidshroom, "Voidshroom"}, { ItemType.IceFlower, "Iceflower"}, { ItemType.Orangeberry, "Orangeberry"}, { ItemType.TearGem, "Teargem"}, { ItemType.Charcoal, "Charcoal"}};
    public static PlayerInventory instance;
    public RectTransform InventoryTextSlider;
    public RectTransform InventoryBoxSlider;
    public TextMeshProUGUI[] inventoryTexts;
    public PlayerInventoryHelper[] playerInventoryHelpers;
    public bool[] helpersEntered;
    private bool enetered { get { return helpersEntered[0] || helpersEntered[1]; } }
    private Coroutine activeCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        for (int i = 0; i < inventoryTexts.Length; i++)
        {
            inventoryTexts[i].text = itemName[(ItemType)(i + 6)] + " x" + Player.inventoryProgress[i + 6];
        }
    }

    public void UpdateItem(int num)
    {
        inventoryTexts[num - 6].text = itemName[(ItemType)(num)] + " x" + Player.inventoryProgress[num];
    }

    public void HelperPointer(int num, bool helperEntered)
    {
        if (enetered != helperEntered && activeCoroutine == null)
        {
            helpersEntered[num] = helperEntered;
            activeCoroutine = StartCoroutine(helperEntered ? InventoryOpen() : InventoryClose());
        }
        else
        {
            helpersEntered[num] = helperEntered;
        }
    }

    IEnumerator InventoryOpen()
    {
        yield return new WaitForFixedUpdate(); 
        while (InventoryBoxSlider.anchoredPosition.x > -300 && enetered)
        {
            InventoryBoxSlider.anchoredPosition = new Vector2(InventoryBoxSlider.anchoredPosition.x - (600f * Time.deltaTime), 0f);//300 distance
            InventoryTextSlider.anchoredPosition = new Vector2(Mathf.Clamp(InventoryTextSlider.anchoredPosition.x + (100f * Time.deltaTime), -20f, 20f), 0f);//40 distance
            yield return new WaitForFixedUpdate();
        }
        if (InventoryBoxSlider.anchoredPosition.x < -300)
        {
            InventoryBoxSlider.anchoredPosition = new Vector2(-300, 0f);
        }
        if (!enetered)
        {
            activeCoroutine = StartCoroutine(InventoryClose());
        }
        else
        {
            activeCoroutine = null;
        }
    }

    IEnumerator InventoryClose()
    {
        yield return new WaitForFixedUpdate();
        while (InventoryBoxSlider.anchoredPosition.x < 0 && !enetered)
        {
            InventoryBoxSlider.anchoredPosition = new Vector2(InventoryBoxSlider.anchoredPosition.x + (600f * Time.deltaTime), 0f);
            InventoryTextSlider.anchoredPosition = new Vector2(Mathf.Clamp(InventoryTextSlider.anchoredPosition.x - (100f * Time.deltaTime), -20f, 20f), 0f);
            yield return new WaitForFixedUpdate();
        }
        if (InventoryBoxSlider.anchoredPosition.x > 0)
        {
            InventoryBoxSlider.anchoredPosition = new Vector2(0, 0f);
        }
        if (enetered)
        {
            activeCoroutine = StartCoroutine(InventoryOpen());
        }
        else
        {
            activeCoroutine = null;
        }
    }
}
