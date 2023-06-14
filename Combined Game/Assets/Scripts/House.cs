using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class House : MonoBehaviour
{
    public static bool isMade = true;
    public TextMeshProUGUI woodAmountText;
    public GameObject upgradeAmountObject;
    public GameObject doorObject;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private const int divisionAmount = 2;
    private int spriteNum = 0;
    private int spriteGoal = 0;
    private bool active = false;

    public AudioSource constructionSounds;

    // Start is called before the first frame update
    void Start()
    {
        if (Player.inventoryProgress[16] == 0 && Player.floatTime == 240f)
        {
            spriteRenderer.sprite = sprites[0];
            doorObject.SetActive(false);
            upgradeAmountObject.SetActive(true);
            isMade = false;
        }
        else
        {
            StartCoroutine(ChangeHouse());
        }
    }

    IEnumerator Build()
    {
        active = true;
        constructionSounds.Play();
        float speed = Player.instance.speed;
        Player.instance.speed = 0f;
        while (spriteNum < spriteGoal)
        {
            //Debug.Log("Change Later");
            yield return new WaitForSeconds(.05f);
            spriteNum++;
            spriteRenderer.sprite = sprites[spriteNum];
            yield return new WaitForSeconds(.05f);
        }
        if (spriteNum == 64)
        {
            yield return new WaitForSeconds(.05f);
            StartCoroutine(ChangeHouse());
            isMade = true;
            doorObject.SetActive(true);
        }
        Player.instance.speed = speed;
        active = false;
        constructionSounds.Stop();
    }

    IEnumerator ChangeHouse()
    {
        while (true)
        {
            spriteRenderer.sprite = sprites[65];
            yield return new WaitUntil(() => Player.floatTime >= 240f);
            spriteRenderer.sprite = sprites[66];
            yield return new WaitUntil(() => Player.floatTime < 240f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!isMade && collision.collider.CompareTag("Player") && Player.inventoryProgress[6] != 0)
        {
            int added = Mathf.Min(Player.inventoryProgress[6], (64 - spriteGoal) / divisionAmount);
            spriteGoal += added * divisionAmount;
            Player.inventoryProgress[6] -= (ushort)added;
            if (spriteGoal == 64)
            {
                upgradeAmountObject.SetActive(false);
            }
            else
            {
                woodAmountText.text = "x" + ((64 - spriteGoal) / divisionAmount);
            }
            PlayerInventory.instance.UpdateItem(6);
            if (!active)
            {
                StartCoroutine(Build());
            }
        }
    }
}
