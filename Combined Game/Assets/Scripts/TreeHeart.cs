using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHeart : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
    public Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public Transform fire;
    public HeartPart[] HeartParts;
    public Collider2D heartCollider;
    public Collider2D mouseCollider;
    public float cookedAmount = 0f;
    public ParticleSystem particle;
    public SpriteRenderer[] spriteRenderers;
    private bool holding = false;

    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Vector2 position = camera.ScreenToWorldPoint(Input.mousePosition);
                //mouse.position = position;
                if (heartCollider.IsTouching(mouseCollider))
                {
                    holding = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                holding = false;
            }

            //Movement
            if (holding)
            {
                Vector2 position = camera.ScreenToWorldPoint(Input.mousePosition);
                //mouse.position = position;
                if (!heartCollider.IsTouching(mouseCollider))
                {
                    rigidbody.AddForce((heartCollider.ClosestPoint(position) - ((Vector2)transform.position)) * 30f);
                }
            }

            cookedAmount += Vector3.Distance(transform.position, fire.position) < 6f ? (3f / Mathf.Max(Vector3.Distance(transform.position, fire.position), 1f)) * .05f * Time.deltaTime : 0f;
            //particle.Stop();
            float amount = cookedAmount > 1f ? Mathf.Max(0f, 2f - cookedAmount) : cookedAmount;
            ParticleSystem.MainModule m = particle.main;
            m.startSpeed = new ParticleSystem.MinMaxCurve(.5f, amount);
            ParticleSystem.EmissionModule e = particle.emission;
            //particle.Play();
#pragma warning disable CS0618 // Type or member is obsolete
            e.rate = new ParticleSystem.MinMaxCurve(amount * 50f);
#pragma warning restore CS0618 // Type or member is obsolete
            foreach (HeartPart heartPart in HeartParts)
            {
                heartPart.part.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.timeSinceLevelLoad * (heartPart.cycleTime * cookedAmount)) * heartPart.Radius(cookedAmount));
            }
            amount = Mathf.Min(cookedAmount, 2f) * .045f;
            Color c = new Color(1f - (6f * amount), 1f - (9f * amount), 1f - (11f * amount));
            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                spriteRenderer.color = c;
            }
        }
        else
        {
            holding = false;
        }
    }
}

[System.Serializable]
public class HeartPart
{
    public Transform part;
    public float smallRotation = 8f;//At .5
    public float bigRotation = 20f;//At 1
    public float cycleTime = 1f;

    public float Radius(float value)
    {
        //Debug.Log("Min: " + Mathf.Min((smallRotation - Mathf.Pow(value, 2f)) * value * 2f, smallRotation));
        //Debug.Log("Max: " + ((value <= 1f ? Mathf.Pow(value, 2f) : Mathf.Min((1f - (Mathf.Pow(2f - Mathf.Min(value, 2f), 2f)) + 1f), 2f)) * (bigRotation - smallRotation)));
        //return Mathf.Min((smallRotation - Mathf.Pow(Mathf.Min(value, 1f), 2f)) * value * 2f, smallRotation) + ((value <= 1f ? Mathf.Pow(value, 2f) : Mathf.Min((1f - (Mathf.Pow(2f - Mathf.Min(value, 2f), 2f)) + 1f), 2f)) * (bigRotation - smallRotation));
        return Mathf.Min((value < 1f ? (smallRotation - Mathf.Pow(Mathf.Min(value, 1f), 2f)) : Mathf.Max(1f - Mathf.Pow(value, 2f), 0f)) * value * 2f, 2f * value * 2f, smallRotation) + ((value <= 1f ? Mathf.Pow(value, 2f) : Mathf.Min(Mathf.Pow(2f - Mathf.Min(value, 2f), 2f), 2f)) * (bigRotation - smallRotation));
    }
}