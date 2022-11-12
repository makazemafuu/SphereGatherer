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

    //to shoot things
    public bool WantsToShoot { get; protected set; } = false;

    Vector3 velocity;
    bool isGrounded;

    void Start()
    {
        animator = Jammo.GetComponent<Animator>();
        speed = speedWalk;
        //UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //not 0f because this might happen before we are on the ground, so to make sure the player IS indeed on the ground we put a slightly lower negative number
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

        if (Input.GetKeyDown(KeyCode.C) && isCrouching == false)
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

        /*private void OnCollisionEnter(Collision collision)
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
        }*/
    }
}
