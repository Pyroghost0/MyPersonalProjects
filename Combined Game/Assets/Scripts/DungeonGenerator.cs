using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public static TerrainType[,] terrain = new TerrainType[15, 7];
    public GameObject[] prefabs;
    public float[] rarity;
    private const float startX = -7.5f;
    private const float startY = -.5f;

    // Start is called before the first frame update
    void Start()
    {
        terrain[7, 0] = TerrainType.Unavailable;
        Player.inventoryProgress[22]++;
        MustSpawn(prefabs[0]);
        MustSpawn(prefabs[1]);
        int numSpawners = (int)Random.Range(Mathf.Pow(Player.inventoryProgress[22], 1.06f), Mathf.Pow(Player.inventoryProgress[22], 1.09f)) -1;
        for (int i = 0; i < numSpawners; i++)
        {
            MustSpawn(prefabs[2]);
        }
        //rarity[2] = Player.inventoryProgress[22];
        //Debug.Log(Player.inventoryProgress[22]);
        GenerateTerrain(Player.inventoryProgress[22]);
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
