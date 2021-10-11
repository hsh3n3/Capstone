using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LightIntensityOnDemand))]
public class LightRendering : MonoBehaviour {

    public enum LightState { pixel, vertex, off, error };
    public enum halo_options { WhenLightOff, WhenLightOn};

    public float maxDist = -1f, //if maxDist == -1, max range is 10x light range
        PixelPercent = 0.5f, 
        tickTime = 0.1f; //tick instead of update for better performance
    [Range(0.0f, 1.0f)] public float OcclusionAllowance = 0.9f;
    public bool timeDependent;
    public Behaviour halo; //leave null for no halo options (if there IS a halo associated with this light, it will be unaffected by this script)
    public halo_options HaloOptions;

    private LightIntensityOnDemand pix, vert;
    private Camera cam;
    private Behaviour realHalo;

    private float lRange;
    private float disto;

    void Awake()
    {
        cam = Camera.main;

        disto = Vector3.Distance(cam.transform.position, this.transform.position);

        pix = this.GetComponent<LightIntensityOnDemand>();
            GameObject newVertexLightObj = Instantiate(new GameObject("vertlight"), pix.transform) as GameObject;
            newVertexLightObj.transform.localPosition = new Vector3(0, 0, 0);
            newVertexLightObj.AddComponent(typeof(Light));
            Light newVertexLight = newVertexLightObj.GetComponent<Light>();
            newVertexLight.color = this.GetComponent<Light>().color;
            newVertexLight.type = this.GetComponent<Light>().type;
            newVertexLight.range = this.GetComponent<Light>().range;
            lRange = this.GetComponent<Light>().range;
            newVertexLight.intensity = this.GetComponent<Light>().intensity * 1.5f;
            newVertexLight.cullingMask = this.GetComponent<Light>().cullingMask;
            newVertexLight.gameObject.AddComponent(typeof(LightIntensityOnDemand));
            vert = newVertexLight.GetComponent<LightIntensityOnDemand>();

            this.GetComponent<Light>().renderMode = LightRenderMode.ForcePixel;
            newVertexLight.gameObject.GetComponent<Light>().renderMode = LightRenderMode.ForceVertex;

        if (maxDist == -1)
        {
            maxDist = pix.gameObject.GetComponent<Light>().range * 10;
        }

        Invoke("Tick", tickTime);
    }

    void Tick()
    {
        LightState state = GetLightState();
            if (state == LightState.off)
            {
                if (!LightWorking())
                {
                    vert.SmoothDark();
                    pix.InstantDark();
                }
            }
            else if (state == LightState.vertex)
            {
                if (!LightWorking())
                {
                    vert.SmoothLight();
                    pix.SmoothDark();
                }
            }
            else if (state == LightState.pixel)
            {
                if (!LightWorking())
                {
                    vert.SmoothDark();
                    pix.SmoothLight();
                }
            }
            else if (state == LightState.error)
            {
                if (!LightWorking())
                {
                    vert.InstantBright();
                    pix.InstantDark();
                }
            }
            if (halo != null)
            {
                HaloState();
            }
        Invoke("Tick", tickTime);
    }

    public LightState GetLightState()
    {
        disto = Vector3.Distance(cam.transform.position, this.transform.position);
        float actualMaxDist = maxDist * QualitySettings.lodBias;
        float PixelPercentDisto = actualMaxDist - (actualMaxDist * (1 - PixelPercent));

        if (disto >= actualMaxDist)                                   //If so far away the light turns off
        {
            return LightState.off;
        }
        else if (disto < actualMaxDist && LightInCameraView()) //if close enough for low-quality lighting
        {
            if (disto >= PixelPercentDisto)
            {
                return LightState.vertex;
            }
            else                           //if close enough for high-quality lighting
            {
                return LightState.pixel;
            }

        }
        else                                                          //if some error in distance calc, default to vertex lighting
        {
            return LightState.off;
        }
    }

    void HaloState()
    {
        if (GetLightState() == LightState.off) // when light off
        {
            if (HaloOptions == halo_options.WhenLightOn)
            {
                halo.enabled = false;
            }
            else
            {
                halo.enabled = true;
            }
        }
        else if (GetLightState() == LightState.pixel || GetLightState() == LightState.vertex) //when light on
        {
            if (HaloOptions == halo_options.WhenLightOn)
            {
                halo.enabled = true;
            }
            else
            {
                halo.enabled = false;
            }
        }
        else
        {
            halo.enabled = false;
        }
    }
    bool LightWorking()
    {
        if (vert.working == false && pix.working == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool LightInCameraView()
    {
        if (Mathf.Abs(Vector3.Angle(this.transform.position - Camera.main.transform.position, Camera.main.transform.forward)) > 90 && disto > lRange * OcclusionAllowance)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
}

