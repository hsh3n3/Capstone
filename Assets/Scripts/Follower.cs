using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public GameObject ObjectToFollow;
    public bool FollowCamera;

    [Range(0,100)]
    public float smoothness;

    public Vector3 offset;

    private void Start()
    {
        if (FollowCamera)
        {
            ObjectToFollow = Camera.main.gameObject;
        }
    }
    void LateUpdate()
    {
        Vector3 target = ObjectToFollow.transform.position;
        transform.position = new Vector3
            (
                ((smoothness != 0) ? Mathf.Lerp(transform.position.x, target.x, smoothness * Time.deltaTime) : target.x) + offset.x,
                ((smoothness != 0) ? Mathf.Lerp(transform.position.y, target.y, smoothness * Time.deltaTime) : target.y) + offset.y,
                ((smoothness != 0) ? Mathf.Lerp(transform.position.z, target.z, smoothness * Time.deltaTime) : target.z) + offset.z
            );
    }
}
