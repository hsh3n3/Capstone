using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class door_interact : MonoBehaviour
{
    private bool isUnlocked = false;
    private Transform door;
    private bool doOnce = false;

    public AudioSource doorOpenAudio;

    public void Start()
    {
        door = this.transform;
    }

    public void Update()
    {
        if (isUnlocked && doOnce)
        {
            doOnce = false;
        }
    }
    public void Interact()
    {
        isUnlocked = true;
        doOnce = true;
        doorOpenAudio.Play();
        GetComponent<Animation>().Play();
        door.gameObject.layer = 0;
    }
}
