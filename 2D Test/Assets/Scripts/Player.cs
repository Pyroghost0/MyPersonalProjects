using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 100f;
    public float jumpPower = 100f;
    public float additionalJumpHoldPower = 50f;
    public PlayerCameraFollowObject playerCameraFollowObject;

    private float timeJumped;
    [HideInInspector] public bool facingRight = true;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    [SerializeField] private Rigidbody2D rigidbody;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    private float fallSpeedYDampThreshold;

    //Change body from transposer to framing trasposer with the settings (Aim Do nothing)
    //Rotating Y by 180 changes transform.right, so change player that way
    //Add camera follow object script, and camera manager
    //Add confiner2D extension (can use composite collider with box2Ds. You must make confiner first with polygons and can set oversized window but high CPU)
    //Camera Control Trigger script for panning up/down in certain spots
    //Camera control Trigger for changing camera for hidden rooms (turn off all other CMVirtualCameras)
    //Optional set cinemamachine brain default blend (and even custom blend between 2 specific cameras) for transitions between them

    void Start()
    {
        fallSpeedYDampThreshold = CameraManager.instance.fallSpeedYDampingThreshold;
    }
    void Update()
    {
        rigidbody.velocity += new Vector2(Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime, 0f);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            timeJumped = Time.time + 1f;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.velocity += new Vector2(0f, additionalJumpHoldPower * speed * Mathf.Max(timeJumped - Time.time, 0f) * Time.deltaTime);
        }

        if (rigidbody.velocity.y < fallSpeedYDampThreshold && !CameraManager.instance.isLearpingYDamping && !CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.LerpYDamping(true);
        }
        if (rigidbody.velocity.y >= 0f && !CameraManager.instance.isLearpingYDamping && CameraManager.instance.lerpedFromPlayerFalling)
        {
            CameraManager.instance.lerpedFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") > 0f && !facingRight) 
        {
            facingRight = true;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            playerCameraFollowObject.CallTurn();
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f && facingRight)
        {
            facingRight = false;
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);
            playerCameraFollowObject.CallTurn();
        }
    }
}
