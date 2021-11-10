using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform pivot;
    void Update()
    {
        Vector2 mp = Input.mousePosition;
        Vector2 s = new Vector2(Screen.width, Screen.height);
        Vector2 middle = new Vector2(Screen.width / 2, Screen.height / 2);
        /*
        if ((mp.x > s.x) || (mp.x < s.x))
        {
            mp.x = s.x;
        }
        if ((mp.y > s.y) || (mp.y < s.y))
        {
            mp.y = s.y;
        }
        */
        pivot.eulerAngles = new Vector3((mp.y/middle.y)*4, 120-(mp.x/middle.y)*6, 0);
    }
}
