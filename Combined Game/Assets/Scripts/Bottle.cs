using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public ingredient ingredient;
    private float dustPow = 0f;
    public int leftoverDust = 400;
    public Transform dustSpawn;
    public GameObject dust;
    public Color lightestColor;
    public Color darkestColor;
    //public float spawnRadius;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer dustSpriteRenderer;
    public Canvas label;
    public static int dustRender = 0;
    public static List<Bottle> bottles = new List<Bottle>();

    public static bool holdingABottle { get{ return bottles[0].holding && bottles[1].holding && bottles[2].holding; } }
    public bool holding = false;
    public Collider2D bottleCollider;
    public Collider2D mouseCollider;
    public Collider2D holdCollider;
    public Transform mouse;
    public Transform hold;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    // Start is called before the first frame update
    void Start()
    {
        bottles.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1f)
        {
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2((center.position - laddleCenter.position).y, (center.position - laddleCenter.position).x) * 57.2958f + 90f), Time.deltaTime * 20f);
            //Vector2 position = camera.ScreenToWorldPoint(Input.mousePosition);
            //mouse.position = position;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (bottleCollider.IsTouching(mouseCollider) && !holdingABottle)
                {
                    holding = true;
                    hold.position = mouse.position;
                    hold.localPosition = new Vector2(0f, hold.localPosition.y);
                    bottles.Remove(this);
                    bottles.Add(this);
                    UpdateOrder();
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                holding = false;
            }

            //Movement
            if (holding)
            {
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2((center.position - farPoint.position).y, (center.position - farPoint.position).x) * 57.2958f + 90f), Time.deltaTime * 50f);
                if (holdCollider.IsTouching(mouseCollider))
                {
                    rigidbody.drag = 75f;
                }
                else
                {
                    rigidbody.drag = 15f;
                    rigidbody.AddForce((holdCollider.ClosestPoint(mouse.position) - ((Vector2)hold.position)) * 1500f);
                }
                if (rigidbody.velocity.y > 0f)
                {
                    dustPow += Time.deltaTime * rigidbody.velocity.y;
                }
                while (dustPow > .25f && leftoverDust > 0)
                {
                    leftoverDust--;
                    dustSpriteRenderer.size = new Vector2(1.375f, 2.375f * (leftoverDust / 400f));
                    dustPow -= .25f;
                    dustRender--;
                    float colorScale = Random.value;
                    Instantiate(dust, dustSpawn.position + new Vector3(Random.Range(-.5f, .5f), Random.Range(-.25f, .25f), 0f), transform.rotation).GetComponent<Dust>().
                        SetUp(dustRender, new Color(lightestColor.r + ((darkestColor.r - lightestColor.r) * colorScale), lightestColor.g + ((darkestColor.g - lightestColor.g) * colorScale), lightestColor.b + ((darkestColor.b - lightestColor.b) * colorScale)), this);
                }
            }
            //else
            //{
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, 0f, Mathf.Atan2((center.position - transform.position).y, (center.position - transform.position).x) * 57.2958f + 90f), Time.deltaTime * 3f);
            //}
        }
        else
        {
            holding = false;
        }
    }

    public static void UpdateOrder()
    {
        for (int i = 0; i < bottles.Count; i++)
        {
            bottles[i].spriteRenderer.sortingOrder = -5001 + (i * 3);
            bottles[i].dustSpriteRenderer.sortingOrder = -5002 + (i * 3);
            bottles[i].label.sortingOrder = -5000 + (i * 3);
        }
    }
}
