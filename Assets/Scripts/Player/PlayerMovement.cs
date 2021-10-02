/*Originally created by Hunter Hughes on 9-21-2021
9-22-2021 Hunter - Updated to add upward collision checks to fix jumping under something, and stepOffset changes to make jumping up against vertical objects smoother.
                   Also added crouch ability, plus some more checks to keep air speed consistent between standing/crouching. Cannot jump while crouching. - need to fix standing up under objects too low

9-23-2021 Hunter - Fixed crouching by using spherecasting to detect overhead objects. 
*/


using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 6.0f;

    public float runSpeed = 11.0f;

    public float crouchSpeed = 3.0f;

    // If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster
    public bool limitDiagonalSpeed = true;

    // If checked, the run key toggles between running and walking. Otherwise player runs if the key is held down and walks otherwise
    // There must be a button set up in the Input Manager called "Run"
    public bool toggleRun = false;

    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    // Units that player can fall before a falling damage function is run. To disable, type "infinity" in the inspector
    public float fallingDamageThreshold = 10.0f;

    // If the player ends up on a slope which is at least the Slope Limit as set on the character controller, then he will slide down
    public bool slideWhenOverSlopeLimit = false;

    // If checked and the player is on an object tagged "Slide", he will slide down it regardless of the slope limit
    public bool slideOnTaggedObjects = false;

    public float slideSpeed = 12.0f;

    // If checked, then the player can change direction while in the air
    public bool airControl = false;

    // Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
    public float antiBumpFactor = .75f;

    // Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping
    public int antiBunnyHopFactor = 1;

    public Vector3 playerHeight = new Vector3(1, 1, 1); //Height of the player when standing
    public Vector3 crouchHeight = new Vector3(1, 0.7f, 1); //Height of the player when crouching

    public LayerMask levelMask; // To set a layer in unity to be 'level' objects for overhead collision checking and ground checking.

    private Vector3 moveDirection = Vector3.zero;
    private bool grounded = false;
    public CharacterController controller;
    private Transform myTransform;
    private float speed;
    private RaycastHit hit;
    private float fallStartLevel;
    private bool falling;
    private float slideLimit;
    private float rayDistance;
    private Vector3 contactPoint;
    private bool playerControl = false;
    private int jumpTimer;
    private bool canStand;
    private bool isCrouching;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        myTransform = transform;
        speed = walkSpeed;
        rayDistance = controller.height * .5f + controller.radius;
        slideLimit = controller.slopeLimit - .1f;
        jumpTimer = antiBunnyHopFactor;
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
        float inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;

        Vector3 p1 = transform.position + controller.center; //Start ray at player character

        if (grounded)
        {
            bool sliding = false;
            // See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
            // because that interferes with step climbing amongst other annoyances
            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
            {
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    sliding = true;
            }
            // However, just raycasting straight down from the center can fail when on steep slopes
            // So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
            else
            {
                Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                    sliding = true;
            }

            // If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
            if (falling)
            {
                falling = false;
                if (myTransform.position.y < fallStartLevel - fallingDamageThreshold)
                    FallingDamageAlert(fallStartLevel - myTransform.position.y);
            }

            // If running isn't on a toggle, then use the appropriate speed depending on whether the run button is down
            if (!toggleRun)
                speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            // If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
            if ((sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide"))
            {
                Vector3 hitNormal = hit.normal;
                moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
                moveDirection *= slideSpeed;
                playerControl = false;
            }
            // Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
            else
            {
                moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
                moveDirection = myTransform.TransformDirection(moveDirection) * speed;
                playerControl = true;
            }

            // Jump! But only if the jump button has been released and player has been grounded for a given number of frames
            if (!Input.GetButton("Jump"))
                jumpTimer++;
            else if (jumpTimer >= antiBunnyHopFactor)
            {
                moveDirection.y = jumpSpeed;
                jumpTimer = 0;
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

            //if crouching, set crouching height and remove jump
            if (isCrouching)
            {

                //Slow transform to make crouch animation smoother
                if (controller.gameObject.transform.localScale.y > crouchHeight.y)
                {
                    controller.gameObject.transform.localScale -= (new Vector3(0, 1, 0) * Time.deltaTime);
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
        }
        else
        {
            // If we stepped over a cliff or something, set the height at which we started falling
            if (!falling)
            {
                falling = true;
                fallStartLevel = myTransform.position.y;
            }

            // If air control is allowed, check movement but don't touch the y component
            if (airControl && playerControl)
            {
                moveDirection.x = inputX * speed * inputModifyFactor;
                moveDirection.z = inputY * speed * inputModifyFactor;
                moveDirection = myTransform.TransformDirection(moveDirection);
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller, and set grounded true or false depending on whether we're standing on something
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    void Update()
    {
        // If the run button is set to toggle, then switch between walk/run speed. (We use Update for this...
        // FixedUpdate is a poor place to use GetButtonDown, since it doesn't necessarily run every frame and can miss the event)
        if (toggleRun && grounded && Input.GetKey(KeyCode.LeftShift))
            speed = (speed == walkSpeed ? runSpeed : walkSpeed);
    }

    // Store point that we're in contact with for use in FixedUpdate if needed
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        contactPoint = hit.point;
    }

    // If falling damage occured, this is the place to do something about it. You can make the player
    // have hitpoints and remove some of them based on the distance fallen, add sound effects, etc.
    void FallingDamageAlert(float fallDistance)
    {
        print("Ouch! Fell " + fallDistance + " units!");
    }
}












/*using UnityEngine;

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
    private Vector3 move;

    public float gravity = -19.62f; //Earth's gravity * 2. Doubled because regular gravity feels a bit too "floaty" in video game world.

    public Vector3 playerHeight = new Vector3(1, 1, 1); //Height of the player when standing
    public Vector3 crouchHeight = new Vector3(1, 0.7f, 1); //Height of the player when crouching

    private float originalStepOffset = 0.7f; // Used for climbing small obstacles.
    public float slideSpeed = 12.0f;


    Vector3 velocity; //For physics
    bool isGrounded; //Checks if player is on ground or not
    bool isCrouching; //Checks if crouching
    bool isAirborn; //Checks if in the air
    bool isSprinting; //Checks if sprinting
    bool canStand; //Checks if you are allowed to stand up or not. (crouching under objects)
    bool isSliding;


   void Start()
    {
        originaljumpHeight = jumpHeight;
        speed = walkSpeed;
    }
         


    void Update()
    {

        SlopeDirection();
        ///////////////////////////////////___________COLLISION CHECKS_____________/////////////////////////////////////////////////////

        //creates tiny sphere that will check collision with anything under the player. If so, will set isGrounded = true.
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, levelMask);



        Vector3 p1 = transform.position + controller.center; //Start ray at player character

        RaycastHit downHit;
        if (Physics.SphereCast(p1, controller.radius, -transform.up, out downHit, 2f, levelMask) && SlopeDirection() < 45f)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

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

        if (isSprinting)
        {
            jumpHeight = sprintJump;
        }

        /////////////////////////////////////////____________________MOVEMENT________________////////////////////////////////////////////////

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        RaycastHit hit;
        Physics.Raycast(controller.transform.position, Vector3.down, out hit, Mathf.Infinity);

        // Saving the normal
        Vector3 n = hit.normal;

        // Crossing my normal with the player's up vector (if your player rotates I guess you can just use Vector3.up to create a vector parallel to the ground
        Vector3 groundParallel = Vector3.Cross(transform.up, n);

        // Crossing the vector we made before with the initial normal gives us a vector that is parallel to the slope and always pointing down
        Vector3 slopeParallel = Vector3.Cross(groundParallel, n);
        Debug.DrawRay(hit.point, slopeParallel * 10, Color.green);

        // Just the current angle we're standing on
        float currentSlope = Mathf.Round(Vector3.Angle(hit.normal, transform.up));
        Debug.Log(currentSlope);

        // If the slope is on a slope too steep and the player is Grounded the player is pushed down the slope.
        if (currentSlope >= 45f && isGrounded)
        {
            isSliding = true;
            // transform.position += slopeParallel.normalized / 2;
        }

        //Called by controller.Move to tell unity HOW to move the character.
        if (isSliding)
        {
                Vector3 hitNormal = hit.normal;
                move= new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
                controller.transform.position *= slideSpeed;
        }
        



        move = transform.right * x  + transform.forward * z ;

        //Actual movement considering speed multiplier.
        controller.Move(move * speed * Time.deltaTime); 

        //Jumping
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //Falling
        if (isAirborn || !isGrounded)
        {
            velocity.y += gravity * Time.deltaTime; //Gravity calculation for player velocity
        }

        //Downward Gravity on player movement
        controller.Move(velocity * Time.deltaTime);


    }

   /* private float SlopeDirection()
    {
        // Raycast with infinite distance to check the slope directly under the player no matter where they are

        RaycastHit hit;
        Physics.Raycast(controller.transform.position, Vector3.down, out hit, Mathf.Infinity);

        // Saving the normal
        Vector3 n = hit.normal;

        // Crossing my normal with the player's up vector (if your player rotates I guess you can just use Vector3.up to create a vector parallel to the ground
        Vector3 groundParallel = Vector3.Cross(transform.up, n);

        // Crossing the vector we made before with the initial normal gives us a vector that is parallel to the slope and always pointing down
        Vector3 slopeParallel = Vector3.Cross(groundParallel, n);
        Debug.DrawRay(hit.point, slopeParallel * 10, Color.green);

        // Just the current angle we're standing on
        float currentSlope = Mathf.Round(Vector3.Angle(hit.normal, transform.up));
        Debug.Log(currentSlope);

        // If the slope is on a slope too steep and the player is Grounded the player is pushed down the slope.
        if (currentSlope >= 45f && isGrounded)
        {
            isSliding = true;
           // transform.position += slopeParallel.normalized / 2;
        }

        // If the player is standing on a slope that isn't too steep, is grounded, as is not sliding anymore we start a function to count time
        else if (currentSlope < 45 && isGrounded && isSliding)
        {
            TimePassed();

            // If enough time has passed the sliding stops. There's no need for these last two if statements, the thing works already, but it's nicer to have the player slide for a little bit more once they get back on the ground
            if (currentSlope < 45 && isGrounded && isSliding && TimePassed() > 1f)
            {
                isSliding = false;
            }
        }

        return currentSlope;
    }



float TimePassed()
    {
        float timePassed = Time.deltaTime;
        return timePassed;
    }
}

    */



