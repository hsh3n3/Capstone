using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDemandAnimation : MonoBehaviour
{
    [System.Serializable]
    public struct Events
    {
        public Animator animator;
        public string ParameterName;
    }
    public Events[] events;

    public void Animate()
    {
        for (int i = 0; i < events.Length; i++)
        {
            events[i].animator.SetTrigger(events[i].ParameterName);
        }
    }

    public void Interact()
    {
        Animate();
    }
}
