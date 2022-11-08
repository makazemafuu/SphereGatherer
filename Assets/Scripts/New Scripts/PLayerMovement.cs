using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Vector 3 move = new Vector3(x, 0f, z); --- FOR GLOBAL SPACE

        //direction based on our x & z movement on local coordinates
        Vector3 move = transform.right * x + transform.forward * z;

        //function Move, framerate independent & with a given speed
        controller.Move(move * speed * Time.deltaTime);
    }
}
