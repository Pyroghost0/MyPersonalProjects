using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] allVirtualCameras;

    [Header("Controls for Y damping")]
    [SerializeField] private float fallAmount = .25f;
    [SerializeField] private float fallYPanTime = .35f;
    public float fallSpeedYDampingThreshold = -15f;

    public bool isLearpingYDamping { get; private set; }
    public bool lerpedFromPlayerFalling { get; set; }

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;
    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;
    private Vector2 startingTrackedObjectOffset;

    private float normYPanAmount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        for (int i = 0; i < allVirtualCameras.Length; i++)
        {
            if (allVirtualCameras[i].enabled)
            {
                currentCamera = allVirtualCameras[i];
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        normYPanAmount = framingTransposer.m_YDamping;
        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
    }

    #region Lerp Y Damping
    public void LerpYDamping(bool isFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isFalling));
    }

    IEnumerator LerpYAction(bool isFalling)
    {
        isLearpingYDamping = true;
        float startDamping = framingTransposer.m_YDamping;
        float endDamping = 0f;
        if (isFalling)
        {
            endDamping = fallAmount;
            lerpedFromPlayerFalling = true;
        }
        else
        {
            endDamping = normYPanAmount;
        }

        float timer = 0f;
        while (timer < fallYPanTime)
        {
            timer += Time.deltaTime;
            framingTransposer.m_YDamping = Mathf.Lerp(startDamping, endDamping, timer / fallYPanTime);
            yield return null;
        }
        isLearpingYDamping = false;
    }

    #endregion

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;
        if (panToStartingPos)
        {
            startingPos = framingTransposer.m_TrackedObjectOffset;
            endPos = startingTrackedObjectOffset;
        }
        else
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.right;//Why
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
            }
            endPos *= panDistance;
            startingPos = startingTrackedObjectOffset;
            endPos += startingPos;
        }

        float timer = 0f;
        while (timer < panTime)
        {
            timer += Time.deltaTime;
            framingTransposer.m_TrackedObjectOffset = Vector3.Lerp(startingPos, endPos, timer / panTime);
            yield return null;
        }
    }

    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        if (currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            cameraFromRight.enabled = true;
            cameraFromLeft.enabled = false;
            currentCamera = cameraFromRight;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else if (currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            cameraFromRight.enabled = false;
            cameraFromLeft.enabled = true;
            currentCamera = cameraFromLeft;
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
}
