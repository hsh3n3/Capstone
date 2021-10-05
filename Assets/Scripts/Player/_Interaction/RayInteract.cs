using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayInteract : MonoBehaviour
{
    public float RayRange = 5f; //how far to cast ray
    LayerMask Layers; //the layers that the raycast can hit. mautomatically set to only the "interactable" layer
    public Material outlineMaterial;
    public Text itemText;
    public Text crosshair;
    string FunctionName = "Interact"; //function to be called on the object we're interacting with. has to exist in a component attached to the object

    private GameObject canvas;
    public Text pickupText;
    public InventoryObject inventory;

    private float pickupTextCountDown;
    void Awake()
    {
        Layers = -1;
        Layers = 1 << 9; //layermasks are so unnecessarily complicated
        itemText.enabled = true;
        pickupText.text = "";
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
                //if (objComponents[i].GetType().GetMethod(FunctionName) != null //if the object has an Interact function
               // {
                    if (!hitObj.GetComponent<InteractOutline>()) //let the outline script do its thing
                    {
                        hitObj.AddComponent<InteractOutline>();
                    }

                    //hunter stick your canvas scripting here
                    itemText.text = hitObj.name;
                    itemText.enabled = true;

                    crosshair.text = "o";

                    //Picking up Items in game world
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        //If this item can be picked up, pick up and destroy.
                        var item = hitObj.GetComponent<AddItem>();
                        if (item)
                        {
                            inventory.AddItem(item.item, 1); //Add to inventory
                            Destroy(hitObj.gameObject); //Destroy item you just picked up from game world
                            pickupText.text = ("Picked up " + item.item.itemName); //Displays item you picked up
                            pickupTextCountDown = 1f; //To start fade out of text over time
                        }
                        else if (objComponents[i].GetType().GetMethod(FunctionName) != null) //If item has a function attached 
                        {
                            objComponents[i].GetType().GetMethod(FunctionName).Invoke(objComponents[i], null); //if FunctionName is found on any components attached to the hitObj, call it
                        }
                    }
            }
        }
        else
        {   //Revert to normal settings if not looking at object
            itemText.enabled = false;
            itemText.text = "";
            crosshair.text = ".";
        }
        if(pickupTextCountDown > 0) //Start countdown to make item pickup text fade out over time
        {
            pickupText.color = new Color(1, 1, 1, pickupTextCountDown);
            pickupTextCountDown -= .25f * Time.deltaTime;
        }
        if(pickupTextCountDown <= 0) //(just for good measure) once count down is over, return object text to be nothing.
        {
            pickupText.text = "";
        }
    }
}
