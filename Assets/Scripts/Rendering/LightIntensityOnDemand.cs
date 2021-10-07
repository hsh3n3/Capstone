using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityOnDemand : MonoBehaviour {

    private float onIntensity, spd;
    private Light l;
    public bool working;

    [SerializeField] public Color haloColor;

    void Awake()
    {
        l = this.GetComponent<Light>();
        onIntensity = l.intensity;
        spd = onIntensity / 1.5f;
    }

    public void InstantDark()
    {
        l.intensity = 0f;
    }

    public void InstantBright()
    {
        l.intensity = onIntensity;
    }

    public void SmoothDark()
    {
        if (l.intensity != 0f)
        {
            l.intensity = Mathf.MoveTowards(l.intensity, 0f, spd / 3 * Time.deltaTime);
            working = true;
            Invoke("SmoothDark", 0.1f * Time.deltaTime);
        }
        else
        {
            working = false;
        }
    }

    public void SmoothLight()
    {
        if (l.intensity != onIntensity)
        {
            l.intensity = Mathf.MoveTowards(l.intensity, onIntensity, spd * Time.deltaTime);
            working = true;
            Invoke("SmoothLight", 0.1f * Time.deltaTime);
        }
        else
        {
            working = false;
        }
    }
    public void SmoothBlack()
    {

    }
    public void SmoothColor()
    {

    }
}
