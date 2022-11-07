using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed;
    public float jumpForce = 5.0f;
    public bool isOnGround = true;
    public bool isOnTerrain = false;
    private float motionUpDown;
    private float motionLeftRight;
    private Rigidbody playerRb;
    public bool WantsToShoot { get; protected set; } = false;

    //[SerializeField] GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //UI.SetActive(false);
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
