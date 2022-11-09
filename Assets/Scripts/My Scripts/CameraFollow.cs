using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Target object : player
    public Transform targetObject;


    //Default distance between the camera and the player
    public Vector3 cameraOffset;

    //Smooth factor, it'll use in Camera position
    public float smoothFactor = 0.5f;

    //Will check that the camera looked at on the target or not
    public bool lookAtTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = transform.position - targetObject.transform.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = targetObject.transform.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        //Camera Rotation Change
        if (lookAtTarget)
        {
            transform.LookAt(targetObject);
        }
    }
}