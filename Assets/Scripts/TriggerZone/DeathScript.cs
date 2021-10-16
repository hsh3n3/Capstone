using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    private GameObject blackScreenParent;
    private Image blackScreen;
    private Transform upperEyeLid;
    private Transform lowerEyeLid;



    private float a = 0.0f;
    private bool isDead;

    public float speed = 0.33f;
    public float closeEyesSpeed = 1.0f;
    public bool instantDeath = false;


    private void Start()
    {
        blackScreenParent = GameObject.Find("BlackScreen");
        blackScreen = blackScreenParent.GetComponent(typeof(Image)) as Image;
        blackScreen.color = new Color(0, 0, 0, 0);
        isDead = false;
        upperEyeLid = GameObject.Find("First Person Player").GetComponent<Blink>().upperEyeLid;
        lowerEyeLid = GameObject.Find("First Person Player").GetComponent<Blink>().lowerEyeLid;

    }

    private void Update()
    {
        if (isDead && !instantDeath)
        {
            if (a < 1f)
            {
                blackScreen.color = new Color(0, 0, 0, a);
                a += speed * Time.deltaTime;
                upperEyeLid.transform.localPosition += new Vector3(0, -130, 0) * Time.deltaTime * closeEyesSpeed;
                lowerEyeLid.transform.localPosition += new Vector3(0, 130, 0) * Time.deltaTime * closeEyesSpeed;

            }
            if(a >= 1f)
            {
                blackScreen.color = new Color(0, 0, 0, 1.0f);
                Reload();
            }
        }
        else if(isDead && instantDeath)
        {
            if (a < 1f)
            {
                blackScreen.color = new Color(0, 0, 0, 1.0f);
                a += speed * Time.deltaTime;
            }
            if (a >= 1f)
            {
                Reload();
            }

        }


    }
    void OnTriggerEnter(Collider collide)
    {
        if(collide.gameObject.tag == "Player")
        {
            isDead = true;
        }

    }

    void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
