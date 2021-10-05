//Initially created by Hunter Hughes 09-21-21

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 300f; // so sensitivity can be changed later on.

    public Transform playerBody; //so that we have something to assign the player camera to.

    float xRotation = 0f;

    private float timer = 2.5f;
    private float a = 1.0f;

    public GameObject blackScreenParent;
    public Image blackScreen;


    // Start is called before the first frame update
    void Start()
    {
        xRotation = -90f; //Starting rotation looking up to go along with player in bed.
        Cursor.lockState = CursorLockMode.Locked; //Locks cursor to middle of the screen and hides it.
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        blackScreenParent = GameObject.Find("BlackScreen");
        blackScreen = blackScreenParent.GetComponent(typeof(Image)) as Image;
        blackScreen.color = new Color(0, 0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Time.deltaTime is used here to ensure sensitivity doesn't change with framerate.
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; //-= so that we can look up or down normally, can switch to += if we wish to invert look controls.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Used to clamp rotation, so that you cannot look up or down more than 180 degrees.

        if(timer > 0) // Keeping player looking up while eyes open
        {
            xRotation = -90f;
            timer -= 1 * Time.deltaTime;
        }
        else {
            playerBody.Rotate(Vector3.up * mouseX); //Horizontal rotation
        }
        if (a > 0f)
        {
            blackScreen.color = new Color(0, 0, 0, a);
            a -= 0.25f * Time.deltaTime;
        }
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical Rotation.



    }
}
