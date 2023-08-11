using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class Rotation : MonoBehaviour
{
    public bool printRubix = false;
    public Transform centerTarget;
    public Transform rotatingObject;
    public Transform rayTarget;
    public Transform rubix;

    public float rotation1;
    public float rotation2;
    private Vector3 result;

    public Vector4 rubixRotation;

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying)
        {
            Vector3 relPos = (centerTarget.position - rotatingObject.position);
            //Slerp is a version of lerp where instead of linear, its slow -> fast -> slow
            rotatingObject.localRotation = Quaternion.Slerp(rotatingObject.localRotation, Quaternion.LookRotation(relPos), Time.deltaTime);
            rotatingObject.Translate(0, 0, 3 * Time.deltaTime);
        }

        result = Vector3.forward * 5f;
        //It HAS to be Quanterloon * vector not v * q
        result = Quaternion.Euler(rotation1, 0f, 0f) * Quaternion.Euler(0f, rotation2, 0f) * result;
        //Multiplying Q by Q is just adding so (45, 0, 0) * (45, 0, 0) = (90, 0, 0)

        if (printRubix)
        {
            Debug.Log(rubix.rotation);
        }
        else
        {
            UpdateRubix();
        }
    }

    public void UpdateRubix()
    {
        rubix.rotation = new Quaternion(rubixRotation.x, rubixRotation.y, rubixRotation.z, rubixRotation.w);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayTarget.position, rayTarget.position + result);
    }
}
