using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class door_interact : MonoBehaviour
{
    private bool isUnlocked;
    public InventoryObject inventory;

    public void Start()
    {
    }

    public void Update()
    {
        if (isUnlocked)
        {
        
        }
    }
    public void Interact()
    {
        isUnlocked = true;




    }
}
