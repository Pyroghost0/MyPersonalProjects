using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollowObject : MonoBehaviour
{
    //This all can be done in a add on called tween machine
    [Header("References")]
    [SerializeField] private Transform playerTransform;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipYRotationTime = .5f;

    private Coroutine turnCoroutine;
    private Player player;
    private bool facingRight;

    // Start is called before the first frame update
    void Awake()
    {
        player = playerTransform.parent.GetComponent<Player>();
        facingRight = !player.facingRight;//Don't know why this is opposite, it souldn't be
        //transform.rotation = Quaternion.Euler(0f, facingRight ? 180f : 0, 0f);
    }

    private void Update()
    {
        transform.position = playerTransform.position;
    }

    public void CallTurn()
    {
        //transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y * -1, 0f);
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    IEnumerator FlipYLerp()
    {
        facingRight = !facingRight;
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = facingRight ? 180f : 0f;
        float yRotation = 0f;

        float timer = 0f;
        while (timer < flipYRotationTime)
        {
            timer += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, timer / flipYRotationTime);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
    }
}
