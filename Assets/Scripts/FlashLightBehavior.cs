using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightBehavior : MonoBehaviour
{
    public GameObject spotLight;
    public bool flashLightOn = false;
   
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashLightOn == false)
            {
                spotLight.SetActive(true);
                flashLightOn = true;
                System.Console.Write("FlashLight On");
            }

            else
            {
                spotLight.SetActive(false);
                flashLightOn = false;
            }
        }
    }
}
