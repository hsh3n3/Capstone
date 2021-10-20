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
            doOnce = false;
        }
    }
    public void Interact()
    {
        Debug.Log("Door open");
        isUnlocked = true;
        doOnce = true;
        door.localPosition += new Vector3(-2, 0, -2);
        door.Rotate(new Vector3(0, -90, 0));
    }
}
