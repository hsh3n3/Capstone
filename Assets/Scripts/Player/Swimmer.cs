using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swimmer : MonoBehaviour
{
    PlayerMovement Pm;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.fog = false;
        RenderSettings.fogColor = new Color(0.2f, 0.4f, 0.8f);
        RenderSettings.fogDensity = 0.04f;


        Pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.fog = isUnderWater();


        if (isUnderWater())
        {
            Pm.walkSpeed = 2;
            Pm.runSpeed = 4;
            Pm.jumpSpeed = 3;
            Pm.gravity = 4;
        }
    }

    bool isUnderWater()
    {
        return gameObject.transform.position.y < -2f;
    }
}
