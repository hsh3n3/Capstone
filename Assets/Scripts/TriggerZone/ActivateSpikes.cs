using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSpikes : MonoBehaviour
{

    public GameObject spikeWall1;
    public GameObject spikeWall2;
    private void Start()
    {
       
    }

    void OnTriggerEnter(Collider collide)
    {

        if (collide.gameObject.tag == "Player")
        {
            spikeWall1.GetComponent<SpikeWall>().isActive = true;
            spikeWall2.GetComponent<SpikeWall>().isActive = true;
        }

    }
}
