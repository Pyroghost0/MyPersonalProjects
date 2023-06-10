using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonGenerator : MonoBehaviour
{
    public TerrainType[,] terrain = new TerrainType[15, 7];
    public GameObject[] prefabs;
    public float[] rarity;
    public TextMeshProUGUI floorText;
    public RectTransform floorTextTransform;
    private const float startX = -7.5f;
    private const float startY = -.5f;

    // Start is called before the first frame update
    void Start()
    {
        //Doors
        terrain[7, 0] = TerrainType.Unavailable;
        terrain[6, 6] = TerrainType.Unavailable;
        terrain[7, 6] = TerrainType.Unavailable;
        terrain[8, 6] = TerrainType.Unavailable;
        terrain[13, 6] = TerrainType.Unavailable;
        terrain[14, 6] = TerrainType.Unavailable;
        Player.inventoryProgress[22]++;
        MustSpawn(prefabs[0]);
        MustSpawn(prefabs[1]);
        int numSpawners = (int)Random.Range(7f * Mathf.Log10(Mathf.Pow(Player.inventoryProgress[22], .75f)) + 2.5f, 7f * Mathf.Log10(Player.inventoryProgress[22]) + 2.5f);
        for (int i = 0; i < numSpawners; i++)
        {
            MustSpawn(prefabs[2]);
        }
        //rarity[2] = Player.inventoryProgress[22];
        //Debug.Log(Player.inventoryProgress[22]);
        GenerateTerrain(Mathf.Sqrt(Mathf.Pow( Player.inventoryProgress[22], 1.15f  )));
        StartCoroutine(DungeonFloorTextCoroutine());
    }

    IEnumerator DungeonFloorTextCoroutine()
    {
        floorText.gameObject.SetActive(true);
        floorText.text = "Floor " + Player.inventoryProgress[22];
        float timer = 0f;
        while (timer < 1f)
        {
            floorTextTransform.anchoredPosition = new Vector2(0f, Mathf.Sin(timer * .5f * Mathf.PI) * -50f);
            floorText.alpha = timer;
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        timer = 0f;
        floorTextTransform.anchoredPosition = new Vector2(0f, -50f);
        floorText.alpha = 1f;
        yield return new WaitForSeconds(2f);
        while (timer < 1f)
        {
            floorTextTransform.anchoredPosition = new Vector2(0f, -50f + (Mathf.Sin(timer * .5f * Mathf.PI) * 50f));
            floorText.alpha = 1f - timer;
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        floorText.gameObject.SetActive(false);
    }

    public void MustSpawn(GameObject prefab)
    {
        while (true)
        {
            int i = Random.Range(0, 15);
            int j = Random.Range(0, 7);
            if (terrain[i, j] == TerrainType.Available)
            {
                Instantiate(prefab, new Vector3(startX + i, startY + j), transform.rotation);
                terrain[i, j] = TerrainType.SmallResource;
                break;
            }
        }
    }

    public void GenerateTerrain(float multiplier)
    {
        //for (int i = 0; i < terrain.Length; i++)
        for (int i = 0; i < terrain.GetLength(0); i++)
        {
            for (int j = 0; j < terrain.GetLength(1); j++)
            {
                //Debug.Log(i + " " + j);
                if (terrain[i, j] == TerrainType.Available)
                {
                    for (short k = 3; k < prefabs.Length; k++)
                    {
                        //Debug.Log(k + ", " + Random.value + ", Rarety: " + rarity[k] + ", M" + multiplier + ", " + rarity[k] * multiplier);
                        if (Random.value <= rarity[k] * multiplier)
                        {
                            Instantiate(prefabs[k], new Vector3(startX + i, startY + j), transform.rotation);
                            terrain[i, j] = TerrainType.SmallResource;
                            break;
                        }
                    }
                }
            }
        }
        /*if (multiplier != 1)
        {
            GenerateTerrain(multiplier - 1);
        }*/
    }
}
