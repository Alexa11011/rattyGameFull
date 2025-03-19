using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    //speeds
    public float JumpHeight, JumpDelay, JumpTimer ;
 //  public float JumpDelay;
   // public float JumpTimer;
    [Range(1, 10)] public float WalkSpeed;
     public float SprintMultiplier;
    public float RotateHorizontal;

    //refrence for moving player
    public CharacterController PlayerController;

    //refrence to feet location
    public Transform GroundCheck;
    //radius of foot checker
    public float GroundDistance = 0.1f;
    //reference to laymask as to avoid checking self for floor
    public LayerMask GroundMask;
    //is the player standing on the ground
    bool IsGrounded;

    //where to move
    private Vector3 MoveDirection;
    
    //current velocity up or down
    Vector3 Velocity;

    //turn speed
    public float Sensitivity;
    //set gravity
    public float Gravity;

    //lowgravity settings
    private float gravityChange = 1;
    
    //references to the gravity lever
    public void LowerGravity()
    {
        gravityChange = 0.2f;
    }
    public void NormaliseGravity()
    {
        gravityChange = 1f;
    }


    private void Awake()
    {
        //no collision with extended wall edges all objects 0 is default layer
        Physics.IgnoreLayerCollision(0, 9);
        //ignore collision with extended wall edges. 11 is playerlayer
        Physics.IgnoreLayerCollision(11, 9);

        float resetjump = JumpDelay;
    }
    private void Turn()
    {

        //get the mouse left and right input
        RotateHorizontal = Input.GetAxis("Mouse X");
        //when i move this out the cube looks jittery but when its in the cam is jittery

        //rotate the player
        transform.Rotate(0, RotateHorizontal * Sensitivity, 0);
    }


    //rotate player same as camera
    private void FixedUpdate()
    {
        //move the player
        PlayerController.Move(MoveDirection * WalkSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        Turn();


        //get the direction of input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //combine both inputs
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            //add sprint if sprinting
            MoveDirection = transform.right * x + transform.forward * z * SprintMultiplier;
        }
        else
        {
            MoveDirection = transform.right * x + transform.forward * z;
        }

        //update player velocity downwards
        Velocity.y += (Gravity + Time.deltaTime) * gravityChange;

        //Move the player down
        PlayerController.Move(Velocity * Time.deltaTime);

        //check if the player is on a ground layer
        IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        //count until can jump again and check if touching ground
        if(IsGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
            JumpTimer = JumpTimer + Time.deltaTime;
        }       
        
        //Jump
        if ((Input.GetKey(KeyCode.Space)) & IsGrounded & JumpDelay < JumpTimer)
        {
            Velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            JumpTimer = 0;
        }      

        //Crouch outside to call standing back up
        if ((Input.GetKey(KeyCode.LeftControl)))
        {
            this.transform.localScale = new Vector3(1, (float)0.5, 1);

        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }


        //Die if fall out of the world
        if(this.transform.position.y <= -10)
        {
            print("GameOver");
        }

    }
}
