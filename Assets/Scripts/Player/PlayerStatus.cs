using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public enum States { 
        walking, //standard play
        swimming, //swimming
        antigravity, //space
        cutscene, //first-person cutscene
        pause //pause
    }
    private States status;

    public void SetStatus(States s)
    {
        status = s;
        if (s == States.walking || s == States.swimming || s == States.antigravity || s == States.cutscene)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public States GetStatus()
    {
        return status;
    }
}
