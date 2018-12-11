using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class DevLog 
{
    public static void Log(string className, string text)
    {
        UnityEngine.Debug.Log("[" + className + "] " + "JOHN CENA. " + text);
    }
}
