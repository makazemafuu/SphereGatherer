using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public Controller MyController;
    public float MaxSpeed = 5;
    public float ForceInput = 100;
    public AnimationCurve SlopeAccelerationMultiplyer = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(45, 0) });
    public float rotationSpeed = 180;
    public float groundTouchDistance = 0.1f;
    private bool touchGround = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        //On tourne le corps dans le sens du déplacement
        //On reste toujours vertical
        Vector3 wantedDirectionMoveBody = MyController.WantedDirectionMove;
        wantedDirectionMoveBody.y = 0;

        Vector3 vecForceInput = wantedDirectionMoveBody * ForceInput * MyController.WantedSpeed;

        //On regarde le sol
        RaycastHit hitInfo;
        touchGround = false;
        if (Physics.SphereCast(transform.position, 0.2f, Vector3.down, out hitInfo))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);
            vecForceInput *= SlopeAccelerationMultiplyer.Evaluate(angle);

            //On aide à monter les pentes : on oriente l input dans le plan du sol
            float magInput = vecForceInput.magnitude;
            vecForceInput = Vector3.ProjectOnPlane(vecForceInput, hitInfo.normal).normalized * magInput;

            //On calcule la distance au sol
            float distToGround = Vector3.Distance(hitInfo.point, transform.position) - GetComponent<Collider>().bounds.size.y / 2;
            touchGround = distToGround < groundTouchDistance;
        }

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

        //Gère la rotation du corps et de la vitesse
        if (MyController.WantedSpeed > 0)
        {
            float angleRotBody = Vector3.Angle(transform.forward, wantedDirectionMoveBody);
            float angleRotVelocity = Vector3.Angle(currentVelocity, wantedDirectionMoveBody);
            float angleRotMax = Time.deltaTime * rotationSpeed;
            angleRotBody = Mathf.Min(angleRotBody, angleRotMax);
            angleRotVelocity = Mathf.Min(angleRotVelocity, angleRotMax);

            if (Mathf.Abs(angleRotBody) > 0)
            {
                Quaternion rot = Quaternion.AngleAxis(angleRotBody, Vector3.Cross(transform.forward, wantedDirectionMoveBody).normalized);
                Vector3 rotForward = rot * transform.forward;
                rotForward.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(rotForward.normalized, Vector3.up);
                GetComponent<Rigidbody>().MoveRotation(targetRot);
                GetComponent<Rigidbody>().angularVelocity = new Vector3();
            }

            if (Mathf.Abs(angleRotVelocity) > 0 && touchGround)
            {
                Quaternion rot = Quaternion.AngleAxis(angleRotVelocity, Vector3.Cross(currentVelocity, wantedDirectionMoveBody).normalized);
                GetComponent<Rigidbody>().velocity = rot * currentVelocity;
            }
        }
        else
        {
            GetComponent<Rigidbody>().angularVelocity = new Vector3();
        }
    }
}