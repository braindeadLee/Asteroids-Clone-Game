using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtility : MonoBehaviour
{
    public static float height;
    public static float width;

    public static void CalculateDimensions()
    {
        Camera cam = Camera.main;

        height = 2 * cam.orthographicSize;
        width = height * cam.aspect;
    }
}
