using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonView : MonoBehaviour
{
    public Controller MyController;
    public Body MyBody;
    public float CameraDistance = 3.0f;
    public float SmoothFactor = 0.001f;
    private const float SmoothFactorDivisor = 1e6f;

    void Update()
    {
        //On tourne la tête dans le sens de la vue
        float t = 1 - Mathf.Pow(SmoothFactor / SmoothFactorDivisor, Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, MyBody.transform.position
            - MyController.WantedDirectionLook.normalized * CameraDistance, t);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(MyController.WantedDirectionLook), t);
    }

}