using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightViewChecker : MonoBehaviour
{

    public int LightInViewCount;

    private LightRendering[] lr;

    public int vertLights, pixLights;

    void Awake()
    {
        lr = FindObjectsOfType<LightRendering>();
        LightInViewCount = lr.Length;
    }

    void Update()
    {
        foreach (LightRendering l in lr)
        {
            if (l.LightInCameraView())
            {
                LightInViewCount++;
                //if (l.GetLightState() == LightRendering.LightState.pixel)
                //{
                //    pixLights++;
                //    vertLights--;
                //}
                //else if (l.GetLightState() == LightRendering.LightState.vertex)
                //{

                //}

            }
            else
            {
                LightInViewCount--;
            }
        }
    }
   
}
