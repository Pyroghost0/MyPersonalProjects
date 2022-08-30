using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> buildings;
    public GameObject missilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
            Missile missile = Instantiate(missilePrefab, new Vector3(Random.Range(-8.5f, 8.5f), 5f, 0f), transform.rotation).GetComponent<Missile>();
            missile.goal = buildings[Random.Range(0, buildings.Count)].position;
        }
    }
}
