using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    Unavailable = 0b_1010,
    Covered = 0b_0001,
    Available = 0b_0000,
    BigResource = 0b_0011,
    CoveredResource = 0b_0111,
    SmallResource = 0b_0010
}

public class TerrainGenerator : MonoBehaviour
{
    public static TerrainType[,] terrain = new TerrainType[50, 38];
    public static short[,] resourceTypes;
    public static TerrainGenerator instance;
    public GameObject[] prefabs;
    public TerrainType[] prefabTypes;
    public float[] rarity;
    public GameObject[] coveredResourcePrefabs;
    public float[] coveredRarity;
    private const float startX = -24.5f;
    private const float startY = -29.5f;
    public static bool regenerate = false;
    public static Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = UnityEngine.SceneManagement.SceneManager.GetSceneByName("Game").GetRootGameObjects()[0].transform;
        //Debug.Log(terrain.Length);//900
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            if (resourceTypes == null)
            {
                resourceTypes = new short[50, 38];
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 38; j++)
                    {
                        resourceTypes[i, j] = -1;
                    }
                }
                terrain = new TerrainType[50, 38];
                GenerateUnavailableTerrain();
                MustSpawnTrees(100);
                GenerateTerrain(25);
                GenerateTerrain(25);
                GenerateTerrain(25);
            }
            else if (regenerate)
            {
                resourceTypes = Data.Terrain();
                terrain = new TerrainType[50, 38];
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 38; j++)
                    {
                        terrain[i, j] = resourceTypes[i, j] == -1 ? TerrainType.Available : resourceTypes[i, j] < prefabs.Length ? prefabTypes[resourceTypes[i, j]] : TerrainType.CoveredResource;
                    }
                }
                for (int i = 0; i < 50; i++)
                {
                    for (int j = 0; j < 38; j++)
                    {
                        if (terrain[i, j] == TerrainType.BigResource)
                        {
                            for (int startI = i - 1; startI < i + 2; startI++)
                            {
                                for (int startJ = j - 1; startJ < j + 2; startJ++)
                                {
                                    //Debug.Log(startI + " " + startJ);
                                    if (startI >= 0 && startI < terrain.GetLength(0) && startJ >= 0 && startJ < terrain.GetLength(1) && terrain[startI, startJ] == TerrainType.Available)
                                    {
                                        terrain[startI, startJ] = TerrainType.Covered;
                                    }
                                }
                            }
                        }
                    }
                }
                GenerateUnavailableTerrain();
                regenerate = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void GenerateUnavailableTerrain()
    {
        //House
        for (int i = 20; i < 30; i++)
        {
            for (int j = 29; j < 35; j++)
            {
                terrain[i, j] = TerrainType.Unavailable;
            }
        }
        terrain[24, 28] = TerrainType.Unavailable;
        terrain[25, 28] = TerrainType.Unavailable;

        //Dungeon
        for (int i = 40; i < 47; i++)
        {
            for (int j = 6; j < 12; j++)
            {
                terrain[i, j] = TerrainType.Unavailable;
            }
        }
        terrain[42, 5] = TerrainType.Unavailable;
        terrain[43, 5] = TerrainType.Unavailable;
        terrain[44, 5] = TerrainType.Unavailable;

        //Ruins
        for (int i = 4; i < 10; i++)
        {
            for (int j = 2; j < 6; j++)
            {
                terrain[i, j] = TerrainType.Unavailable;
            }
        }

        //Campfire
        terrain[17, 28] = TerrainType.Unavailable;
        terrain[17, 29] = TerrainType.Unavailable;
        terrain[16, 28] = TerrainType.Unavailable;
        terrain[16, 29] = TerrainType.Unavailable;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateTerrain(50);
        }
    }

    public void MustSpawnTrees(int num)
    {
        int spawned = 0;
        while (spawned <= num)
        {
            int i = Random.Range(0, 50);
            int j = Random.Range(0, 38);
            if (CheckIfClear(i, j))
            {
                Instantiate(prefabs[0], new Vector3(startX + i, startY + j), transform.rotation, parent).transform.GetChild(0).GetComponent<Resource>().SetRowColumn(i, j);
                for (int startI = i - 1; startI < i + 2; startI++)
                {
                    for (int startJ = j - 1; startJ < j + 2; startJ++)
                    {
                        if (startI >= 0 && startI < terrain.GetLength(0) && startJ >= 0 && startJ < terrain.GetLength(1) && terrain[startI, startJ] != TerrainType.Unavailable)
                        {
                            terrain[startI, startJ] = TerrainType.Covered;
                        }
                    }
                }
                terrain[i, j] = TerrainType.BigResource;
                resourceTypes[i, j] = 0;
                spawned++;
            }
        }
    }

    public void GenerateTerrain(int multiplier)
    {
        //for (int i = 0; i < terrain.Length; i++)
        for (int i = 0; i < terrain.GetLength(0); i++)
        {
            for (int j = 0; j < terrain.GetLength(1); j++)
            {
                //Debug.Log(i + " " + j);
                if (terrain[i, j] == TerrainType.Available)
                {
                    for (short k = 0; k < prefabs.Length; k++)
                    {
                        if (Random.value <= rarity[k] * multiplier)
                        {
                            if (prefabTypes[k] == TerrainType.BigResource)
                            {
                                if (CheckIfClear(i, j))
                                {
                                    Instantiate(prefabs[k], new Vector3(startX + i, startY + j), transform.rotation, parent).transform.GetChild(0).GetComponent<Resource>().SetRowColumn(i, j);
                                    for (int startI = i-1; startI < i+2; startI++)
                                    {
                                        for (int startJ = j-1; startJ < j+2; startJ++)
                                        {
                                            if (startI >= 0 && startI < terrain.GetLength(0) && startJ >= 0 && startJ < terrain.GetLength(1) && terrain[startI, startJ] != TerrainType.Unavailable)
                                            {
                                                terrain[startI, startJ] = TerrainType.Covered;
                                            }
                                        }
                                    }
                                    terrain[i, j] = TerrainType.BigResource;
                                    resourceTypes[i, j] = k;
                                    break;
                                }
                            }
                            else
                            {
                                Instantiate(prefabs[k], new Vector3(startX + i, startY + j), transform.rotation, parent).transform.GetChild(0).GetComponent<Resource>().SetRowColumn(i, j);
                                terrain[i, j] = TerrainType.SmallResource;
                                resourceTypes[i, j] = k;
                                break;
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < terrain.GetLength(0); i++)
        {
            for (int j = 0; j < terrain.GetLength(1); j++)
            {
                if (terrain[i, j] == TerrainType.Covered)
                {
#pragma warning disable CS0162 // Unreachable code detected
                    for (int k = 0; k < coveredResourcePrefabs.Length; k++)
#pragma warning restore CS0162 // Unreachable code detected
                    {
                        if (Random.value <= coveredRarity[k] * multiplier)
                        {
                            Instantiate(coveredResourcePrefabs[k], new Vector3(startX + i, startY + j), transform.rotation, parent).transform.GetChild(0).GetComponent<Resource>().SetRowColumn(i, j);
                            terrain[i, j] = TerrainType.CoveredResource;
                            resourceTypes[i, j] = (short)(k + prefabs.Length);
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

    public void Load()
    {
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < 38; j++)
            {
                if (resourceTypes[i, j] != -1)
                {
                    Instantiate(resourceTypes[i, j] < prefabTypes.Length ? prefabs[resourceTypes[i, j]] : coveredResourcePrefabs[resourceTypes[i, j] - prefabs.Length], new Vector3(startX + i, startY + j), transform.rotation, parent).transform.GetChild(0).GetComponent<Resource>().SetRowColumn(i, j);
                }
            }
        }
    }

    public void RemoveResource(int row, int column)
    {
        if (terrain[row, column] == TerrainType.BigResource)
        {
            terrain[row, column] = TerrainType.Available;
            for (int startI = row - 1; startI < row + 2; startI++)
            {
                for (int startJ = column - 1; startJ < column + 2; startJ++)
                {
                    //Debug.Log(startI + " " + startJ);
                    if (startI >= 0 && startI < terrain.GetLength(0) && startJ >= 0 && startJ < terrain.GetLength(1) && terrain[startI, startJ] == TerrainType.Covered && CheckIfClear(startI, startJ, true))
                    {
                        terrain[startI, startJ] = TerrainType.Available;
                    }
                }
            }
        }
        else if (terrain[row, column] == TerrainType.CoveredResource)
        {
            terrain[row, column] = CheckIfClear(row, column, true) ? TerrainType.Available : TerrainType.Covered;
        }
        else
        {
            terrain[row, column] = TerrainType.Available;
        }
        resourceTypes[row, column] = -1;
    }

    private bool CheckIfClear(int row, int column, bool mustBeBig = false)
    {
        for (int startI = row - 1; startI < row + 2; startI++)
        {
            for (int startJ = column - 1; startJ < column + 2; startJ++)
            {
                //Debug.Log(startI + " " + startJ);
                if (startI >= 0 && startI < terrain.GetLength(0) && startJ >= 0 && startJ < terrain.GetLength(1))
                {
                    if (mustBeBig)
                    {
                        if (terrain[startI, startJ] == TerrainType.BigResource)
                        {
                            return false;
                        }
                    }
                    else if ((terrain[startI, startJ] & TerrainType.SmallResource) == TerrainType.SmallResource)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
