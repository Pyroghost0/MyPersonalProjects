using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MiningType
{
    Gather = 0,
    Chop = 1,
    Mine = 2
}

public class Resource : MonoBehaviour, InteractableObject
{
    public GameObject parentObject;
    public TerrainType terrainType;
    public ItemType[] possibleItems;
    public float[] itemChance = { 1f };
    public GameObject itemPrefab;
    public float spawnRadius = .3f;
    public MiningType miningType;
    public float totalGatherTime = 2f;
    private float gatherTime;
    public Transform bar;
    public Vector3 upBarPosition = new Vector3(0f, .5f);
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer gatherBarAmount;
    public GameObject gatherBar;
    private Coroutine gatherCoroutine;
    //public Player player;

    public int row;
    public int column;

    public void SetRowColumn(int r, int c)
    {
        row = r;
        column = c;
    }

    void Start()
    {
        gatherTime = totalGatherTime;
        if (terrainType == TerrainType.BigResource)
        {
            transform.position += new Vector3(Random.Range(-.05f, .05f), Random.Range(-.05f, .05f));
        }
        else if (terrainType == TerrainType.CoveredResource)
        {
            int i = -1;
            int j = -1;
            while (i < 2)
            {
                bool found = false;
                while (j < 2)
                {
                    if (row + i < 0 || row + i >= TerrainGenerator.terrain.GetLength(0) || column + j < 0 || column + j >= TerrainGenerator.terrain.GetLength(1))
                    {
                        j++;
                    }
                    else if (TerrainGenerator.terrain[row + i, column+ j] == TerrainType.BigResource)
                    {
                        found = true;
                        break;
                    }
                    else
                    {
                        j++;
                    }
                }
                if (found)
                {
                    break;
                }
                else
                {
                    j = -1;
                }
                i++;
            }
            transform.position += new Vector3(Random.Range(-.25f, .25f) + (.4f * i), Random.Range(-.25f, .25f) + (.2f * j) + .25f);
        }
        else
        {
            transform.position += new Vector3(Random.Range(-.2f, .2f), Random.Range(-.25f, .25f));
        }
        spriteRenderer.sortingOrder = -(int)(transform.position.y * 10f);
    }

    public void Interact()//Player p)
    {
        /*if (gatherCoroutine == null)//Done here instead of start so that there is less lag when spawning all the resources;
        {
            gatherTime = totalGatherTime;
        }*/
        //player = p;
        gatherCoroutine = StartCoroutine(GatherCoroutine());
    }

    IEnumerator GatherCoroutine()
    {
        if (transform.position.y - Player.instance.transform.position.y > 0f)
        {
            bar.localPosition = upBarPosition;
        }
        gatherBar.SetActive(true);
        while (gatherTime > 0f)
        {
            yield return new WaitForFixedUpdate();
            gatherTime -= Time.deltaTime * Player.gatherEfficiency[(int)miningType];
            gatherBarAmount.size = new Vector2((gatherTime / totalGatherTime) * .96875f, .109375f);//124/128, 14/128
        }
        for (int i = 0; i < possibleItems.Length; i++)
        {
            if (Random.value <= itemChance[i])
            {
                Instantiate(itemPrefab, transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius)), transform.rotation).GetComponent<Item>().itemType = possibleItems[i];
            }
        }
        //Player.instance.objectsInRadius.Remove(transform);
        Player.instance.interaction = null;
        if (SceneManager.GetActiveScene().name == "Game")
        {
            TerrainGenerator.instance.RemoveResource(row, column);
        }
        Player.instance.animator.SetTrigger("Stop");
        Destroy(parentObject);
    }

    public void Cancel()
    {
        StopCoroutine(gatherCoroutine);
        Player.instance.interaction = null;
        Player.instance.animator.SetTrigger("Stop");
        gatherBar.SetActive(false);
    }
}
