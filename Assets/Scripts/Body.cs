using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Controller MyController;
    public float MaxSpeed = 5;

    // Update is called once per frame
    void Update()
    {
        //On tourne le corps dans le sens du déplacement
        //On reste toujours vertical
        Vector3 wantedDirectionMoveBody = MyController.WantedDirectionMove;
        wantedDirectionMoveBody.y = 0;

        transform.rotation = Quaternion.LookRotation(wantedDirectionMoveBody, Vector3.up);
        transform.position += wantedDirectionMoveBody * MyController.WantedSpeed * MaxSpeed * Time.deltaTime;
    }
}