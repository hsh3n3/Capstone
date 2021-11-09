using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{

    public float accelerateSpeed = 50f;
    public float turnSpeed = 20f;

    private Rigidbody rbody;

    // Start is called before the first frame update
    void Start()
    {

        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w")){
           
            rbody.AddForce(transform.forward * -accelerateSpeed, ForceMode.Acceleration);
           
        }

        if (Input.GetKeyDown("s"))
        {

            rbody.AddForce(transform.forward * accelerateSpeed, ForceMode.Acceleration);
           
        }

        if (Input.GetKeyDown("a"))
        {

            rbody.AddForce(0f, turnSpeed * Time.deltaTime, 0f);
           
        }

       

    }
}
