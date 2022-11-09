using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Controller
{
    public float XSensivity = 360; //Degres par seconde par valeur d input.
    public float YSensivity = 360; //Degres par seconde par valeur d input.
    public string MouseXName = "Mouse X";
    public string MouseYName = "Mouse Y";

    public float MaxUpDownAngle = 70; //Angle max de rot verticale depuis l horizontal

    // Update is called once per frame
    void Update()
    {
        //Lecture des inputs
        Vector2 axisLook = new Vector2(Input.GetAxis(MouseXName), Input.GetAxis(MouseYName));

        //Rotation de la direction de look si input sur les axes
        //Deadzone deja gérée par les axis
        if (axisLook.x != 0 || axisLook.y != 0)
        {
            Vector3 WantedDirectionLookRightTargetSmooth = Vector3.Cross(transform.up, WantedDirectionLookTargetSmooth).normalized;
            Quaternion rotateHorizontal = Quaternion.AngleAxis(axisLook.x * XSensivity * Time.deltaTime, Vector3.up);
            Quaternion rotateVertical = Quaternion.AngleAxis(-axisLook.y * YSensivity * Time.deltaTime, WantedDirectionLookRightTargetSmooth);

            //On teste la rotation verticale par rapport au max
            Vector3 afterVertRot = rotateVertical * WantedDirectionLookTargetSmooth;
            float angleWithUp = Vector3.Angle(afterVertRot, Vector3.up);
            float angleMinWithUp = 90 - MaxUpDownAngle;
            if (angleWithUp < angleMinWithUp)
                rotateVertical *= Quaternion.AngleAxis(angleMinWithUp - angleWithUp, WantedDirectionLookRightTargetSmooth);
            float angleMaxWithUp = 90 + MaxUpDownAngle;
            if (angleWithUp > angleMaxWithUp)
                rotateVertical *= Quaternion.AngleAxis(angleMaxWithUp - angleWithUp, WantedDirectionLookRightTargetSmooth);


            WantedDirectionLookTargetSmooth = rotateHorizontal * rotateVertical * WantedDirectionLookTargetSmooth;

            //On gère le shoot
            WantsToShoot = Input.GetButton("Fire1");
        }

        //On applique doucement
        SmoothWantedDirectionLook(Time.deltaTime);

        //On affiche le debug
        DrawDebug();
    }
}