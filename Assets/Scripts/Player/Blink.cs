using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform upperEyeLid;
    public Transform lowerEyeLid;


    // Update is called once per frame
    void Update()
    {
        if(upperEyeLid.transform.localPosition.y < 625)
        {
            upperEyeLid.transform.localPosition += new Vector3(0, 150, 0) * Time.deltaTime;
            lowerEyeLid.transform.localPosition += new Vector3(0, -150, 0) * Time.deltaTime;
        }
        
    }
}
