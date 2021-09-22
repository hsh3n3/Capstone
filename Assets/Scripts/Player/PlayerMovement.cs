/*Originally created by Hunter Hughes on 9-21-2021
9-22-2021 Hunter - Updated to add upward collision checks to fix jumping under something, and stepOffset changes to make jumping up against vertical objects smoother.
                   Also added crouch ability, plus some more checks to keep air speed consistent between standing/crouching. Cannot jump while crouching. - need to fix standing up under objects above you while crouching.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; //Character object in scene that we can make move

    public float speed = 12f; //movement speed
    public float walkSpeed = 12f;
    public float crouchSpeed = 6f;
    public float gravity = -19.62f; //Earth's gravity * 2. Doubled because regular gravity feels a bit too "floaty" in video game world.
    public float jumpHeight = 3f;//jump height
    public Vector3 playerHeight = new Vector3(1,1,1);
    public Vector3 crouchHeight = new Vector3(1, 0.65f, 1);

    public Transform groundCheck; //To check if you are on the ground.
    public float groundDistance = 0.4f; 
    public LayerMask groundMask; //To set a layer in unity to be 'ground' objects for collision checking.

    public float originalStepOffset = 0.7f;
    public float originaljumpHeight = 3f;


    Vector3 velocity;
    bool isGrounded; //Checks if player is on ground or not
    bool isCrouching;
    bool isAirborn;

    // Update is called once per frame
    void Update()
    {
        //creates tiny sphere that will check collision with anything under the player. If so, will set isGrounded = true.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

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
            velocity.y = -2f; //forces our player all the way to the ground.
            controller.stepOffset = originalStepOffset; // make it so you can climb small objects, such as stairs when walking.
            isAirborn = false;
         }


        //Check to see if we are in the air
        if(velocity.y > 0)
        {
            isAirborn = true;
        }

        else
        {
            controller.stepOffset = 0; //Makes jumping up against a vertical object smooth by removing step offset.
        }

        if (Input.GetKey(KeyCode.C))
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }


        //if crouching, set crouching height and remove jump
        if (isCrouching)
        {
            //controller.height = crouchHeight;
            jumpHeight = 0f;
            speed = crouchSpeed;
            controller.gameObject.transform.localScale = crouchHeight;
        }
        //revert to non-crouch settings
        else
        {
            //controller.height = playerHeight;
            jumpHeight = originaljumpHeight;
            speed = walkSpeed;
            controller.gameObject.transform.localScale = playerHeight;
        }

        //keep speed consistent in the air
        if (isAirborn)
        {
            speed = walkSpeed;
        }



        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //can be called to allow movement of player object.

        controller.Move(move * speed * Time.deltaTime); //Calls move and multiplies by speed multiplier and uses deltaTime to make sure it is independent of framerate.

       //This allows us to jump in game
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime; //Gravity calculation for player velocity

        controller.Move(velocity * Time.deltaTime); //Making player fall downward.
    }
}
