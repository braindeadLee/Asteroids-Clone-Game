using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() => CameraUtility.CalculateDimensions();

    // Update is called once per frame
    private void Update()
    {
        //Vertical
        Vector3 position = transform.position;

        float bodyheight = transform.lossyScale.y * 0.9f;
        Debug.Log("");
        Debug.Log("");

        if (position.x > (CameraUtility.width / 2) + bodyheight)
        {
            position.x = (-CameraUtility.width / 2) - bodyheight;
        }
        else if (position.x < (-CameraUtility.width / 2) - bodyheight)
        {
            position.x = (CameraUtility.width / 2) + bodyheight;
        }

        //Horizontal
        if (position.y > CameraUtility.height / 2 + bodyheight)
        {
            position.y = -CameraUtility.height / 2 - bodyheight;
        }
        else if (position.y < -CameraUtility.height / 2 - bodyheight)
        {
            position.y = CameraUtility.height / 2 + bodyheight;
        }

        transform.position = position;
    }
}
