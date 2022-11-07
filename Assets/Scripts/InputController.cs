using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Controller
{
    public string MouseXName = "Mouse X";
    public string MouseYName = "Mouse Y";

    // Update is called once per frame
    void Update()
    {

        //Lecture des inputs
        Vector2 axisLook = new Vector2(Input.GetAxis(MouseXName), Input.GetAxis(MouseYName));

        //Rotation de la direction de look si input sur les axes
        //Deadzone deja gérée par les axis
        if (axisLook.x != 0 || axisLook.y != 0)
        {

        }

    }
}