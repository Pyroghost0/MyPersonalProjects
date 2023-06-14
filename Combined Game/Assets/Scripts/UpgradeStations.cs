using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
//using UnityEditor;
/*using System.Linq;
using System.Runtime.Serialization;*/

public enum StationType { 
    Potion,
    Heal,
    Damage,
    UnfoundDefence,
    Defence,
    Fire
}

public class UpgradeStations : MonoBehaviour, InteractableObject
{
    public const float timeDiff = 480f;
    public StationType stationType;
    public SpriteRenderer spriteRenderer;
    public GameObject build;
    public GameObject popup;
    public string title;
    [TextArea(4, 10)] public string explaination;//Min 4 lines, max 10 lines
    public UpgradeItems buildAmount;
    public TextMeshProUGUI buildText;
    public UpgradeItems[] upgradeItems;
    public Transform[] inventory;
    public Button[] buttons;
    public Image fireProgress;
    private Coroutine fireCoroutine;
    public static short potionAmount = 5;

    public AudioSource audioSource;

    /*#if UNITY_EDITOR

        [CustomEditor(typeof(UpgradeStations))]
        public class UpgradeEditor : Editor
        {
            bool fold = true;

            //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                /*UpgradeStations player = (UpgradeStations)serializedObject.targetObject;
                fold = EditorGUILayout.Foldout(fold, "Player Stats");
                if (fold)
                {
                    EditorGUILayout.LabelField("Type, Current, Start, Min, Max");
                    foreach (StatBlock stat in player.playerStats)
                    {
                        GUILayout.BeginHorizontal("box");
                        UpgradeStations.stats statType = (UpgradeStations.stats)EditorGUILayout.EnumPopup(stat.stat);
                        stat.stat = statType;
                        float current = EditorGUILayout.FloatField(stat.current);
                        stat.current = current;
                        float start = EditorGUILayout.FloatField(stat.start);
                        stat.start = start;
                        float min = EditorGUILayout.FloatField(stat.min);
                        stat.min = min;
                        float max = EditorGUILayout.FloatField(stat.max);
                        stat.max = max;
                        GUILayout.EndHorizontal();
                    }
                }*/
    /*UpgradeStations upgradeStations = (UpgradeStations)serializedObject.targetObject;
    fold = EditorGUILayout.Foldout(fold, "Player Stats");
    if (fold)
    {
        EditorGUILayout.LabelField("Upgrade Resources");
        foreach (ItemAmount itemAmount in upgradeStations.itemAmounts)
        {
            GUILayout.BeginHorizontal("box");
            //UpgradeStations.stats statType = (UpgradeStations.stats)EditorGUILayout.EnumPopup(stat.stat);
            //stat.stat = statType;
            //float current = EditorGUILayout.FloatField(stat.current);
            itemAmount.itemType = (ItemType)EditorGUILayout.EnumPopup(itemAmount.itemType);
            itemAmount.amount = EditorGUILayout.IntField(itemAmount.amount);
            GUILayout.EndHorizontal();
        }
    }

    serializedObject.ApplyModifiedProperties();
}
}

#endif*/

