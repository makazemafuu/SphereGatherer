using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float mouseSpeed = 4;
    public Transform Player;
    public float distanceToPlayer = 4;

    public float upDownMax = 80;
    public float upDownMin = -80;

    //Pour lisser le mouvement
    public float rotationSmoothTime = 0.12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    private float orbit;
    private float upDown;
    void LateUpdate()
    {
        //On récupère les mouvements de la souris
        upDown -= Input.GetAxis("Mouse Y") * mouseSpeed;
        orbit += Input.GetAxis("Mouse X") * mouseSpeed;

        //On limite les mouvements verticaux afin de ne pas passer en dessous du joueur
        upDown = Mathf.Clamp(upDown, upDownMin, upDownMax);

        //On effectue la rotation de la caméra
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(upDown, orbit), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        //On place la caméra à une certaine distance du joueur
        transform.position = Player.position - transform.forward * distanceToPlayer;
    }
}