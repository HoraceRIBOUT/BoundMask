using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Input_Utils 
{
    public static bool GetKey(List<KeyCode> keyList)
    {
        foreach(KeyCode key in keyList)
        {
            if (Input.GetKey(key))
                return true;
        }
        return false;
    }
    public static bool GetKeyDown(List<KeyCode> keyList)
    {
        foreach (KeyCode key in keyList)
        {
            if (Input.GetKeyDown(key))
                return true;
        }
        return false;
    }
    public static bool GetKeyUp(List<KeyCode> keyList)
    {
        foreach (KeyCode key in keyList)
        {
            if (Input.GetKeyUp(key))
                return true;
        }
        return false;
    }
}
