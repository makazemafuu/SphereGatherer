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

        Vector3 vecForceInput = wantedDirectionMoveBody * ForceInput * MyController.WantedSpeed;
        Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;
        Vector3 acceleration = (vecForceInput / GetComponent<Rigidbody>().mass) * Time.deltaTime;
        Vector3 newVelocity = currentVelocity + acceleration;

        //Si on depasse la vitesse max, on scale l acceleration
        if (newVelocity.sqrMagnitude > MaxSpeed * MaxSpeed)
        {
            float alpha = 0;
            MyUtils.ScaleBToGetMagAPlusB(currentVelocity, acceleration, MaxSpeed, out alpha);
            vecForceInput = (alpha * acceleration * GetComponent<Rigidbody>().mass) / Time.deltaTime;
        }

        //On applique la force
        GetComponent<Rigidbody>().AddForce(vecForceInput);

        //Gère la rotation du corps
        GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(wantedDirectionMoveBody, Vector3.up));
    }
}