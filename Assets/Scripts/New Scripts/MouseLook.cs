using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Time.deltaTime = amount of time that has gone by since the last update
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Quaternions = rotations in Unity
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); 
        playerBody.Rotate(Vector3.up * mouseX); //y axis
    }
}
