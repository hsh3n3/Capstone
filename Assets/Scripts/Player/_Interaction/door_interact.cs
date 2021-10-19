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

    public void Start()
    {
        door = this.transform;
    }

    public void Update()
    {
        if (isUnlocked && doOnce)
        {
            door.localPosition = new Vector3(-2, 0, -2);
            door.localRotation = new Quaternion(0, -90, 0, 0);
            doOnce = false;
        }
    }
    public void Interact()
    {
        isUnlocked = true;
        doOnce = true;
    }
}
