using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSpikes : MonoBehaviour
{

    public GameObject spikeWall1;
    public GameObject spikeWall2;

    public AudioSource StartSound;
    public AudioSource LoopSound;

    private bool doOnce;
    private float timer = 1.61f;
    private void Start()
    {


    }

    private void Update()
    {
        if (doOnce == true)
        {
            StartSound.Play();
            LoopSound.Play();
            doOnce = false;
        }

    }

    void OnTriggerEnter(Collider collide)
    {

        if (collide.gameObject.tag == "Player")
        {
            spikeWall1.GetComponent<SpikeWall>().isActive = true;
            spikeWall2.GetComponent<SpikeWall>().isActive = true;
            doOnce = true;

        }

    }
}
