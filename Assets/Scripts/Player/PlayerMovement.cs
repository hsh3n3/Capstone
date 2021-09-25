/*Originally created by Hunter Hughes on 9-21-2021
9-22-2021 Hunter - Updated to add upward collision checks to fix jumping under something, and stepOffset changes to make jumping up against vertical objects smoother.
                   Also added crouch ability, plus some more checks to keep air speed consistent between standing/crouching. Cannot jump while crouching. - need to fix standing up under objects too low

9-23-2021 Hunter - Fixed crouching by using spherecasting to detect overhead objects. 
*/



using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; //Character object in scene that we can make move
    public Transform groundCheck; //To check if you are on the ground.
    public LayerMask levelMask; // To set a layer in unity to be 'level' objects for overhead collision checking and ground checking.

    private float speed; //changing variable for player speed
    public float walkSpeed = 9; //Speed when walking
    public float sprintSpeed = 15; //Speed when sprinting
    public float crouchSpeed = 4.5f; //Speed when crouching

    public float jumpHeight = 3; //Jump height
    public float sprintJump = 1.5f; //Jump height when sprinting
    private float originaljumpHeight; //used to revert jump height

    public float gravity = -19.62f; //Earth's gravity * 2. Doubled because regular gravity feels a bit too "floaty" in video game world.

    public Vector3 playerHeight = new Vector3(1, 1, 1); //Height of the player when standing
    public Vector3 crouchHeight = new Vector3(1, 0.7f, 1); //Height of the player when crouching

    private float groundDistance = 0.3f; //Used for ground collision detection. Do not change
    private float originalStepOffset = 0.7f; // Used for climbing small obstacles.


    Vector3 velocity; //For physics
    bool isGrounded; //Checks if player is on ground or not
    bool isCrouching; //Checks if crouching
    bool isAirborn; //Checks if in the air
    bool isSprinting; //Checks if sprinting
    bool canStand; //Checks if you are allowed to stand up or not. (crouching under objects)


   void Start()
    {
        originaljumpHeight = jumpHeight;
        speed = walkSpeed;
    }
         


    void Update()
    {
        ///////////////////////////////////___________COLLISION CHECKS_____________/////////////////////////////////////////////////////

        //creates tiny sphere that will check collision with anything under the player. If so, will set isGrounded = true.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, levelMask);

        Vector3 p1 = transform.position + controller.center; //Start ray at player character

        //This condition is to check for things above the character. If you hit a ceiling, make the character fall back down.
        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            if (velocity.y > 0)
            {
                velocity.y = -velocity.y;
            }
        }

        if (isGrounded && velocity.y < 0)
        {
            controller.stepOffset = originalStepOffset; // make it so you can climb small objects, such as stairs when walking.
            velocity.y = 0;
        }

        if (isGrounded)
        {
            isAirborn = false;
        }
        else
        {
            isAirborn = true;
            controller.stepOffset = 0; //Makes jumping up against a vertical object smooth by removing step offset.
        }


        //Keep player crouching if there is a low object above their head.
        if (Input.GetKey(KeyCode.C) || !canStand)
        {
            isCrouching = true;
        }

        //Let player stand up if not.
        else
        {
            isCrouching = false;
        }


        //////////////////////////////////////___________SPRINT CHECKS________________//////////////////////////////////////////////////////
        // There are many to help with keeping momentum in the air and not allowing sprint under certain circumstances.

        //If sprint key, not crouching, and on the ground, allow sprint
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && isGrounded)
        {
            isSprinting = true;
        }
        
        //if not hitting sprint key, and not crouching but on the ground, turn off sprint.
        else if(!Input.GetKey(KeyCode.LeftShift) && !isCrouching && isGrounded)
        {
            isSprinting = false;
        }
        //If sprint key, and is crouching on the ground, DO NOT allow sprint.
        else if((Input.GetKey(KeyCode.LeftShift) && isCrouching && isGrounded))
        {
            isSprinting = false;
        }

        //If sprinting, change jump height and run speed.
        if (isSprinting)
        {
            jumpHeight = sprintJump;
            speed = sprintSpeed;
        }
        //Otherwise keep speed regular.
        else
        {
            speed = walkSpeed;
        }

        ////////////////////////////////////////__________CROUCH CHECKS______________//////////////////////////////////////////////////////////////

        //if crouching, set crouching height and remove jump
        if (isCrouching)
        {
            jumpHeight = 0f;
            speed = crouchSpeed;

            //Slow transform to make crouch animation smoother
            if(controller.gameObject.transform.localScale.y > crouchHeight.y)
            {
                controller.gameObject.transform.localScale -= (new Vector3(0,1,0) * Time.deltaTime);
                controller.Move(new Vector3(0, -1.75f, 0) * Time.deltaTime);
            }
            //Making sure player is exactly crouch height
            else 
            {
                controller.gameObject.transform.localScale = crouchHeight;
            }
           
            //Overhead Collision Detection to prevent standing under low objects
            RaycastHit upHit;
            if (Physics.SphereCast(p1, controller.radius, transform.up, out upHit, 2.5f, levelMask))
            {   
                canStand = false;
            }
            else
            {
                canStand = true;
            }
        }



        //If not crouching, stand up
        else
        {
            jumpHeight = originaljumpHeight;

            //Slow transform to make uncrouch animation smoother
            if (controller.gameObject.transform.localScale.y < playerHeight.y)
            {
                controller.gameObject.transform.localScale += (new Vector3(0, 1, 0) * Time.deltaTime);
                controller.Move(new Vector3(0, 1.75f, 0) * Time.deltaTime);
            }
            //Making sure player is exactly normal height
            else
            {
                controller.gameObject.transform.localScale = playerHeight;

            }

            canStand = true;
            isCrouching = false;
        }

        //Airborn crouch checks to maintain original speed instead of crouching speed
        if (isAirborn && isCrouching && !isSprinting)
        {
            speed = walkSpeed;
        }
        else if (isAirborn && isCrouching && isSprinting)
        {
            speed = sprintSpeed;
        }



        /////////////////////////////////////////____________________MOVEMENT________________////////////////////////////////////////////////

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Called by controller.Move to tell unity HOW to move the character.
        Vector3 move = transform.right * x + transform.forward * z;

        //Actual movement considering speed multiplier.
        controller.Move(move * speed * Time.deltaTime); 

        //Jumping
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Falling
        if (isAirborn)
        {
            velocity.y += gravity * Time.deltaTime; //Gravity calculation for player velocity
        }

        //Downward Gravity on player movement
        controller.Move(velocity * Time.deltaTime);

    }
}
