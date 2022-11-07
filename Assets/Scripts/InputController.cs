using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Controller
{
    public float XSensivity = 360; //Degres par seconde par valeur d input.
    public float YSensivity = 360; //Degres par seconde par valeur d input.
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
            Vector3 WantedDirectionLookRight = Vector3.Cross(Vector3.up, WantedDirectionLook);
            Quaternion rotateHorizontal = Quaternion.AngleAxis(axisLook.x * XSensivity * Time.deltaTime, Vector3.up);
            Quaternion rotateVertical = Quaternion.AngleAxis(-axisLook.y * YSensivity * Time.deltaTime, WantedDirectionLookRight);
            WantedDirectionLook = rotateHorizontal * rotateVertical * WantedDirectionLook;
        }

        //On affiche le debug
        DrawDebug();
    }
}