using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [SerializeField]
    private float speed = 0f;

    [SerializeField]
    private float speedWalk = 5f;

    [SerializeField]
    private float speedRun = 10f;

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float jumpHeight = 3f;

    [SerializeField]
    private float mass = 1.5f;

    //Référence pour Jammo
    [SerializeField]
    private GameObject Jammo;

    private bool isCrouching;

    private Animator animator;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform floorCheck;
    public float floorDistance = 0.4f;
    public LayerMask floorMask;

    //to shoot things
    public bool WantsToShoot { get; protected set; } = false;

    Vector3 velocity;
    bool isGrounded;
    bool isOnFloor;

    [SerializeField] GameObject UI;

    void Start()
    {
        animator = Jammo.GetComponent<Animator>();
        speed = speedWalk;
        UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            Debug.Log("The player is on the ground !");
            velocity.y = -2f; //not 0f because this might happen before we are on the ground, so to make sure the player IS indeed on the ground we put a slightly lower negative number
        }

        isOnFloor = Physics.CheckSphere(floorCheck.position, floorDistance, floorMask);

        if (isOnFloor && velocity.y < 0)
        {
            Debug.Log("The player is on the floor !");
            velocity.y = -2f; //not 0f because this might happen before we are on the ground, so to make sure the player IS indeed on the ground we put a slightly lower negative number
            //collision.gameObject.SetActive(false);
            Cursor.visible = true;
            UI.SetActive(true);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Vector 3 move = new Vector3(x, 0f, z); --- FOR GLOBAL SPACE

        //direction based on our x & z movement on local coordinates
        Vector3 move = transform.right * x + transform.forward * z;

        //function Move, framerate independent & with a given speed
        if (!isCrouching)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        if (x != 0 || z != 0)
        {
            if (speed == speedRun)
            {
                animator.SetInteger("Moving", 2);
            }
            else
            {
                animator.SetInteger("Moving", 1);
            }
        }
        else
        {
            animator.SetInteger("Moving", 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = speedRun;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = speedWalk;
        }

        if (Input.GetKeyDown(KeyCode.C) && isCrouching == false && speed != speedRun)
        {
            animator.SetBool("Crouch", true);
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.C) && isCrouching == true)
        {
            animator.SetBool("Crouch", false);
            isCrouching = false;
        }

        //to allow the player to jump
        if (Input.GetButtonDown("Jump") && isGrounded && isCrouching == false)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }

        velocity.y += gravity * mass * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //On gère le shoot
        WantsToShoot = Input.GetButton("Fire1");

    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            if (isOnFloor && velocity.y < 0)
            {

                isOnFloor = Physics.CheckSphere(floorCheck.position, floorDistance, floorMask);

                Debug.Log("The player is on the floor !");
                velocity.y = -2f; //not 0f because this might happen before we are on the ground, so to make sure the player IS indeed on the ground we put a slightly lower negative number
                isOnFloor = true;

                collision.gameObject.SetActive(false);
                //UI.SetActive(true);

            }
        }
    }*/
}
