//Initially created by Hunter Hughes 09-21-21

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 300f; // so sensitivity can be changed later on.

    public Transform playerBody; //so that we have something to assign the player camera to.

   // [HideInInspector]
    public float xRotation = 0f;

    private float timer = 2.5f;
    private float a = 1.0f;

    private GameObject blackScreenParent;
    private Image blackScreen;

    private PlayerStatus status;

    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();
       // status.SetStatus(PlayerStatus.States.walking);//set status to 'walking' which locks the cursor and hides it

        xRotation = -90f; //Starting rotation looking up to go along with player in bed.
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        blackScreenParent = GameObject.Find("BlackScreen");
        blackScreen = blackScreenParent.GetComponent(typeof(Image)) as Image;
        blackScreen.color = new Color(0, 0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY; //-= so that we can look up or down normally, can switch to += if we wish to invert look controls.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Used to clamp rotation, so that you cannot look up or down more than 180 degrees.




        if(timer > 0) // Keeping player looking up while eyes open
        {
            xRotation = -90f;
            timer -= 1 * Time.deltaTime;
        }
        else {
            playerBody.Rotate(Vector3.up * mouseX); //Horizontal rotation
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical Rotation.
        }
        if (a > 0f) // Fade in from black screen
        {
           // blackScreen.color = new Color(0, 0, 0, a);
            a -= 0.5f * Time.deltaTime;
        }
//<<<<<<< HEAD
//       // PlayerStatus.States s = status.GetStatus();
//       /* if (s == PlayerStatus.States.walking)
//        {
//            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical Rotation.
//        }
//        else if (s == PlayerStatus.States.antigravity)
//        {

//        } */

//=======
//        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical Rotation.
//>>>>>>> 1d42a6ec9d93d364bfc6ec11f794671e019081bb







    }
}
