using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_PushWall : MonoBehaviour
{

    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject wall4;
    public GameObject wall5;
    public GameObject wall6;
    public GameObject wall7;
    public GameObject wall8;



    void OnTriggerEnter(Collider collide)
    {

        if (collide.gameObject.tag == "Player")
        {
            wall1.GetComponent<SpikeWall>().isActive = true;
            wall2.GetComponent<SpikeWall>().isActive = true;
            wall3.GetComponent<SpikeWall>().isActive = true;
            wall4.GetComponent<SpikeWall>().isActive = true;
            wall5.GetComponent<SpikeWall>().isActive = true;
            wall6.GetComponent<SpikeWall>().isActive = true;
            wall7.GetComponent<SpikeWall>().isActive = true;
            wall8.GetComponent<SpikeWall>().isActive = true;

        }
       

    }

}
