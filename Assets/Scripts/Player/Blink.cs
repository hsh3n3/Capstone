using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform upperEyeLid;
    public Transform lowerEyeLid;

    public float timer = 3f;



    private void Start()
    {
        upperEyeLid = GameObject.Find("UpperEyeLid").transform;
        lowerEyeLid= GameObject.Find("LowerEyeLid").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            upperEyeLid.transform.localPosition += new Vector3(0, 130, 0) * Time.deltaTime;
            lowerEyeLid.transform.localPosition += new Vector3(0, -130, 0) * Time.deltaTime;

            timer -= Time.deltaTime;
        }
        
    }
}
