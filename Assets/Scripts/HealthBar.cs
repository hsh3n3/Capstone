using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider Healthslider;
  



    public void SetHealth(float health)
    {
        print("Current Health:" + health);
        Healthslider.value = health;
    }



}
