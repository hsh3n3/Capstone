using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWall : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1.0f;
    private float timerReset;
    public float timer = 10f;
    private Transform spikes;
    public Transform rearCollider;
    private bool extending;
    [HideInInspector]
    public bool isActive = false;
    private float waitTime = 0f;


    public float WaitSeconds;
    public bool reverse;
    void Start()
    {
        spikes = this.transform;
        timerReset = timer;
        extending = true;
        waitTime = WaitSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }

        if (waitTime <= 0)
        {
            waitTime = 0;
        }

        if (isActive && waitTime <= 0)
        {
            if (!reverse)
            {
                if (timer > 0 && extending)
                {
                    spikes.transform.localPosition += new Vector3(0, 0, speed * Time.deltaTime);
                    rearCollider.transform.localPosition += new Vector3(0, 0, speed * Time.deltaTime);
                    timer -= Time.deltaTime * speed;
                }
                else if (timer <= 0 && extending)
                {
                    extending = false;
                    timer = 0;
                }
                else if (timer <= timerReset && !extending)
                {
                    spikes.transform.localPosition -= new Vector3(0, 0, speed * Time.deltaTime);
                    rearCollider.transform.localPosition -= new Vector3(0, 0, speed * Time.deltaTime);
                    timer += Time.deltaTime * speed;

                }
                else if (timer > timerReset)
                {
                    timer = timerReset;
                    extending = true;
                }


            }
            if (reverse && waitTime <= 0)
            {
                if (timer > 0 && extending)
                {
                    spikes.transform.localPosition += new Vector3(0, 0, -(speed * Time.deltaTime));
                    rearCollider.transform.localPosition += new Vector3(0, 0, -(speed * Time.deltaTime));
                    timer -= Time.deltaTime * speed;
                }
                else if (timer <= 0 && extending)
                {
                    extending = false;
                    timer = 0;
                }
                else if (timer <= timerReset && !extending)
                {
                    spikes.transform.localPosition -= new Vector3(0, 0, -(speed * Time.deltaTime));
                    rearCollider.transform.localPosition -= new Vector3(0, 0, -(speed * Time.deltaTime));
                    timer += Time.deltaTime * speed;

                }
                else if (timer > timerReset)
                {
                    timer = timerReset;
                    extending = true;
                }

            }
        }
        



    }

}
