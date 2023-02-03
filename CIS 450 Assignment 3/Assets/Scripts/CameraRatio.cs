/* Caleb Kahn
 * CameraRatio
 * Assignment 3 (Hard)
 * Sets the camera to a 16:9 ratio
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRatio : MonoBehaviour
{
    //http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
    public UnityEngine.UI.CanvasScaler[] canvasScalers;

    void Start()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;
            /*private float GetScale(int width, int height, Vector2 scalerReferenceResolution, float scalerMatchWidthOrHeight)
            {//(w / 800)^1-x * (h / 600)^x = (16 * w^2) / (9 * 800 * h)
                return Mathf.Pow(width/scalerReferenceResolution.x, 1f - scalerMatchWidthOrHeight)*
                       Mathf.Pow(height/scalerReferenceResolution.y, scalerMatchWidthOrHeight);
            }*/
            float scale = (Mathf.Log(450f * Screen.width / Screen.height) - (5f * Mathf.Log(2f)) - (2f * Mathf.Log(5f))) / (Mathf.Log(Screen.width) - (2 * Mathf.Log(2f)) - Mathf.Log(Screen.height / 3f));
            //Debug.Log(scale);
            //Debug.Log(Screen.height);
            //Debug.Log(Screen.height / 600f);
            for (int i = 0; i < canvasScalers.Length; i++)
            {
                canvasScalers[i].matchWidthOrHeight = scale;
            }
            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
