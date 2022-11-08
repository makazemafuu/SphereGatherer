using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public PlayerMove MyController;
    public float speed = 5.0f;
    public float rotationSpeed;
    public float jumpForce = 5.0f;
    public float ForceInput = 100;
    public bool isOnGround = true;
    public bool isOnTerrain = false;
    private float motionUpDown;
    private float motionLeftRight;
    private Rigidbody playerRb;
    public bool WantsToShoot { get; protected set; } = false;
    public AnimationCurve SlopeAccelerationMultiplier = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(45, 0) });
    public Vector3 WantedDirectionMove { get; protected set; } = new Vector3(0, 0, 1);
    public float groundTouchDistance = 0.1f;
    private bool touchGround = false;

    //[SerializeField] GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //UI.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //On tourne le corps dans le sens du déplacement
        //On reste toujours vertical
        Vector3 wantedDirectionMoveBody = MyController.WantedDirectionMove;
        wantedDirectionMoveBody.y = 0;

        Vector3 vecForceInput = wantedDirectionMoveBody * ForceInput * MyController.speed;

        //On regarde le sol
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, 0.1f, Vector3.down, out hitInfo))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);
            vecForceInput *= SlopeAccelerationMultiplier.Evaluate(angle);

            //On aide à monter les pentes : on oriente l'input dans le plan du sol
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
        if (newVelocity.sqrMagnitude > speed * speed)
        {
            float alpha = 0;
            MyUtils.ScaleBToGetMagAPlusB(currentVelocity, acceleration, speed, out alpha);
            vecForceInput = (alpha * acceleration * GetComponent<Rigidbody>().mass) / Time.deltaTime;
        }

        //On applique la force
        GetComponent<Rigidbody>().AddForce(vecForceInput);

        //Gère la rotation du corps et de la vitesse
        if (MyController.speed > 0)
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

    // Update is called once per frame
    void Update()
    {
        //get player input
        motionUpDown = Input.GetAxis("Vertical");
        motionLeftRight = Input.GetAxis("Horizontal");

        Vector3 movementDirection = new Vector3(motionLeftRight, 0, motionUpDown);
        movementDirection.Normalize();

        //move the player forward
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        //transform.Translate(Vector3.forward * Time.deltaTime * speed * motionUpDown);
        //transform.Translate(Vector3.right * Time.deltaTime * speed * motionLeftRight);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up); //quaternion is a specific type for storing rotations
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            //transform.forward = movementDirection;
        }

        //let the player jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
        }

        WantsToShoot = Input.GetButton("Fire1");

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Player is on the ground !");
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("Terrain"))
        {
            Debug.Log("Player is on the terrain !");
            isOnTerrain = true;

            //collision.gameObject.SetActive(false);
            //UI.SetActive(true);
        }
    }

}
