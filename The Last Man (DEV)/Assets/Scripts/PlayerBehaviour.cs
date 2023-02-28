using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    private Rigidbody rb;
    private GameObject player;
    [SerializeField] private Transform c_groundCheck;
    [SerializeField] private LayerMask l_groundMask;
    [SerializeField] private Transform playerCamera;


    private float horizontalInput, verticalInput;
    private float mouseHorizontal, mouseVertical;
    [SerializeField] private bool jumpIsTrigger;
    [SerializeField] private bool _shoot;
    [SerializeField, Range(10f, 1000f)] private float mouseSensitivity = 1000f;

    //private Vector3 move
    [SerializeField, Range(0f, 3f)] private float jumpForce;
    [SerializeField, Range(0f, 10f)] private float movementSpeed = 5f;
    private float rotX, rotY;

    [SerializeField] private bool isGrounded;
    [SerializeField, Range(0f, 10f)] private float groundDistance;

    private bool jumpLaunched = false;
    [SerializeField] private float jumpAirTime;

    [SerializeField] private bool ignoreJump = true;

    private float jumpTimer;
    [SerializeField, Range(0f, 0.1f)] private float jumpDelay;



    //testing for jump 
    [SerializeField] private bool UseJumpLaunched = false;
    [SerializeField] private bool UseJumpTrigger = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        player = GetComponent<GameObject>();
    }

    private void FixedUpdate()
    {
        GetPlayerInput();
        ApplyMovement();
        ApplyMouseDirection();
        //MovePlayerCamera();
        //JumpLaunch();
        //CheckJumpTrigger();

        if(!ignoreJump)
        {
            TestingJump();
        }

        isGrounded = CheckGrounded();
    }


    private void TestingJump()
    {
        if(UseJumpLaunched)
        {
            JumpLaunch();
        }
        else if(UseJumpTrigger)
        {
            CheckJumpTrigger();
        }
        
    }



    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");

        jumpIsTrigger = Input.GetButton("Jump");

        _shoot = Input.GetButton("Fire1");
    }

    public bool Shoot()
    {
        return _shoot;
    }


    private bool CheckGrounded()
    {
        if(c_groundCheck != null)
        {
            bool OnGround = Physics.CheckSphere(c_groundCheck.position, groundDistance, l_groundMask);

            return OnGround;
        }
        else
        {
            return false;
            //print("Please add GroundCheck to Reference");
        }
    }


    private void ApplyMovement()
    {
        if(rb != null)
        {
            Vector3 m_Input = new Vector3(horizontalInput, 0 , verticalInput);
            m_Input = rb.rotation * m_Input;
            rb.MovePosition(transform.position + m_Input * Time.deltaTime * movementSpeed);

            //Vector3 m_Input = new Vector3(horizontalInput, 0f, verticalInput);
            //Vector3 moveVector = transform.TransformDirection(m_Input) * movementSpeed;
            // might still change rb.velocity.y to a different value depending on the gravity 
            //rb.velocity = new Vector3(moveVector.x, rb.velocity.y, moveVector.z);
        }
        else
        {
            print("Please attach Rigidbody to " + this.name);
        }

    }

    private void ApplyMouseDirection()
    {
        //look up and down based on the mouse input
        rotY -= mouseVertical * mouseSensitivity * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -90f, 90f);

        //playerCamera.transform.Rotate(Vector3.right * rotY);
        //playerCamera.transform.RotateAround(transform.position, Vector3.forward, rotY);
        //transform.Rotate(Vector3.forward * rotY);
        //playerCamera.transform.localRotation = Quaternion.Euler(rotY, 0f, 0f);

        // look left and rightbased on the mouse input
        rotX = mouseHorizontal * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * rotX);
    }


    private void MovePlayerCamera()
    {
        if (playerCamera != null)
        {
            rotX -= mouseVertical * mouseSensitivity;
            
            transform.Rotate(0f, mouseHorizontal * mouseSensitivity, 0f);
            playerCamera.transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        }
        else
            print("Please attach camera to " + this.name);


    }

    private void JumpLaunch()
    {
        if(rb != null)
        {
            if(jumpIsTrigger && isGrounded)
            {
                jumpLaunched = true;
                jumpTimer += Time.deltaTime;
                if(jumpTimer > jumpDelay)
                {
                    rb.AddForce(Vector3.up * jumpForce , ForceMode.Impulse);
                    jumpLaunched = false;
                    jumpTimer = 0;  
                }
            }
        }
    }

    private void CheckJumpTrigger()
    {
        if(jumpIsTrigger && isGrounded)
        {
           // jumpLaunched = true;
            rb.velocity += jumpForce * Vector3.up; 
        }
    }


    private void OnDrawGizmos()
    {
        if(c_groundCheck != null)
        {
            if(isGrounded)
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.cyan;

            //Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(c_groundCheck.position, groundDistance);

            


        }
        else
        {
            print("Please add GroundCheck to Reference");
        }
    }


    public Vector2 GetPlayerMovement()
    {
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        return movement;
    }

    public bool GetJumpLaunced()
    {
        //return jumpLaunched;

        bool jump; 

        if(ignoreJump)
        {
            jump = false;
        }
        else
        {
            jump = jumpIsTrigger;
        }

        return jump;
    }

}
