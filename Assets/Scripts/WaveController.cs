using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public static WaveController instance;

    public float amplitude = 1f;
    public float length = 2f;
    public float speed = 1f;
    public float offest = 0f;



    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance Exists already, destroying object.");
            Destroy(this);
        }
    }

    private void Update()
    {
        offest += Time.deltaTime * speed;
    }

    public float GetWaveHeight(float _x)
    {
        return amplitude * Mathf.Sin(_x / length + offest);
    }


}
