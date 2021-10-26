using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightBehavior : MonoBehaviour
{
    public GameObject spotLight;
    [HideInInspector]
    public GameObject flashlight;
    public bool flashLightOn = false;
    public InventoryObject inventory;
    private MeshRenderer flashlightRender;





    // Start is called before the first frame update
    void Start()
    {
        flashlight = GameObject.Find("PlayerFlashlight");

        flashlightRender = flashlight.GetComponent<MeshRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        var checkHasItem = flashlight.GetComponent<RemoveItemCheck>();
        if (inventory.RemoveItemCheck(checkHasItem.item) == true)
        {
            
            flashlightRender.enabled = true;


        }
        else
        {
            flashlightRender.enabled = false;
            spotLight.SetActive(false);
        }

        if(flashlightRender.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (flashLightOn == false)
                {
                    spotLight.SetActive(true);
                    flashLightOn = true;
                }

                else
                {
                    spotLight.SetActive(false);
                    flashLightOn = false;
                }
            }
        }
       
    }
}
