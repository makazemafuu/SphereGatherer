using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Controller MyController;
    public float MaxSpeed = 5;
    public float ForceInput = 100;

    // Update is called once per frame
    void FixedUpdate()
    {
        //On tourne le corps dans le sens du déplacement
        //On reste toujours vertical
        Vector3 wantedDirectionMoveBody = MyController.WantedDirectionMove;
        wantedDirectionMoveBody.y = 0;

        //Si la vitesse est inférieure a notre max speed, on applique une force
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude < MaxSpeed * MaxSpeed)
            GetComponent<Rigidbody>().AddForce(wantedDirectionMoveBody * ForceInput * MyController.WantedSpeed);

        GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(wantedDirectionMoveBody, Vector3.up));
    }
}