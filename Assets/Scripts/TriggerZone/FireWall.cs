using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : MonoBehaviour
{
    private Transform fireWallTransform;
    public float fireWallSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        fireWallTransform = GameObject.Find("FireWallCollider").transform;
    }

    // Update is called once per frame
    void Update()
    {
        fireWallTransform.transform.localPosition += new Vector3(0, 0, fireWallSpeed * Time.deltaTime);
    }
}
