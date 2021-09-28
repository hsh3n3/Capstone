using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteract : MonoBehaviour
{
    public float RayRange = 5f; //how far to cast ray
    LayerMask Layers; //the layers that the raycast can hit. mautomatically set to only the "interactable" layer

    string FunctionName = "Interact"; //function to be called on the object we're interacting with. has to exist in a component attached to the object

    void Awake()
    {
        Layers = -1;
        Layers = 1 << 9; //layermasks are so unnecessarily complicated
    }
    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * RayRange, Color.green);
        if (Physics.Raycast(ray, out hit, RayRange, Layers, QueryTriggerInteraction.Ignore)) //cast ray out to RayRange and ignore layer 8 (the player)
        {
            GameObject hitObj = hit.collider.gameObject;
            Component[] objComponents = hitObj.GetComponents(typeof(Component));
            for (int i = 0; i < objComponents.Length; i++)
            {
                if (objComponents[i].GetType().GetMethod(FunctionName) != null)
                {
                    objComponents[i].GetType().GetMethod(FunctionName).Invoke(objComponents[i], null); //if FunctionName is found on any components attached to the hitObj, call it
                }
            }
        }
    }
}