    void Start()
    {
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 10f);
        if (stationType == StationType.Potion)
        {
            if ((Player.inventoryProgress[17] / 1) % 2 == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                build.SetActive(false);
                if ((Player.inventoryProgress[17] / 2) % 2 != 0 && (Player.inventoryProgress[17] / 4) % 2 != 0)
                {
                    Player.instance.buildButton.SetActive(false);
                }
            }
        }
        else if (stationType == StationType.Heal)
        {
            if ((Player.inventoryProgress[17] / 2) % 2 == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                build.SetActive(false);
            }
        }
        else if (stationType == StationType.Damage)
        {
            if ((Player.inventoryProgress[17] / 4) % 2 == 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                build.SetActive(false);
            }
        }
        else if (stationType == StationType.UnfoundDefence)
        {
            if ((Player.inventoryProgress[17] / 8) % 2 == 1)
            {
                Destroy(gameObject);
            }
        }
        else if (stationType == StationType.Defence)
        {
            if ((Player.inventoryProgress[17] / 8) % 2 == 0)
            {
                gameObject.SetActive(false);
            }
        }
        else// if (stationType == StationType.Fire)
        {
            if (Player.inventoryProgress[21] % 2 == 1)
            {
                GetComponent<Animator>().SetBool("Fire", true);
                fireCoroutine = StartCoroutine(FireOff());
            }
        }
    }

    public void UpdateBuildButton()
    {
        string text = "";
        for (int j = 0; j < buildAmount.itemAmount.Length; j++)
        {
            text += "x" + buildAmount.itemAmount[j].amount + " " + PlayerInventory.itemName[buildAmount.itemAmount[j].itemType] + (j + 1 < buildAmount.itemAmount.Length ? "\n" : "");
        }
        buildText.text = text;
        build.GetComponent<Button>().interactable = true;
        for (int i = 0; i < buildAmount.itemAmount.Length; i++)
        {
            if (buildAmount.itemAmount[i].amount > Player.inventoryProgress[(int)buildAmount.itemAmount[i].itemType])
            {
                build.GetComponent<Button>().interactable = false;
            }
        }
    }

    IEnumerator FireOff()
    {
        // Time Passed = (Player.floatTime - ((Player.inventoryProgress[21] / 16) - (((Player.inventoryProgress[21] / 4) % 4) * 1440f)))
        //Debug.Log(Player.floatTime - ((Player.inventoryProgress[21] / 16) - (((Player.inventoryProgress[21] / 4) % 4) * 1440f)));
        audioSource.Play();
        yield return new WaitForSeconds(Mathf.Max( (timeDiff - (Player.floatTime - ((Player.inventoryProgress[21] / 16) - (((Player.inventoryProgress[21] / 4) % 4) * 1440f)))), 0f) / Player.timeMultiplier);
        Debug.Log("Fire off");
        audioSource.Stop();
        GetComponent<Animator>().SetBool("Fire", false);
    }

    public void Inventory()
    {
        for (int i = 0; i < 14; i++)
        {
            inventory[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = "x" + Player.inventoryProgress[i + 2];
        }
    }

    public void Build()
    {
        Player.instance.BuildMode();
        if (Player.inventoryProgress[17] % 2 != 0 && (Player.inventoryProgress[17] / 2) % 2 != 0 && (Player.inventoryProgress[17] / 4) % 2 != 0)
        {
            Player.instance.buildButton.SetActive(false);
        }
        for (int i = 0; i < buildAmount.itemAmount.Length; i++)
        {
            Player.inventoryProgress[(int)buildAmount.itemAmount[i].itemType] -= (ushort)buildAmount.itemAmount[i].amount;
        }
        build.SetActive(false);
        PlayerInventory.instance.UpdateTexts();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Tutorial(title, explaination);
    }

    public void Cancel()
    {
        Player.instance.interaction = null;
        Time.timeScale = 1f;
        popup.SetActive(false);
    }

    public void Interact()
    {
        if (!popup.activeSelf)
        {
            if (stationType == StationType.UnfoundDefence)
            {
                Player.instance.interaction = null;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().Tutorial(title, explaination);
                Player.inventoryProgress[17] += 8;
                Destroy(gameObject);
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    buttons[i].interactable = HasResources(i);
                    string text = "";
                    if (upgradeItems[i].itemAmount.Length > 0)
                    {
                        if (upgradeItems[i].itemAmount[0].itemType == ItemType.Free)
                        {
                            text = "Free";
                        }
                        else
                        {
                            for (int j = 0; j < upgradeItems[i].itemAmount.Length; j++)
                            {
                                text += "x" + upgradeItems[i].itemAmount[j].amount + " " + PlayerInventory.itemName[upgradeItems[i].itemAmount[j].itemType] + (j + 1 < upgradeItems[i].itemAmount.Length ? "\n" : "");
                            }
                        }
                    }
                    buttons[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
                }
                if (stationType == StationType.Heal)
                {
                    buttons[0].interactable = buttons[0].interactable && !Player.healed;
                    buttons[1].interactable = buttons[1].interactable && !Player.healed;
                    buttons[2].interactable = buttons[2].interactable && !Player.healed;
                    buttons[3].interactable = buttons[3].interactable && !Player.healed;
                }
                else if (stationType == StationType.Damage)
                {
                    buttons[0].interactable = buttons[0].interactable && (Player.inventoryProgress[19] / 1) % 2 == 0;
                    buttons[1].interactable = buttons[1].interactable && (Player.inventoryProgress[19] / 2) % 2 == 0;
                    buttons[2].interactable = buttons[2].interactable && (Player.inventoryProgress[19] / 4) % 2 == 0;
                    buttons[3].interactable = buttons[3].interactable && (Player.inventoryProgress[19] / 8) % 2 == 0;
                }
                else if (stationType == StationType.Defence)
                {
                    Debug.Log(Player.inventoryProgress[20]);
                    buttons[0].interactable = buttons[0].interactable && (Player.inventoryProgress[20] / 1) % 2 == 0;
                    buttons[1].interactable = buttons[1].interactable && (Player.inventoryProgress[20] / 2) % 2 == 0;
                    buttons[2].interactable = buttons[2].interactable && (Player.inventoryProgress[20] / 4) % 2 == 0;
                    buttons[3].interactable = buttons[3].interactable && (Player.inventoryProgress[20] / 8) % 2 == 0;
                }
                else if (stationType == StationType.Fire)
                {
                    //0 = [1] has [0] none, 1 = [0] small [1] big, 2-3 [0] today [1] yesterday, [3] 2+ days, 4-15 = time
                    if (Player.inventoryProgress[21] % 2 == 1)
                    {
                        fireProgress.fillAmount = Mathf.Min((Player.floatTime - ((Player.inventoryProgress[21] / 16) - (((Player.inventoryProgress[21] / 4) % 4) * 1440f))), timeDiff) / timeDiff;
                        buttons[0].interactable = false;
                        buttons[1].interactable = fireProgress.fillAmount == 1f;
                        buttons[2].interactable = fireProgress.fillAmount != 1f;
                        buttons[3].interactable = false;
                    }
                    else
                    {
                        fireProgress.fillAmount = 0f;
                        //buttons[0].interactable = true;//Player.inventoryProgress[6] >= 3;
                        buttons[1].interactable = false;
                        buttons[2].interactable = false;
                        //buttons[3].interactable = true;//Player.inventoryProgress[6] >= 10;
                    }
                }
                popup.SetActive(true);
                Inventory();
                Time.timeScale = 0f;
            }
            /*if (stationType == StationType.Potion)
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
                    /*{

                    }
                    popup.SetActive(true);
                    Time.timeScale = 0f;*/
        }
    }

    public void Button(int num)
    {
        //if (HasResources(num))
        if (true)
        {
            //Take resources
            for (int i = 0; i < upgradeItems[num].itemAmount.Length; i++)
            {
                Player.inventoryProgress[(int)upgradeItems[num].itemAmount[i].itemType] -= (ushort)upgradeItems[num].itemAmount[i].amount;
                if (upgradeItems[num].itemAmount[i].itemType != ItemType.Free)
                {
                    PlayerInventory.instance.UpdateItem((int)upgradeItems[num].itemAmount[i].itemType);
                }
            }
            for (int i = 0; i < 4; i ++)
            {
                buttons[i].interactable = buttons[i].interactable && HasResources(i);
            }

            //Action
            if (stationType == StationType.Potion)
            {
                PotionMaker.goal = num == 0 ? ItemType.Red : num == 1 ? ItemType.Yellow : num == 2 ? ItemType.Blue : ItemType.Bomb;
                SceneManager.LoadScene("Potion");
            }
            else if (stationType == StationType.Heal)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().longButtonSound.Play();
                Player.instance.UpdateHealth((int)((Player.maxHealth / 4f) * (num + 1f)));
                Player.healed = true;
                buttons[0].interactable = false;
                buttons[1].interactable = false;
                buttons[2].interactable = false;
                buttons[3].interactable = false;
            }
            else if (stationType == StationType.Damage)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().longButtonSound.Play();
                buttons[num].interactable = false;
                Player.inventoryProgress[19] += (ushort)Mathf.Pow(2f, num);
                if (num == 0)
                {
                    Player.bulletCooldownTime = .45f;
                }
                else if (num == 1)
                {
                    potionAmount = 8;
                }
                else if (num == 2)
                {
                    Player.gatherEfficiency[1] = 2f;
                }
                else //if (num == 3)
                {
                    Player.gatherEfficiency[2] = 2f;
                }
            }
            else if (stationType == StationType.Defence)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().longButtonSound.Play();
                buttons[num].interactable = false;
                Player.inventoryProgress[20] += (ushort)Mathf.Pow(2f, num);
                Player.maxHealth++;
                Player.instance.UpdateHealth(1);
            }
            else //if (stationType == StationType.Fire)
            {
                if (num == 0)
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().shortButtonSound.Play();
                    Player.inventoryProgress[21] = (ushort)(1 + (Player.dayTime * 16)); 
                    buttons[0].interactable = false;
                    buttons[1].interactable = false;
                    buttons[2].interactable = true;
                    buttons[3].interactable = false;
                    GetComponent<Animator>().SetBool("Fire", true);
                    fireCoroutine = StartCoroutine(FireOff());
                }
                else if (num == 1)
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().longButtonSound.Play();
                    fireProgress.fillAmount = 0f;
                    Player.inventoryProgress[15] += (Player.inventoryProgress[21] / 2) % 2 == 1 ? (ushort)10 : (ushort)3;
                    PlayerInventory.instance.UpdateItem(15);
                    buttons[0].interactable = Player.inventoryProgress[6] >= 3;
                    buttons[1].interactable = false;
                    buttons[2].interactable = false;
                    buttons[3].interactable = Player.inventoryProgress[6] >= 10;
                    Player.inventoryProgress[21] = 0;
                }
                else if (num == 2)
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().shortButtonSound.Play();
                    fireProgress.fillAmount = 0f;
                    Player.inventoryProgress[6] += (Player.inventoryProgress[21] / 2) % 2 == 1 ? (ushort)10 : (ushort)3;
                    PlayerInventory.instance.UpdateItem(6);
                    Player.inventoryProgress[21] = 6;
                    buttons[0].interactable = true;
                    buttons[1].interactable = false;
                    buttons[2].interactable = false;
                    buttons[3].interactable = true;
                    GetComponent<Animator>().SetBool("Fire", false);
                    StopCoroutine(fireCoroutine);
                    audioSource.Stop();
                }
                else //if (num == 3)
                {
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().shortButtonSound.Play();
                    Player.inventoryProgress[21] = (ushort)(3 + (Player.dayTime * 16));
                    buttons[0].interactable = false;
                    buttons[1].interactable = false;
                    buttons[2].interactable = true;
                    buttons[3].interactable = false;
                    GetComponent<Animator>().SetBool("Fire", true);
                    fireCoroutine = StartCoroutine(FireOff());
                }
            }
            Inventory();
        }
    }

    public bool HasResources(int num)
    {
        for (int i = 0; i < upgradeItems[num].itemAmount.Length; i++)
        {
            if (upgradeItems[num].itemAmount[i].amount > Player.inventoryProgress[(int)upgradeItems[num].itemAmount[i].itemType])
            {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class UpgradeItems
{
    public ItemAmount[] itemAmount;
}

[System.Serializable]
public class ItemAmount
{
    public ItemType itemType;
    public int amount;
}

/*
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(UpgradeStations))]
public class ItemInfoDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // The 6 comes from extra spacing between the fields (2px each)
        return EditorGUIUtility.singleLineHeight * 4 + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.LabelField(position, label);

        var itemRect = new Rect(position.x, position.y + 18, position.width, 16);
        var amountRect = new Rect(position.x, position.y + 36, position.width, 16);
        //var genderRect = new Rect(position.x, position.y + 54, position.width, 16);

        EditorGUI.indentLevel++;

        EditorGUI.PropertyField(itemRect, property.FindPropertyRelative("itemType"));
        EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("itemAmount"));
        //EditorGUI.PropertyField(genderRect, property.FindPropertyRelative("Gender"));

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }
}
#endif*/