using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MiningType
{
    gather = 0,
    chop = 1,
    mine = 2
}

public class Resource : MonoBehaviour, InteractableObject
{
    public ItemType[] possibleItems;
    public float[] itemChance = { 1f };
    public GameObject itemPrefab;
    public float spawnRadius = .3f;
    public MiningType miningType;
    public float totalGatherTime = 2f;
    private float gatherTime;
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
        spriteRenderer.sortingOrder = -(int)transform.position.y * 10;
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
        TerrainGenerator.instance.RemoveResource(row, column);
        Destroy(gameObject);
    }

    public void Cancel()
    {
        StopCoroutine(gatherCoroutine);
        Player.instance.interaction = null;
        gatherBar.SetActive(false);
    }
}
