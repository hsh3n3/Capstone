//Originally created by Hunter Hughes on 9-21-2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; //Character object in scene that we can make move

    public float speed = 12f; //movement speed
    public float gravity = -19.62f; //Earth's gravity * 2. Doubled because regular gravity feels a bit too "floaty" in video game world.
    public float jumpHeight = 3f;//jump height

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask; //To set a layer in unity to be 'ground' objects for collision checking.

    Vector3 velocity;
    bool isGrounded; //Checks if player is on ground or not

    // Update is called once per frame
    void Update()
    {
        //creates tiny sphere that will check collision with anything under the player. If so, will set isGrounded = true.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); 

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; //forces our player all the way to the ground.
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
