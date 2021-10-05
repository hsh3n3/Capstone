using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour
{
    public GameObject blackScreenParent;
    public Image blackScreen;




   

    private float a = 0.0f;
    private bool isDead;

    private void Start()
    {
        blackScreenParent = GameObject.Find("BlackScreen");
        blackScreen = blackScreenParent.GetComponent(typeof(Image)) as Image;
        blackScreen.color = new Color(0, 0, 0, 0);
        isDead = false;

}

    private void Update()
    {
        if (isDead)
        {
            if (a < 1f)
            {
                blackScreen.color = new Color(0, 0, 0, a);
                a += 0.5f * Time.deltaTime;
            }
            if(a >= 1f)
            {
                blackScreen.color = new Color(0, 0, 0, 1.0f);
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
