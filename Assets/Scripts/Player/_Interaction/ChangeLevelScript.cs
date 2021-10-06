using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevelScript : MonoBehaviour
{
    public string LevelName;
    private Transform player;
    private Transform bed;

    private float timer = 5;

    private Vector3 moveDirection;

    private PlayerMovement playerMovement;
    private MouseLook look;
    private Transform playerCamera;

    private bool getInBed = false;

    public Transform upperEyeLid;
    public Transform lowerEyeLid;

    public void Start()
    {
        player = GameObject.Find("First Person Player").transform;
        bed = GameObject.Find("Bed").transform;

        playerMovement = GameObject.Find("First Person Player").GetComponent<PlayerMovement>();
        look = GameObject.Find("First Person Camera").GetComponent<MouseLook>();
        playerCamera = GameObject.Find("First Person Camera").transform;


        //upperEyeLid = GameObject.Find("UpperEyeLid").transform;
        //lowerEyeLid = GameObject.Find("LowerEyeLid").transform;

    }

    public void Update()
    {
        if (timer > 0 && getInBed) // Keeping player looking up while eyes open
        {
            //look.enabled = false;
            look.xRotation = 0;
            player.rotation = Quaternion.Euler(-90, 0, 0);
            playerMovement.enabled = false;
            player.position = new Vector3(bed.transform.position.x, bed.transform.position.y + 1f, bed.transform.position.z + 2f);
            playerCamera.rotation = Quaternion.Euler(-90, 0, 0);

          
            upperEyeLid.transform.localPosition += new Vector3(0, -150, 0) * Time.deltaTime;
            lowerEyeLid.transform.localPosition += new Vector3(0, 150, 0) * Time.deltaTime;

               
           

            timer -= 1 * Time.deltaTime;
        }
        if(timer <= 0)
        {
            SceneManager.LoadScene(LevelName, LoadSceneMode.Single);
        }
    }
    public void Interact()
    {
        getInBed = true;
        

        

    }
}
